using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_Player_HotPotato_Brian : MonoBehaviour
{

    // The other players corresponding to who's currently holding the potato 
    public GameObject leftPlayer; 
    public GameObject oppPlayer; 
    public GameObject rightPlayer; 
    public Transform guide; 

    public int playerID; 
    public bool isHoldingPotato = false; 

    // UI for the player timer 
    public GameObject playerTimer; 
    // Boolean to indicate whether the timer should now start counting down 
    bool timerCountdown = false; 

    // The current time for the player 
    float currTime = 0f; 
    // Player time,  the maximum amount of time a player can hold a potato 
    public float pTime = 10f; 

    public GameObject hotPotato;

    // Update is called once per frame
    void Update()
    {
        // Start the player time when the player holds the potato 
        if (isHoldingPotato) {
            if (!timerCountdown) {
                currTime = pTime; 
                timerCountdown = true; 
            }
        /*
        // ======= Tossing the Potato =======
        // Press A --> Pass to the left player 
        if (Input.GetKeyDown(KeyCode.A)) {
            Scripts_Player_HotPotato_Brian pScript = leftPlayer.GetComponent<Scripts_Player_HotPotato_Brian>();
            pScript.isHoldingPotato = true; 
            hotPotato.transform.position = pScript.guide.transform.position;
            hotPotato.transform.rotation = pScript.guide.transform.rotation;            
            isHoldingPotato = false;
            timerCountdown = false;
        }
        // Press W --> Pass to the opposite player 
        else if (Input.GetKeyDown(KeyCode.W)) {
            Scripts_Player_HotPotato_Brian pScript = oppPlayer.GetComponent<Scripts_Player_HotPotato_Brian>();
            pScript.isHoldingPotato = true; 
            hotPotato.transform.position = pScript.guide.transform.position;
            hotPotato.transform.rotation = pScript.guide.transform.rotation;             
            isHoldingPotato = false;
            timerCountdown = false;
        }
        // Press D --> Pass to the right player 
        else if (Input.GetKeyDown(KeyCode.D)) {
            Scripts_Player_HotPotato_Brian pScript = rightPlayer.GetComponent<Scripts_Player_HotPotato_Brian>();
            pScript.isHoldingPotato = true; 
            hotPotato.transform.position = pScript.guide.transform.position;
            hotPotato.transform.rotation = pScript.guide.transform.rotation;             
            isHoldingPotato = false;
            timerCountdown = false;
        }
        // ==================================
*/
        currTime -= 1 * Time.deltaTime; 
        playerTimer.SetActive(true);
        playerTimer.GetComponent<TMPro.TextMeshProUGUI>().text = currTime.ToString("0");            
        
        }
    }
}
