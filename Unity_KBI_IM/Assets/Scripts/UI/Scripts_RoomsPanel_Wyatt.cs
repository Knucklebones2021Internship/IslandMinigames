using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class Scripts_RoomsPanel_Wyatt : MonoBehaviourPunCallbacks {
	[Header("Panels")]
	[SerializeField] GameObject joinRoomPanel;
	[SerializeField] GameObject createRoomPanel;

	[Header("JoinPanel")]
	[SerializeField] TMP_InputField joinRoomNameField;
	[SerializeField] TextMeshProUGUI joinErrorLabel;

	[Header("CreatePanel")]
	[SerializeField] TMP_InputField createRoomNameField;
	[SerializeField] TMP_InputField createRoomCapacityField;
	[SerializeField] Toggle createRoomPrivateToggle;
	[SerializeField] TextMeshProUGUI createErrorLabel;

	[Header("RoomDropDown")]
	[SerializeField] Transform roomItemContainer;
	[SerializeField] GameObject roomItemTemplate;

	List<Scripts_RoomItem_Wyatt> roomItems = new List<Scripts_RoomItem_Wyatt>();
	List<RoomInfo> roomInfos = new List<RoomInfo>();

	float timeBetweenRoomListUpdates = 1.5f;
	float nextRoomListUpdateTime = 0f;

	void Awake() {
		joinRoomPanel.SetActive(false);
		createRoomPanel.SetActive(false);
	}

	#region JoinPanel
	public void OnJoinRoomButton() {
		joinRoomPanel.SetActive(true);
		joinRoomNameField.text = "";
		joinErrorLabel.gameObject.SetActive(false);
	}

	public void JoinRoomFromListButton(string roomName) {
		JoinRoom(roomName);
	}

	public void OnJoinSubmitButton() {
		string name = joinRoomNameField.text;

		JoinRoom(name);
	}

	void JoinRoom(string name) {
		Scripts_NetworkManager_Wyatt.SetRoomName(name);
		PhotonNetwork.JoinRoom(name);
	}

	public void OnJoinCancelButton() {
		joinRoomPanel.SetActive(false);
	}
	#endregion

	#region CreatePanel
	public void OnCreateRoomButton() {
		createRoomPanel.SetActive(true);
		createRoomNameField.text = "";
		createRoomCapacityField.text = "4";
		createRoomPrivateToggle.isOn = true;
		createErrorLabel.gameObject.SetActive(false);
	}

	public void OnCreateSubmitButton() {
		string name = createRoomNameField.text;
		int capacity = Int32.Parse(createRoomCapacityField.text);
		bool privateRoom = createRoomPrivateToggle.isOn;

		#region ERROR CHECKING
		if (name.Length == 0) {
			createErrorLabel.text = "Error: Invalid room name.";
			createErrorLabel.gameObject.SetActive(true);
			return;
		}

		if (capacity <= 0) {
			createErrorLabel.text = "Error: Invalid room capacity";
			createErrorLabel.gameObject.SetActive(true);
			return;
		}

		for (int i=0; i<roomInfos.Count; i++) {
			if (roomInfos[i].Name == name) {
				createErrorLabel.text = "Error: Room already exists.";
				createErrorLabel.gameObject.SetActive(true);
				return;
			}
		}
		#endregion

		// create the scene
		Scripts_NetworkManager_Wyatt.SetRoomName(name);
		RoomOptions roomOptions = new RoomOptions() { MaxPlayers = (byte)capacity, IsVisible = !privateRoom, IsOpen = true };
		PhotonNetwork.CreateRoom(name, roomOptions, Scripts_NetworkManager_Wyatt.customLobby);

		createRoomPanel.SetActive(false);
	}

	public void OnCreateCancelButton() {
		createRoomPanel.SetActive(false);
	}
	#endregion

	// whenever the room list is updated, refresh the list of rooms
	public override void OnRoomListUpdate(List<RoomInfo> roomList) {
		if (Time.time <= nextRoomListUpdateTime) return;
		nextRoomListUpdateTime = Time.time + timeBetweenRoomListUpdates;

		// clear old room items
		foreach (Scripts_RoomItem_Wyatt item in roomItems) {
			Destroy(item.gameObject);
		} roomItems.Clear(); roomInfos.Clear();

		// create all the rooms from scratch
		foreach (RoomInfo info in roomList) {
			if (info.IsVisible) { // only display if the room is public
				Scripts_RoomItem_Wyatt newItem = Instantiate(roomItemTemplate, roomItemContainer).GetComponent<Scripts_RoomItem_Wyatt>();
				newItem.SetUpRoomItem(info.Name, info.PlayerCount, info.MaxPlayers);
				roomItems.Add(newItem);
			} roomInfos.Add(info);
			print(info.Name);
		}
	}

	public void DeselectAllRoomItems() {
		for (int i=0; i<roomItems.Count; i++) roomItems[i].Reset();
	}

	// once we've joined the room, enter the PregameRoom scene to wait for your fellow players
	public override void OnJoinedRoom() {
		SceneManager.LoadScene("PregameRoom");
	}

	public override void OnJoinRoomFailed(short returnCode, string message) {
		joinErrorLabel.text = "Error: " + message;
		joinErrorLabel.gameObject.SetActive(true);
	}
}
