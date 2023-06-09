using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IntoTheMystic.ReUsableItems
{
    public class GenericButton : MonoBehaviour
    {
        public Rigidbody buttonTopRigid;
        public Transform buttonTop;
        public Transform buttonLowerLimit;
        public Transform buttonUpperLimit;
        public float threshHold;
        public float force = 10;
        private float upperLowerDiff;
        public bool isPressed;
        private bool prevPressedState;
        [SerializeField] public Collider[] CollidersToIgnore;
        [SerializeField] public UnityEvent onPressed;
        [SerializeField] public UnityEvent onReleased;
        public bool use_rotation_animator=false;
        public Animator animator = null;
        public string Forward="Forward";
        public string Backward= "Backward";
        private bool has_played_sound=false;
        public AudioSource audioSource;
        public AudioClip clip;
        public bool manualPress;
        private float lagTime = 1.7f, timer;

        void Start()
        {
            timer = Time.time;
            audioSource = GetComponent<AudioSource>();
            Collider localCollider = GetComponent<Collider>();
            if (localCollider != null)
            {
                Physics.IgnoreCollision(localCollider, buttonTop.GetComponentInChildren<Collider>());

                foreach (Collider singleCollider in CollidersToIgnore)
                {
                    Physics.IgnoreCollision(localCollider, singleCollider);
                }
            }

            if (transform.eulerAngles != Vector3.zero)
            {
                Vector3 savedAngle = transform.eulerAngles;
                transform.eulerAngles = Vector3.zero;
                upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
                transform.eulerAngles = savedAngle;
            }
            else
                upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y+.5f;

            Debug.Log(upperLowerDiff);
        }

        // Update is called once per frame
        void Update()
        {
            //if (timeRemaining > 0)
            //{
            //    timeRemaining -= Time.deltaTime;
            //}
            //Rigidbody rb = buttonTop.GetComponent<Rigidbody>();
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
            buttonTop.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (buttonTop.localPosition.y >= 0)
                buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
            else if(!manualPress)
                buttonTopRigid.AddForce(buttonTop.transform.up * force * buttonTopRigid.mass * Time.deltaTime *.4f);

            if (buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
                buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x, buttonLowerLimit.position.y, buttonLowerLimit.position.z);


            if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < (upperLowerDiff * threshHold)+.02f)
            {
                isPressed = true;

            }
            else
            {
                isPressed = false;

            }

            if (isPressed /*&& prevPressedState != isPressed*/)
                Pressed();
            if (!isPressed/* && prevPressedState != isPressed*/)
                Released();
        }

        public void Pressed()
        {
           // prevPressedState = isPressed;
            if(use_rotation_animator)
            {
                animator.SetBool(Forward, true);
                animator.SetBool(Backward, false);
            }
            if(!has_played_sound && !audioSource.isPlaying && Time.time - timer > lagTime /*&& timeRemaining>0f*/)
            {
                timer = Time.time;
                audioSource.PlayOneShot(clip);
                has_played_sound = true;
            }

            onPressed.Invoke();
           // Debug.Log("pressed");
        }

        public void Released()
        {
            has_played_sound = false;
            if (use_rotation_animator)
            {
                animator.SetBool(Forward, false);
                animator.SetBool(Backward, true);
            }
           // prevPressedState = isPressed;
            onReleased.Invoke();
        }
    
        
    }


}