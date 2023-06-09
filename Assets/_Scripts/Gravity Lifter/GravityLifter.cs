using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLifter : MonoBehaviour
{

    [Tooltip("Maximum distance for picking up stuff")]
    [SerializeField] private float MaxGrabDistance = 10.0f;

    [Tooltip("Attach MainCamera here")]
    [SerializeField] private Camera MainCamera;

    [Tooltip("Throw Force (0 to disable)")]
    [SerializeField] private float throwForce = 0;

    [Tooltip("Save the previous parent")]
    [SerializeField] private Transform walltransform;

    public GameObject grabbedItem;

    private void FixedUpdate()
    {
        
    }

    public bool LiftObject()
    {
        bool retVal = false;
        if(grabbedItem != null)
        {
            Rigidbody rb = grabbedItem.GetComponent<Rigidbody>();
            grabbedItem.transform.parent = null;
            //grabbedItem.transform.GetComponent<Rigidbody>().useGravity = false;
            //BoxCollider[] itemColliders = grabbedItem.GetComponentsInChildren<BoxCollider>();
            //foreach(BoxCollider itemCollider in itemColliders)
            //{
            //    itemCollider.isTrigger = false;
            //    itemCollider.enabled = true;
            //}

            //rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(MainCamera.transform.forward * throwForce, ForceMode.VelocityChange);
            //grabbedItem.transform.parent = walltransform;

            grabbedItem.transform.SetParent(walltransform, true);
            grabbedItem = null;
            retVal = false;

        }
        else
        {
            RaycastHit hit;
            Ray ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out hit, MaxGrabDistance))
            {
                if(hit.collider.gameObject.tag == "Pickable")
                {
                    grabbedItem = hit.collider.gameObject;
                    //grabbedItem.GetComponent<Rigidbody>().isKinematic = true;
                    walltransform = grabbedItem.transform.parent;
                    retVal =  true;
                }
            }

        }
        return retVal;  
    }
}
