using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavy : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> Leaves = new List<GameObject>();
    List<float> Vals = new List<float>();
    public float rate = .1f;
    void Start()
    {
        int childNum = transform.childCount;
        for(int i = 0; i < childNum; i++)
        {
            Leaves.Add(transform.GetChild(i).gameObject);
            Vals.Add(0.1f);
        }
        InvokeRepeating("ChangeVal", 0, 2);
    }
    void ChangeVal()
    {
       for (int i = 0; i < Vals.Count; i++)
        {
            if (Vals[i] < 0)
            {
                Vals[i] = Random.Range(0f, .1f);
            } else
            {
                Vals[i] = Random.Range(-.1f, 0f);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < Leaves.Count; i++)
        {
            Transform GO = Leaves[i].transform;
            float Value = Vals[i];
            //GO.localScale = new Vector3(GO.localScale.x, GO.localScale.y, Mathf.Lerp(GO.localScale.z,Value,rate));
            GO.Rotate(Mathf.Lerp(0, Value * 3, rate), Mathf.Lerp(0, Value * 3, rate), Mathf.Lerp(0, Value * 3, rate));
        }
    }
}
