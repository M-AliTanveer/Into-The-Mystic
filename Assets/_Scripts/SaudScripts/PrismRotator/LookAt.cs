using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // Start is called before the first frame update
    public float MinViewableDistance = 5f, ScaleLimiter=50f, MaxViewableDistance=70f;
    public float speed = 2f;
    public float height = 0.1f;
    public bool is_Rotating = true, is_Scaling = true;
    void Start()
    {

        foreach (Collider c in GameObject.FindGameObjectsWithTag("Player")[0].gameObject.GetComponents<Collider>())
        {
            Physics.IgnoreCollision(c, GetComponent<Collider>());
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(new Vector3(GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.x, GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.y, GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position.z));
        
        if (!is_Scaling)
        {
            var a = Vector3.Distance(GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.position, transform.position) / ScaleLimiter;
            transform.localScale = new Vector3(a, a, a);

        }
        if (!is_Rotating)
        {
            transform.Rotate(90.0f, 90.0f, 90.0f);

        }
        float newY = Mathf.Sin(Time.time * speed) * height / 250 + transform.position.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (Vector3.Distance(transform.GetComponent<Renderer>().bounds.center, GameObject.FindGameObjectsWithTag("Player")[0].transform.position) < MinViewableDistance
            || Vector3.Distance(transform.GetComponent<Renderer>().bounds.center, GameObject.FindGameObjectsWithTag("Player")[0].transform.position) > MaxViewableDistance)
        {
            if(transform.parent!=null)
            {
                transform.parent.GetComponent<Collider>().enabled = false;

            }
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

        }
        else
        {
            if (transform.parent != null)
            {
                transform.parent.GetComponent<Collider>().enabled = true;

            }
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;

        }
    }
}
