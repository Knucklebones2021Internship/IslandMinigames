using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_JumpMonster_MiniGolf_BrianLin : MonoBehaviour
{
    // Way points where the jump monster moves during its "animation" 
    public GameObject[] wayPoints; 
    // Index of the list of way points 
    int index = 0; 
    // Movement speed of the jump monster 
    public float mvmSpeed; 
    // Radius of the way points 
    float wpRadius = 1;
    // Boolean for the monster to continue moving 
    bool moving = true; 

    // <summary> 
    // Move jump monster to its specified way points 
    // </summary>    
    void Update()
    {
        if (Vector3.Distance(wayPoints[index].transform.position, transform.position) < wpRadius) {
            index++; 
            if (index >= wayPoints.Length) { 
                moving = false; 
                index = 0; 
            }
        }

        if (moving) {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[index].transform.position, Time.deltaTime * mvmSpeed);
        }
        
    }

}
