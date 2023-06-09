using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismRotation : MonoBehaviour
{
    float speed= 4f, Angle=90.0f;
    bool is_rotate = false;
    public Transform prism;
    // Start is called before the first frame update
    void Start()
    {
        Angle += prism.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_rotate)
        {
            prism.rotation = Quaternion.Slerp(prism.rotation, Quaternion.Euler(prism.rotation.x, Angle, prism.rotation.z), Time.deltaTime * speed);
            if (prism.rotation == Quaternion.Euler(prism.rotation.x, Angle, prism.rotation.z))
            {
                Angle += 90.0f;
                is_rotate = false;
            }
        }

    }
    public void RotatePrism()
    {
        is_rotate = true;
    }
}
