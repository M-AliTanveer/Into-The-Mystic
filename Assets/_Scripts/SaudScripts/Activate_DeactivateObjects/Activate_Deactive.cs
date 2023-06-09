using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Deactive : MonoBehaviour
{
    public GameObject[] ObjectstoActivate;
    public bool enable = false;
    public bool disable = false;
    public GameObject[] ObjectstoDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleObjects(enable, disable);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        ToggleObjects(false);
    //    }
    //}

    private void ToggleObjects(bool enable, bool disable)
    {
        if(enable)
        {
            foreach (GameObject obj in ObjectstoActivate)
            {
                obj.SetActive(true);
            }
        }
        if (disable)
        {
            foreach (GameObject obj in ObjectstoDeactivate)
            {
                obj.SetActive(false);
            }
        }

    }
}
