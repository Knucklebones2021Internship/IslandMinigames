using Photon.Pun;
using UnityEngine;

public class Scripts_MiniGolf_Manager_Wyatt : MonoBehaviourPunCallbacks {
	[SerializeField] public GameObject playerPrefab;
	[SerializeField] Transform spawnTransform;

	public static GameObject LocalPlayerInstance;

	void Start() {
		if (PhotonNetwork.IsConnected) {
			PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation, 0);
		} else {
			Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
		}
	}
}
