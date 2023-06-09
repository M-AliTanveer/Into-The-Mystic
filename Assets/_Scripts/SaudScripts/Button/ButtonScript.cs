using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ButtonScript : MonoBehaviour
{
    public GameObject Button;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;
    GameObject presser;
    bool isPressed;
    public Collider[] collidersToIgnore;
    private int objectsOnButton = 0;
    public bool use_rotation_animator=false;
    public Animator animator=null;
    public string Forward = "Forward";
    public string Backward = "Backward";
    private AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        foreach (Collider collider in collidersToIgnore)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider, true);
        }
        isPressed = false;
    }
    private void Update()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.up, out hit, 3f))
        //{
            
        //    if (isPressed && hit.collider == null)
        //    {
        //        objectsOnButton = 0;
        //        OnTriggerExit(null);
        //    }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if((other.CompareTag("Pickable") && other.CompareTag("Pickable") || other.CompareTag("Player") && other.CompareTag("Player")))
        {
            objectsOnButton++;
            if (!isPressed)
            {
                if (use_rotation_animator)
                {
                    animator.SetBool(Forward, true);
                    animator.SetBool(Backward, false);
                }
                source.Play();
                Button.transform.localPosition = new Vector3(0, 0.01f, 0);
                presser = other.gameObject;
                OnPress.Invoke();
                isPressed = true;
                StartCoroutine(CheckTriggerExit(other.gameObject));
            }
        }

        System.Collections.IEnumerator CheckTriggerExit(GameObject triggeringObject)
        {
            while (isPressed)
            {
                yield return null;

                if (!triggeringObject || !triggeringObject.activeInHierarchy)
                {
                    
                    OnTriggerExit(null);
                    isPressed = false;
                }
            }
        }

    }
    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        objectsOnButton--;
        if (other == null || (objectsOnButton<1 && (other.CompareTag("Pickable") && other.CompareTag("Pickable") || other.CompareTag("Player") && other.CompareTag("Player"))))
        {
            source.Play();
            if (use_rotation_animator)
            {
                animator.SetBool(Forward, false);
                animator.SetBool(Backward, true);
            }
            Button.transform.localPosition = new Vector3(0, 0.05f, 0);
            OnRelease.Invoke();
            isPressed = false;
        }
      

    }
}
