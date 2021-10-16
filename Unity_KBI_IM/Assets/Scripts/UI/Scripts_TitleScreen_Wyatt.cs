using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-50)]
public class Scripts_TitleScreen_Wyatt : Scripts_BaseManager_Wyatt {
	PlayerInput tsInput;

	[SerializeField] GameObject TitlePanel;
	[SerializeField] GameObject QuickPlayPanel;
	[SerializeField] GameObject QueuePanel;
	[SerializeField] GameObject RoomsPanel;
	[SerializeField] GameObject SettingsPanel;

	protected override void Awake() {
		base.Awake();

        Screen.orientation = ScreenOrientation.Portrait;
		Input.backButtonLeavesApp = true;

		tsInput = GetComponent<PlayerInput>();

		TitlePanel.SetActive(true);
		QuickPlayPanel.SetActive(false);
		QueuePanel.SetActive(false);
		RoomsPanel.SetActive(false);
		SettingsPanel.SetActive(false);
	}

	void OnEnable()  { tsInput.actions["Back"].started += OnBack; }
	void OnDisable() { tsInput.actions["Back"].started -= OnBack; }

	#region BUTTON FUNCS
	public void EnterQuickPlay() { 
		if (!Scripts_NetworkManager_Wyatt.connectedToMaster) return;

		TitlePanel.SetActive(false);
		QuickPlayPanel.SetActive(true);

		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.quickplayLobby);
	}

	public void EnterQueue() { 
		if (!Scripts_NetworkManager_Wyatt.connectedToMaster) return;

		TitlePanel.SetActive(false);
		QueuePanel.SetActive(true);

		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.matchmadeLobby);
	}

	public void EnterRooms() {
		if (!Scripts_NetworkManager_Wyatt.connectedToMaster) return;

		TitlePanel.SetActive(false);
		RoomsPanel.SetActive(true);

		PhotonNetwork.JoinLobby(Scripts_NetworkManager_Wyatt.customLobby);
	}

	public void EnterSettings() { 
		TitlePanel.SetActive(false);
		SettingsPanel.SetActive(true);
	}

	public void Quit() { Application.Quit(); }
	#endregion

	// TODO: make functional for other panels as well!
	public void OnBack(InputAction.CallbackContext ctx) {
		if (!TitlePanel.activeInHierarchy) {
			TitlePanel.SetActive(true);
			QuickPlayPanel.SetActive(false);
			QueuePanel.SetActive(false);
			RoomsPanel.SetActive(false);
			SettingsPanel.SetActive(false);
		} else Quit();
	}
}
