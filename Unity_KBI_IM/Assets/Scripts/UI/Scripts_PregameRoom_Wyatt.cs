using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class Scripts_PregameRoom_Wyatt : Scripts_BaseManager_Wyatt {
	[SerializeField] TextMeshProUGUI roomNameLabel;
	[SerializeField] TextMeshProUGUI lobbyTypeLabel;
	[SerializeField] TextMeshProUGUI occupancyLabel;
	[SerializeField] Button playButton;

	PlayerInput input;

	protected override void Awake() {
		base.Awake();

		input = GetComponent<PlayerInput>();
        Screen.orientation = ScreenOrientation.Portrait;
	}

	void Start() {
		roomNameLabel.text = Scripts_NetworkManager_Wyatt.GetRoomName();
		lobbyTypeLabel.text = Scripts_NetworkManager_Wyatt.multiplayerType.ToString();
		UpdateOccupancyText();
		if (!PhotonNetwork.IsMasterClient) playButton.gameObject.SetActive(false);
	}

	void OnEnable() { 
		input.actions["Back"].started += OnBack;
		Scripts_NetworkManager_Wyatt.LeftRoom += OnLeaveRoom;
		Scripts_NetworkManager_Wyatt.PlayerEnteredRoom += OnPlayerEntered;
		Scripts_NetworkManager_Wyatt.PlayerLeftRoom += OnPlayerLeft;
	}

	void OnDisable() { 
		input.actions["Back"].started -= OnBack; 
		Scripts_NetworkManager_Wyatt.LeftRoom -= OnLeaveRoom;
		Scripts_NetworkManager_Wyatt.PlayerEnteredRoom -= OnPlayerEntered;
		Scripts_NetworkManager_Wyatt.PlayerLeftRoom -= OnPlayerLeft;
	}

	#region EVENT CALLBACKS
	void OnBack(InputAction.CallbackContext ctx) => OnLeaveRoomButton();

	void OnLeaveRoom() {
		SceneManager.LoadScene("Titlescreen");
	}

	void OnPlayerEntered(Player player) { 
		UpdateOccupancyText();
	}

	void OnPlayerLeft(Player player) {
		UpdateOccupancyText();
		if (PhotonNetwork.IsMasterClient) playButton.gameObject.SetActive(true);
	}
	#endregion

	#region BUTTON FUNCS
	public void OnPlayButton() {
		playButton.interactable = false;
		PhotonNetwork.LoadLevel("LoadingScreen");
	}

	public void OnLeaveRoomButton() {
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.LeaveRoom();
	}
	#endregion

	void UpdateOccupancyText() => occupancyLabel.text = "" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
}
