using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_MiniGolf_Manager_Wyatt : MonoBehaviourPunCallbacks {
	[SerializeField] public GameObject playerPrefab;
	[SerializeField] Transform spawnTransform;

	public static GameObject LocalPlayerInstance;

	public static Dictionary<int, Scripts_MiniGolf_BallController_Zach> localSpectateList = new Dictionary<int, Scripts_MiniGolf_BallController_Zach>();

	bool localSpectateListGenerated = false;
	int roomSize;

	void Start() {
		roomSize = PhotonNetwork.PlayerList.Length;

		if (PhotonNetwork.IsConnected) {
			LocalPlayerInstance = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation, 0);
		} else {
			LocalPlayerInstance = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
		}
	}

	private void Update() {
		if (!localSpectateListGenerated) { // find all BallControllers in the scene and add them to the spectate dictionary
			Scripts_MiniGolf_BallController_Zach[] players = FindObjectsOfType<Scripts_MiniGolf_BallController_Zach>();
			for (int i = 0; i < players.Length; i++) {
				int id = players[i].photonView.ViewID;
				if (!localSpectateList.ContainsKey(id)) {
					localSpectateList.Add(id, players[i]);
				}
			}
			if (players.Length == roomSize) localSpectateListGenerated = true;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			foreach (KeyValuePair<int, Scripts_MiniGolf_BallController_Zach> entry in localSpectateList) {
				print(entry.Key + ": " + entry.Value);
			}
		}
	}

	/// <summary>
	/// Remove a player from the local spectate list based on their PhotonView name. 
	/// Returns the BallController associated with the player.
	/// </summary>
	/// <param name="photonName"></param>
	public static Scripts_MiniGolf_BallController_Zach RemovePlayerFromLocalSpectateList(int photonID) {
		Scripts_MiniGolf_BallController_Zach temp;
		if (localSpectateList.TryGetValue(photonID, out temp)) {
			localSpectateList.Remove(photonID);
        } return temp;
	}

	/// <summary>
	/// Returns the spectate candidate at index % localSpectateList.Count, or null if there are no spectate candidates.
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public static Scripts_MiniGolf_BallController_Zach GetLocalSpectateCandidate(int index) {
		if (localSpectateList.Count == 0) return null; // if there are no candidates, return null

		int trueIndex = index % localSpectateList.Count;

		foreach (KeyValuePair<int, Scripts_MiniGolf_BallController_Zach> entry in localSpectateList) {
			if (trueIndex == 0) {
				return entry.Value;
			} trueIndex--;
		} return null;
	}
}
