using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingSpinning : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0), .1f);
    }
}
