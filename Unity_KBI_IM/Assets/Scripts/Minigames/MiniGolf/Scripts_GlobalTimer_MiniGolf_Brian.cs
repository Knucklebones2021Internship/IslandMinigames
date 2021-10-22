using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Scripts_GlobalTimer_MiniGolf_Brian : MonoBehaviour
{
    float currTime = 0f; 
    public float startingTime; 

    void Start() 
    {
        currTime = startingTime; 
    }

    // <summary> 
    // Decrement the global timer for the game by changing the text. The color
    // of the text becomes red when it reaches a specific number 
    // </summary>
    void Update()
    {
        currTime -= 1 * Time.deltaTime; 
        GetComponent<TMPro.TextMeshProUGUI>().text = currTime.ToString("0");    
        
        // Timer UI turns red at 10 seconds 
        if (currTime <= 10) {
            GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        }   

        if (currTime <= 0) {
            currTime = 0;
        }
    }
}
