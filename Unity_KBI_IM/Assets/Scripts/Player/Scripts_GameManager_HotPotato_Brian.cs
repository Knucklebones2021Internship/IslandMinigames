using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

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

    void Update() {
        if (Mouse.current.leftButton.isPressed) {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out raycastHit, 100f)) {
                //Debug.Log( raycastHit.transform.gameObject.name );
                if (raycastHit.transform != null) {
                    PlayerUpdate(raycastHit.transform.gameObject);
                }
            }            
        }        
    }

    void PlayerUpdate(GameObject gameObject) {
        if (gameObject.tag == "Player") {
            GameObject p = gameObject.transform.parent.gameObject; 
            Scripts_Player_HotPotato_Brian pScript = p.GetComponent<Scripts_Player_HotPotato_Brian>();
            Debug.Log(pScript.playerID);
        }
    }   
}
