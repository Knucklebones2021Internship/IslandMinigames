using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Scripts_GameManager_HotPotato_Brian : MonoBehaviour
{
    // List of players in the game 
    public List<GameObject> playerList; 
    public GameObject hotPotato;

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

    // <summary> 
    // Detect whether the user is clicking on a player  
    // </summary>
    void Update() {
        if (Mouse.current.leftButton.isPressed) {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 100f)) {
                if (raycastHit.transform != null) {
                    PlayerUpdate(raycastHit.transform.gameObject);
                }
            }            
        }        
    }

    // <summary> 
    // Pass the potato to the clicked player 
    // </summary>
    // <param name="gameObject"></param>
    void PlayerUpdate(GameObject gameObject) {
        if (gameObject.tag == "Player") {
            GameObject p = gameObject.transform.parent.gameObject; 
            Scripts_Player_HotPotato_Brian pScript = p.GetComponent<Scripts_Player_HotPotato_Brian>();
            Debug.Log(pScript.playerID);
        }
    }   
}
