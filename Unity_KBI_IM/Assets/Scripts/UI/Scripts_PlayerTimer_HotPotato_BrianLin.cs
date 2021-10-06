using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_PlayerTimer_HotPotato_BrianLin : MonoBehaviour
{
    public GameObject pTimer; 

    // Update is called once per frame
    void Update()
    {
        int pTime = 0;
        int.TryParse(GetComponent<TMPro.TextMeshProUGUI>().text, out pTime);    
        
        Animator animator = pTimer.GetComponent<Animator>();

        if (pTime > 3) {
            animator.SetBool("playerHurry", false); 
        }  
        else if (pTime <= 3) {
            animator.SetBool("playerHurry", true);
        }
    }
}
