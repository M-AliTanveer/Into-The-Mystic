using IntoTheMystic.ReUsableItems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerbuttonFix : MonoBehaviour
{
    bool playerOnTop = false;
    GameObject player;
    GenericButton script;

    private void Start()
    {
        script = transform.parent.parent.gameObject.GetComponent<GenericButton>();
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hit, .4f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerOnTop = true;
                Debug.Log("hi");
                player = hit.collider.gameObject;
                script.manualPress = true;
                script.buttonTopRigid.AddForce(-script.buttonTop.transform.up * 10 * script.buttonTopRigid.mass * Time.deltaTime * 1000f);
            }

            else if (hit.collider.gameObject.CompareTag("Pickable"))
            {
                playerOnTop = true;
                Debug.Log("hi");
                player = hit.collider.gameObject;
                script.manualPress = true;
                script.buttonTopRigid.AddForce(-script.buttonTop.transform.up *hit.transform.GetComponent<Rigidbody>().mass* Time.deltaTime * .1f);
            }

        }
        else
        {
            if (playerOnTop)
            {
                script.manualPress = false;
                playerOnTop = false;
                script.Released();
            }
        }
    }
}
