using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Scripts_GameManager_HotPotato_Brian : MonoBehaviour
{
    // List of players in the game 
    public List<GameObject> playerList; 
    public GameObject hotPotato;

    // Game Timers
    public GameObject globalTimer;
    public GameObject playerTimer;
    // Game Over UI  
    public GameObject gameOverPanel;
    // Player Number text for the player that lost  
    public GameObject playerNum;  

    // <summary> 
    // Randomly determine the first player to hold the potato 
    // </summary>
    void Start()
    {
        int pid = Random.Range(1, playerList.Count + 1);
        Scripts_Player_HotPotato_Brian pScript = playerList[pid-1].GetComponent<Scripts_Player_HotPotato_Brian>();
        pScript.isHoldingPotato = true; 
        hotPotato.transform.position = pScript.guide.transform.position;
        hotPotato.transform.rotation = pScript.guide.transform.rotation;
    }

    void Update() {
        int endTime = 0;
        int.TryParse(globalTimer.GetComponent<TMPro.TextMeshProUGUI>().text, out endTime);
        if (endTime <= 75) {
            Debug.Log(endTime); 
        } 
    }
  
}
