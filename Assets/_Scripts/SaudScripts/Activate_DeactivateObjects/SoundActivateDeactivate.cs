using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActivateDeactivate : MonoBehaviour
{
    public bool active = false;
    public bool deactive = false;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(active)
            {
                audioSource.Play();
            }
            if(deactive)
            {
                audioSource.Stop();
            }
            
        }
    }

}
