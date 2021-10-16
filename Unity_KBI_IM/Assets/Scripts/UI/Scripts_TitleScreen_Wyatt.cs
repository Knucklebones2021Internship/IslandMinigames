using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-50)]
public class Scripts_TitleScreen_Wyatt : Scripts_BaseManager_Wyatt {
	PlayerInput tsInput;

	[Header("Panels")]
	[SerializeField] GameObject ConnectingToServerPanel;
	[SerializeField] GameObject TitlePanel;
	[SerializeField] GameObject QuickPlayPanel;
	[SerializeField] GameObject QueuePanel;
	[SerializeField] GameObject RoomsPanel;
	[SerializeField] GameObject SettingsPanel;

	[Header("SubPanels")]
	[SerializeField] GameObject JoinRoomPanel;
	[SerializeField] GameObject CreateRoomPanel;

	protected override void Awake() {
		base.Awake();

        Screen.orientation = ScreenOrientation.Portrait;
		Input.backButtonLeavesApp = true;

		tsInput = GetComponent<PlayerInput>();

		ConnectingToServerPanel.SetActive(true);
		TitlePanel.SetActive(false);
		QuickPlayPanel.SetActive(false);
		QueuePanel.SetActive(false);
		RoomsPanel.SetActive(false);
		SettingsPanel.SetActive(false);
	}

	void OnEnable()  {
		Scripts_NetworkManager_Wyatt.ConnectedToMaster += DisplayTitleScreen;
		tsInput.actions["Back"].started += OnBack; 
	}

	void OnDisable() { 
		Scripts_NetworkManager_Wyatt.ConnectedToMaster -= DisplayTitleScreen;
		tsInput.actions["Back"].started -= OnBack; 
	}

	#region BUTTON FUNCS
	public void EnterQuickPlay() { 
		TitlePanel.SetActive(false);
		QuickPlayPanel.SetActive(true);

		Scripts_NetworkManager_Wyatt.multiplayerType = MultiplayerType.QUICKPLAY;
		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.quickplayLobby);
	}

	public void EnterQueue() { 
		TitlePanel.SetActive(false);
		QueuePanel.SetActive(true);

		Scripts_NetworkManager_Wyatt.multiplayerType = MultiplayerType.MATCHMAKING;
		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.matchmadeLobby);
	}

	public void EnterRooms() {
		TitlePanel.SetActive(false);
		RoomsPanel.SetActive(true);

		Scripts_NetworkManager_Wyatt.multiplayerType = MultiplayerType.CUSTOM_LOBBY;
		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.customLobby);
	}

	public void EnterSettings() { 
		TitlePanel.SetActive(false);
		SettingsPanel.SetActive(true);
	}

	public void Quit() { Application.Quit(); }
	#endregion

	void DisplayTitleScreen() { 
		ConnectingToServerPanel.SetActive(false);
		TitlePanel.SetActive(true);
	}

	public void OnBack(InputAction.CallbackContext ctx) {
		if (JoinRoomPanel.activeInHierarchy || CreateRoomPanel.activeInHierarchy) {
			JoinRoomPanel.SetActive(false);
			CreateRoomPanel.SetActive(false);

			RoomsPanel.GetComponent<Scripts_RoomsPanel_Wyatt>().SetUIInteractable(true);
		} else if (!TitlePanel.activeInHierarchy && !ConnectingToServerPanel.activeInHierarchy) {
			TitlePanel.SetActive(true);
			QuickPlayPanel.SetActive(false);
			QueuePanel.SetActive(false);
			RoomsPanel.SetActive(false);
			SettingsPanel.SetActive(false);
		} else Quit();
	}
}
