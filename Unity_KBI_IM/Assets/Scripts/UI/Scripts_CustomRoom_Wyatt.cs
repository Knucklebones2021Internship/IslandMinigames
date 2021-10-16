using Photon.Pun;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class Scripts_CustomRoom_Wyatt : Scripts_BaseManager_Wyatt {
	[SerializeField] TextMeshProUGUI roomNameLabel;

	protected override void Awake() {
		base.Awake();

        Screen.orientation = ScreenOrientation.Portrait;
	}

	void Start() {
		roomNameLabel.text = Scripts_NetworkManager_Wyatt.GetRoomName();
	}

	public void OnLeaveRoomButton() {
		PhotonNetwork.LeaveRoom();
	}
}
