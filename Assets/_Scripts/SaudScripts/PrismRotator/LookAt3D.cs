using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt3D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.x, GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.y, GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.z));

    }
}
