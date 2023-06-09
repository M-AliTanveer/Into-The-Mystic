using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockPlatform : MonoBehaviour
{
    public static bool isOnPlayer = false;
    private bool has_flipped=false;
    public Transform cube_gun;
    public GameObject key_obj;
    
    public UnityEvent RotationEvent, InverseRotationEvent;
    //void Start()
    //{
       
    //}
    void Update()
    {
        if(isOnPlayer && !has_flipped)
        {
            GetComponent<AudioSource>().Play();
            RotationEvent.Invoke();
            key_obj.SetActive(true);
            has_flipped = true;
        }
        else if (!isOnPlayer && has_flipped)
        {
            GetComponent<AudioSource>().Play();
            InverseRotationEvent.Invoke();
            has_flipped = false;
            key_obj.SetActive(false);

        }

    }

   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnPlayer = false;
        }
    }

    public bool IsOnPlayer()
    {
        return isOnPlayer;
    }
}
