using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scripts_QuickPlayPanel_Wyatt : MonoBehaviour {
	void OnEnable() {
		Scripts_NetworkManager_Wyatt.JoinedLobby += QueueForRoom;
		Scripts_NetworkManager_Wyatt.JoinedRoom += EnterPregameScene;
	}

	void OnDisable() {
		Scripts_NetworkManager_Wyatt.JoinedLobby -= QueueForRoom;
		Scripts_NetworkManager_Wyatt.JoinedRoom -= EnterPregameScene;
	}

	void QueueForRoom() {
		PhotonNetwork.JoinRandomOrCreateRoom();
	}

	void EnterPregameScene() {
		SceneManager.LoadScene("PregameRoom");
	}
}
