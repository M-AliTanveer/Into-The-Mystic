using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballProtrudution : MonoBehaviour
{
    public Material materialToModify;
    public float minHeight=0.0131f;
    public float maxHeight=.06f;
    private float currentHeight;
    public float period = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time, period) / period;

        if (t < 0.5f)
        {
            currentHeight = Mathf.Lerp(minHeight, maxHeight, t * 2f);
 
        }
        else
        {
            currentHeight = Mathf.Lerp(maxHeight, minHeight, (t - 0.5f) * 2f);
            
        }
        materialToModify.SetFloat("_Parallax", currentHeight);
    }
}
