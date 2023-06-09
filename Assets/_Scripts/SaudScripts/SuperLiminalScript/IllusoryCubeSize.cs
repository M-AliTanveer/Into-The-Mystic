using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusoryCubeSize : MonoBehaviour
{
    [Header("Components")]
    public Transform target;            

    [Header("Parameters")]
    public LayerMask targetMask;        
    public LayerMask ignoreTargetMask;  
    public float offsetFactor;          

    float originalDistance;         
    float originalScale;                
    Vector3 targetScale;

	public Camera cam_obj;
	int x, y;
    void Start()
    {

		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {


		HandleInput();
        ResizeTarget();
    }
	void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (target == null)
			{
				x = Screen.width / 2;
				y = Screen.height / 2;
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
				{
	
						target = hit.transform;
					    cam_obj.gameObject.SetActive(true);

						target.GetComponent<Rigidbody>().isKinematic = true;



						originalDistance = Vector3.Distance(Camera.main.transform.position, target.position);

						originalScale = target.localScale.x;

						targetScale = target.localScale;
					

				}
			}
			else
			{
				cam_obj.gameObject.SetActive(false);
				target.GetComponent<Rigidbody>().isKinematic = false;

				target = null;
			}
		}
	}

	void ResizeTarget()
	{
		if (target == null)
		{
			return;
		}
		x = Screen.width / 2;
		y = Screen.height / 2;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreTargetMask))
		{
			target.position = hit.point - Camera.main.transform.forward * targetScale.x;

			float currentDistance = Vector3.Distance(Camera.main.transform.position, target.position);

			float s = currentDistance / originalDistance;

			targetScale.x = targetScale.y = targetScale.z = s;

			target.localScale = targetScale * originalScale;
		}
	}
}
