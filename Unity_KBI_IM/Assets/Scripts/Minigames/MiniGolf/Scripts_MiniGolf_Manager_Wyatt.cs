using Photon.Pun;
using UnityEngine;

public class Scripts_MiniGolf_Manager_Wyatt : MonoBehaviourPunCallbacks {
	[SerializeField] public GameObject playerPrefab;
	[SerializeField] Transform spawnTransform;

	public static GameObject LocalPlayerInstance;

	void Start() {
		print("instatiation1");
		if (PhotonNetwork.IsConnected) {
			GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation, 0);
			print(go.name);
		} else {
			Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
		}
		print("instatiation2");
	}
}
