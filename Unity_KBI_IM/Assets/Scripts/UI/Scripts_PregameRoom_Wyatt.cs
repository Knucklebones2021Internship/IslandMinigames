using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-50)]
public class Scripts_PregameRoom_Wyatt : Scripts_BaseManager_Wyatt {
	[SerializeField] TextMeshProUGUI roomNameLabel;

	PlayerInput input;

	protected override void Awake() {
		base.Awake();

		input = GetComponent<PlayerInput>();
        Screen.orientation = ScreenOrientation.Portrait;
	}

	void Start() {
		roomNameLabel.text = Scripts_NetworkManager_Wyatt.GetRoomName();
	}

	void OnEnable() { input.actions["Back"].started += OnBack; }
	void OnDisable() { input.actions["Back"].started -= OnBack; }

	void OnBack(InputAction.CallbackContext ctx) => OnLeaveRoomButton();

	public void OnLeaveRoomButton() {
		PhotonNetwork.LeaveRoom();
	}
}
