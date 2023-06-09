using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollision : MonoBehaviour
{
    private bool hasEntered = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "FPSPlayer" && !hasEntered)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            hasEntered = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Player" && hasEntered)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            hasEntered = false;
            Debug.Log("yo");
        }
    }

}
