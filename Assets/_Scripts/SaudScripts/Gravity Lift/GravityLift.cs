using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLift : MonoBehaviour
{
    float throwForce = 600;
    Vector3 objectPos;
    float distance;

    public bool canHold = true;
    private GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;
    private Quaternion forward;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) )
        {
            
            if (isHolding==false)
            {
                int x = Screen.width / 2;
                int y = Screen.height / 2;

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 5f))
                {
                    if (hitInfo.transform.CompareTag("Pickable"))
                    {
                        GetComponent<AudioSource>().Play();
                        item = hitInfo.transform.gameObject;

                        isHolding = true;
                        item.transform.position = tempParent.transform.position;

                        item.GetComponent<Rigidbody>().useGravity = false;
                        item.GetComponent<Rigidbody>().detectCollisions = true;
                    }
                }
            }
            else
            {
                isHolding = false;
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody>().useGravity = true;
                //item.transform.position = objectPos;
            }

        }
        //else 
        //{
        //    isHolding = false;
        //}
        //Check if isholding
        if (isHolding == true)
        {
            distance = Vector3.Distance(item.transform.position, tempParent.transform.position);

            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            var hitColliders = Physics.OverlapSphere(item.GetComponentInChildren<Renderer>().bounds.center, item.transform.localScale.x);
            List<int> colliderList = new List<int>();
            foreach (var hitCollider in hitColliders)
            {
                if(!hitCollider.CompareTag("Player") && !hitCollider.CompareTag("Portal") && hitCollider!=item.GetComponent<Collider>())
                {
                    colliderList.Add(1);
                }
            }    

            if (colliderList.Count==0)
            {
                //item.transform.rotation=forward;
               // Vector3.Lerp(item.transform.position, tempParent.transform.position,.5f);
                item.transform.position = tempParent.transform.position;
                //item.transform.forward = tempParent.transform.forward;
                //forward = item.transform.rotation;

            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
            //    isHolding = false;
            //}
        }
        //else
        //{
        //    objectPos = item.transform.position;
        //    item.transform.SetParent(null);
        //    item.GetComponent<Rigidbody>().useGravity = true;
        //    item.transform.position = objectPos;
        //}
    }

    //void OnMouseDown()
    //{
    //    if (distance <= 1f)
    //    {
    //        isHolding = true;
    //        item.GetComponent<Rigidbody>().useGravity = false;
    //        item.GetComponent<Rigidbody>().detectCollisions = true;
    //    }
    //}
    //void OnMouseUp()
    //{
    //    isHolding = false;
    //}
}