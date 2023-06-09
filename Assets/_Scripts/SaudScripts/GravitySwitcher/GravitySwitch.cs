using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    public GameObject player;
    bool rotatetotem = false;
    float startTime, duration; Quaternion startRotation, endRotation;

    // Start is called before the first frame update
    void Start()
    {
        duration = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotatetotem)
        {
            float timePassed = Time.time - startTime;
            float t = timePassed / duration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            if (t >= 1)
            {
                rotatetotem = false;
            }
        }
    }

    public void GravityFlip()
    {
        if(player.GetComponent<FPSController>().pitchMinMax.x==-297f)
        {
            startRotation = transform.rotation;

            endRotation = Quaternion.Euler(0, 0, 0);
            //player.GetComponent<CharacterController>().enabled = false;
            //player.AddComponent<Rigidbody>().useGravity = true;
            //player.GetComponent<CharacterController>().enabled = false;
            //Destroy(player.GetComponent<Rigidbody>());

            //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 10f, player.transform.position.z);
            player.GetComponent<FPSController>().pitchMinMax = new Vector2(-453.1f, -250.1f);
           // player.GetComponent<FPSController>().pitchMinMax = new Vector2(-40f, 80f);

            player.GetComponent<FPSController>().gravity = -player.GetComponent<FPSController>().gravity;

            player.GetComponent<FPSController>().flipgravity = false;


            startTime = Time.time;
            rotatetotem = true;
        }
        else
        {
            startRotation = transform.rotation;

            endRotation = Quaternion.Euler(0, 0, 90);

            //if (player.GetComponent<Rigidbody>() != null)
            //{
            //    player.GetComponent<Rigidbody>().useGravity = false;

            //}
            player.GetComponent<FPSController>().pitchMinMax = new Vector2(-297f, -65.7f);
            player.GetComponent<FPSController>().gravity = -player.GetComponent<FPSController>().gravity;
            player.GetComponent<FPSController>().flipgravity = true;
            
            
            startTime = Time.time;
            rotatetotem = true;
        }


    }
}
