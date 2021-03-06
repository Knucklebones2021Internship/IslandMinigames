using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Scripts_Player_HotPotato_Brian : MonoBehaviour
{

    // The other players corresponding to who's currently holding the potato 
    public GameObject leftPlayer; 
    public GameObject oppPlayer; 
    public GameObject rightPlayer; 
    public Transform guide; 

    public int playerID; 
    public bool isHoldingPotato = false;
    public bool justReceivedPotato = false;
    public bool gameOver = false; 

    // UI for the player timer 
    public GameObject playerTimer; 
    // Boolean to indicate whether the timer should now start counting down 
    public bool timerCountdown = false; 

    // The current time for the player 
    float currTime = 0f; 
    // Player time,  the maximum amount of time a player can hold a potato 
    public float pTime = 10f; 

    // The hot potato being passed around 
    public GameObject hotPotato;

    // +++ Player Score Timer +++
    public float scoreTime = 0f; 

    public AK.Wwise.Event receivePotatoEvent;
    public AK.Wwise.Event tossPotatoCorrectEvent;

    // <summary> 
    // Start the player timer when they are holding the hot potato 
    // Determine whether the user has decided to pass the potato to another player 
    // </summary>
    void Update()
    {
        if (!gameOver) {
            StartCoroutine(Play()); 
        }
    }

    IEnumerator Play() {
        // Start the player time when the player holds the potato 
        if (isHoldingPotato) {
            if (justReceivedPotato)
            {
                receivePotatoEvent.Post(gameObject);
                justReceivedPotato = false;
            }
            yield return new WaitForSeconds(0.5f); 

            if (!timerCountdown) {
                currTime = pTime; 
                timerCountdown = true;   
            }

            // +++ Score Time Calculation +++
            Scripts_Potato_HotPotato_BrianLin potScript = hotPotato.GetComponent<Scripts_Potato_HotPotato_BrianLin>();
            
            if (potScript.potType) { scoreTime += Time.deltaTime; } 
            else if (!potScript.potType) { scoreTime -= Time.deltaTime; }
            // ++++++++++++++++++++++++++++++

            //Debug.Log(Input.acceleration);
            
            if (Input.acceleration.x < -0.5f) {
                //Debug.Log("Left"); 
                StartCoroutine(PotatoPass(leftPlayer)); 
            }
            
            else if (Input.acceleration.x > 0.6f ) {
                //Debug.Log("Right"); 
                StartCoroutine(PotatoPass(rightPlayer)); 
            }
              
            else if (Input.acceleration.z < -0.9f) {
                //Debug.Log("Opp"); 
                StartCoroutine(PotatoPass(oppPlayer)); 
            } /**/
                    

            // Detect whether the user has touched a player 
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

            // +++ Mobile Touch Input +++
            if (Input.touchCount > 0) 
            {
                for (int i = 0; i < Input.touchCount; ++i) {
                    Touch touch = Input.GetTouch(i);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, 100f)) {
                        if (raycastHit.transform != null) {
                            PlayerUpdate(raycastHit.transform.gameObject);
                        }
                    } 
                }
               
            }

            currTime -= 1 * Time.deltaTime; 
            playerTimer.SetActive(true);
            playerTimer.GetComponent<TMPro.TextMeshProUGUI>().text = currTime.ToString("0");               
            if (currTime <= 0) {
                currTime = 0;
                playerTimer.SetActive(false);
            }          
        
        }        
    }
    // <summary> 
    // Pass the potato to the clicked player if they are not holding the potato 
    // </summary>
    // <param name="gameObject"></param>
    void PlayerUpdate(GameObject gameObject) {
        if (gameObject.tag == "Player") {
            GameObject p = gameObject.transform.parent.gameObject; 
            Scripts_Player_HotPotato_Brian pScript = p.GetComponent<Scripts_Player_HotPotato_Brian>();

            // If the user presses another player that doesn't have the hot potato 
            if (pScript.playerID != playerID) {
                pScript.isHoldingPotato = true;
                pScript.justReceivedPotato = true;
                //tossPotatoCorrectEvent.Post(gameObject);
                //Debug.Log("How often is this getting called?");
                hotPotato.transform.position = pScript.guide.transform.position;
                hotPotato.transform.rotation = pScript.guide.transform.rotation; 
                isHoldingPotato = false;
                timerCountdown = false;
            }
        }
    }

    // <summary> 
    // Pass the potato to the player corresponding to the direction of the phone flick. 
    // </summary>
    // <param name="player"></param>
    IEnumerator PotatoPass(GameObject player) {
        Scripts_Player_HotPotato_Brian pScript = player.GetComponent<Scripts_Player_HotPotato_Brian>();

        pScript.isHoldingPotato = true;
        pScript.justReceivedPotato = true;
        hotPotato.transform.position = pScript.guide.transform.position;
        hotPotato.transform.rotation = pScript.guide.transform.rotation;
        tossPotatoCorrectEvent.Post(gameObject);
        isHoldingPotato = false;
        timerCountdown = false;
        yield return new WaitForSeconds(0.1f);      
    }      
}
