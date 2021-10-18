using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventTest : MonoBehaviour
{
    public AK.Wwise.Event testEvent;
    public AK.Wwise.Event sfxTestEvent;

    // Start is called before the first frame update
    void Start()
    {
        testEvent.Post(gameObject);
        sfxTestEvent.Post(gameObject);
        AkSoundEngine.PostEvent("Ball_Jump", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space was pressed");
            //AkSoundEngine.PostEvent("Ball_Jump", this.gameObject);
        }
    }
}
