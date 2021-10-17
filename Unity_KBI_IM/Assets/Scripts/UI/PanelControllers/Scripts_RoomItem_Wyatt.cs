using TMPro;
using UnityEngine;

public class Scripts_RoomItem_Wyatt : MonoBehaviour {
	[SerializeField] TextMeshProUGUI roomNameLabel;
	[SerializeField] TextMeshProUGUI roomOccupancyLabel;
	[SerializeField] GameObject joinButton;
	[SerializeField] Scripts_RoomsPanel_Wyatt roomsPanel;

	public void Reset() {
		gameObject.SetActive(true);
		joinButton.SetActive(false);
		roomOccupancyLabel.gameObject.SetActive(true);
	}

	public void OnSelect() {
		roomsPanel.DeselectAllRoomItems();
		joinButton.SetActive(true);
		roomOccupancyLabel.gameObject.SetActive(false);
	}

	public void SetUpRoomItem(string name, int playerCount, int maxPlayers) {
		roomNameLabel.text = name;
		roomOccupancyLabel.text = "" + playerCount + "/" + maxPlayers;
		joinButton.SetActive(false);
		gameObject.SetActive(true);
	}

	public void OnJoinButton() {
		roomsPanel.JoinRoomFromListButton(roomNameLabel.text);
	}
}
