using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_GameManager_HotPotato_Brian : MonoBehaviour
{

    public List<GameObject> playerList; 
    public GameObject hotPotato;

    // Randomly determine the first player to hold the potato 
    void Start()
    {
        int pid = Random.Range(1, playerList.Count + 1);
        Scripts_Player_HotPotato_Brian pScript = playerList[pid-1].GetComponent<Scripts_Player_HotPotato_Brian>();
        pScript.isHoldingPotato = true; 
        hotPotato.transform.position = pScript.guide.transform.position;
        hotPotato.transform.rotation = pScript.guide.transform.rotation;
    }

   
}
