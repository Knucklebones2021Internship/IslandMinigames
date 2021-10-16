using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public enum MultiplayerStatus { MATCHMAKING, CUSTOM_LOBBY }

// TODO: move to global manager

[DefaultExecutionOrder(-60)]
public class Scripts_NetworkManager_Wyatt : MonoBehaviourPunCallbacks {
	public static Scripts_NetworkManager_Wyatt Instance { get; private set; }

	public static MultiplayerStatus multiplayerStatus;

	public static bool connectedToMaster = false;

	public static TypedLobby quickplayLobby = new TypedLobby("quickplay", LobbyType.Default);
	public static TypedLobby matchmadeLobby = new TypedLobby("matchmade", LobbyType.Default);
	public static TypedLobby customLobby = new TypedLobby("custom", LobbyType.Default);

	static string currentRoomName;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			PhotonNetwork.ConnectUsingSettings();
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	public override void OnConnectedToMaster() {
		connectedToMaster = true;
	}

	public override void OnLeftRoom() {
		SceneManager.LoadScene("Titlescreen");
	}

	public static void SetRoomName(string name) => currentRoomName = name;
	public static string GetRoomName() => currentRoomName;
}
