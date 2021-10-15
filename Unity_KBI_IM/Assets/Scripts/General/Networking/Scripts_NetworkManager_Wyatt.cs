using Photon.Pun;

public enum MultiplayerStatus { MATCHMAKING, CUSTOM_LOBBY }

public class Scripts_NetworkManager_Wyatt : MonoBehaviourPunCallbacks {
	public static Scripts_NetworkManager_Wyatt Instance { get; private set; }

	public static MultiplayerStatus multiplayerStatus;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public static void ConnectToServer() {
		multiplayerStatus = MultiplayerStatus.CUSTOM_LOBBY;

		PhotonNetwork.ConnectUsingSettings();
	}
}
