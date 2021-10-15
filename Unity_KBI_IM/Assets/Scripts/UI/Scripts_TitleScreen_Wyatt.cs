using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

[DefaultExecutionOrder(-50)]
public class Scripts_TitleScreen_Wyatt : Scripts_BaseManager_Wyatt {
	PlayerInput tsInput;

	[SerializeField] GameObject TitlePanel;
	[SerializeField] GameObject QuickPlayPanel;
	[SerializeField] GameObject QueuePanel;
	[SerializeField] GameObject LobbiesPanel;
	[SerializeField] GameObject SettingsPanel;

	protected override void Awake() {
		base.Awake();

        Screen.orientation = ScreenOrientation.Portrait;
		Input.backButtonLeavesApp = true;

		tsInput = GetComponent<PlayerInput>();

		TitlePanel.SetActive(true);
		QuickPlayPanel.SetActive(false);
		QueuePanel.SetActive(false);
		LobbiesPanel.SetActive(false);
		SettingsPanel.SetActive(false);
	}

	void OnEnable()  { tsInput.actions["Back"].started += OnBack; }
	void OnDisable() { tsInput.actions["Back"].started -= OnBack; }

	#region BUTTON FUNCS
	public void EnterQuickPlay() { 
		TitlePanel.SetActive(false);
		QuickPlayPanel.SetActive(true);
	}

	public void EnterQueue() { 
		TitlePanel.SetActive(false);
		QueuePanel.SetActive(true);
	}

	public void EnterLobbies() { 
		TitlePanel.SetActive(false);
		LobbiesPanel.SetActive(true);

		Scripts_NetworkManager_Wyatt.ConnectToServer();
	}

	public void EnterSettings() { 
		TitlePanel.SetActive(false);
		SettingsPanel.SetActive(true);
	}

	public void Quit() { Application.Quit(); }
	#endregion

	public void OnBack(InputAction.CallbackContext ctx) {
		if (!TitlePanel.activeInHierarchy) {
			TitlePanel.SetActive(true);
			QuickPlayPanel.SetActive(false);
			QueuePanel.SetActive(false);
			LobbiesPanel.SetActive(false);
			SettingsPanel.SetActive(false);
		} else Quit();
	}
}
