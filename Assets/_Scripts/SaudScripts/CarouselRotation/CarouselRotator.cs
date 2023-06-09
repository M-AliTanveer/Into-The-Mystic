using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselRotator : MonoBehaviour
{
    
    public float rotationsPerMinute = 5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0, Space.World);
    }
}
