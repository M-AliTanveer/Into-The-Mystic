using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendyHallway : MonoBehaviour
{
    public float RotationSpeed=7f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * (RotationSpeed * Time.deltaTime));
    }
}
