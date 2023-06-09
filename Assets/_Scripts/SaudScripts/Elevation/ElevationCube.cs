using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationCube : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.5f;
    public float speed = 10;
    public Vector3 o_pos;
    Vector3 velocity;

    bool rise = false, fall = false;
    public bool forward = true;
    // Start is called before the first frame update
    void Start()
    {
        o_pos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(forward)
        {
            if (rise)
            {
                //fall = false;
                transform.position = Vector3.SmoothDamp(transform.position, o_pos, ref velocity, smoothTime, speed);

                //if (transform.position.y == o_pos.y)
                //{
                //    rise = false;

                //}
            }
            if (fall)
            {
                //rise = false;
                transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime, speed);

                //if (transform.position.y == target.position.y)
                //{
                //    fall = false;

                //}

                //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, original_pos.y - transform.localScale.y, transform.position.z), 5f * Time.deltaTime);
                //if (transform.position == new Vector3(transform.position.x, original_pos.y - transform.localScale.y, transform.position.z))
                //{
                //    fall = false;
                //}
            }

        }

        else
        {
            if (rise)
            {
                //fall = false;
                transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime, speed);
               

 
            }
            if (fall)
            {

                transform.position = Vector3.SmoothDamp(transform.position, o_pos, ref velocity, smoothTime, speed);
            }

        }


    }

    public void Rise()
    {
        rise = true;
        fall = false;
    }
    public void Fall()
    {
        fall = true;
        rise = false;
    }
}
