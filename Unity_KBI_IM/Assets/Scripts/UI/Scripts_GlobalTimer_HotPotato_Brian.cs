using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Scripts_GlobalTimer_HotPotato_Brian : MonoBehaviour {

    float currTime = 0f; 
    public float startingTime = 90f; 

    void Start() 
    {
        currTime = startingTime; 
    }

    // Update is called once per frame
    void Update()
    {
        currTime -= 1 * Time.deltaTime; 
        GetComponent<TMPro.TextMeshProUGUI>().text = currTime.ToString("0"); 
        if (currTime <= 80) {
            GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        }        
        if (currTime <= 75) {
            currTime = 75;
        }
    }
}
