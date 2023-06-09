using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsObject : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.5f;
    public float speed = 10;
    Vector3 velocity;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime, speed);
    }
}
