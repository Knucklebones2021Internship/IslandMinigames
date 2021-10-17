using Photon.Pun;
using Photon.Realtime;
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
		RoomOptions options = new RoomOptions() { MaxPlayers = 4 };
		PhotonNetwork.JoinRandomOrCreateRoom(typedLobby: Scripts_NetworkManager_Wyatt.quickplayLobby, roomOptions: options);
	}

	void EnterPregameScene() {
		SceneManager.LoadScene("PregameRoom");
	}
}
