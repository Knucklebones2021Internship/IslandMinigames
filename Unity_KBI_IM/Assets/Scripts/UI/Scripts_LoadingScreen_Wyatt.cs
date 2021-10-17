using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager class to handle loading screen operations, such as the asynchronous loading of the Level, 
/// UI loading components, and animation selections. 
/// </summary>
[DefaultExecutionOrder(-50)]
public class Scripts_LoadingScreen_Wyatt : Scripts_BaseManager_Wyatt {
	[SerializeField] Slider loadingSlider;
	[SerializeField] TextMeshProUGUI tipText;
	[SerializeField] string[] tips;

	string sceneName = "TestLab_BubbleController";

	// the minimum amount of time to show the loading screen, if the level loads before this we will still wait
	float minimumLoadTime = 3f;
	float loadTime;

	protected override void Awake() {
		base.Awake();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	void Start() {
		tipText.text = tips[Random.Range(0, tips.Length)];

		PhotonNetwork.AutomaticallySyncScene = false;
		PhotonNetwork.IsMessageQueueRunning = false;
		StartCoroutine(LoadLevel(sceneName));
	}

	IEnumerator LoadLevel(string sceneToLoad) {
		loadTime = 0f;

		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
		asyncOperation.allowSceneActivation = false;
		// loop until we've finished loading the scene
		while (!asyncOperation.isDone) {
			loadTime += Time.deltaTime;

			// update UI with the slower of our two load time measures
			float progress = Mathf.Min(asyncOperation.progress / 0.9f, loadTime / minimumLoadTime);
			UpdateProgressUI(progress);

			// wait for async loading to complete and min load time to elapse
            if (progress >= 1f) {
				asyncOperation.allowSceneActivation = true;

				SceneManager.UnloadSceneAsync("LoadingScreen");

				PhotonNetwork.IsMessageQueueRunning = true;
            } yield return null;
        }
	}

	void UpdateProgressUI(float progress) {
		loadingSlider.value = progress;
	}
}