using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class Scripts_PregameRoom_Wyatt : Scripts_BaseManager_Wyatt {
	[SerializeField] TextMeshProUGUI roomNameLabel;
	[SerializeField] TextMeshProUGUI lobbyTypeLabel;
	[SerializeField] TextMeshProUGUI occupancyLabel;

	PlayerInput input;

	protected override void Awake() {
		base.Awake();

		input = GetComponent<PlayerInput>();
        Screen.orientation = ScreenOrientation.Portrait;
	}

	void Start() {
		roomNameLabel.text = Scripts_NetworkManager_Wyatt.GetRoomName();
		lobbyTypeLabel.text = Scripts_NetworkManager_Wyatt.multiplayerType.ToString();
		occupancyLabel.text = "" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
		// TODO: update this dynamically
	}

	void OnEnable() { 
		input.actions["Back"].started += OnBack;
		Scripts_NetworkManager_Wyatt.LeftRoom += OnLeaveRoom;
	}

	void OnDisable() { 
		input.actions["Back"].started -= OnBack; 
		Scripts_NetworkManager_Wyatt.LeftRoom -= OnLeaveRoom;
	}

	void OnBack(InputAction.CallbackContext ctx) => OnLeaveRoomButton();

	public void OnLeaveRoomButton() {
		PhotonNetwork.LeaveRoom();
	}

	void OnLeaveRoom() {
		SceneManager.LoadScene("Titlescreen");
	}
}
