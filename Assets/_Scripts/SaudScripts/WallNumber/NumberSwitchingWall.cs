using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSwitchingWall : MonoBehaviour
{
    int i = 0;
    public int max=3;
    public Material[] m_Materials ;
    private List<Transform> children; 
    // Start is called before the first frame update
    void Start()
    {
        children = new List<Transform>();
        GetComponent<Renderer>().material = m_Materials[0];
    }
    public int ReturnSize()
    {
        return max;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.childCount>0)
        {
            foreach (Transform child in transform)
            {

                children.Add(child);
                i++;
                child.transform.parent= null;

                if (children.Count <= max)
                {
                    GetComponent<MeshRenderer>().material = m_Materials[i];
                    if (children.Count == max)
                    {
                        //transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<MeshRenderer>().material = m_Materials[0];
                        transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                else if (children.Count > max)
                {

                    GetComponent<MeshRenderer>().material = m_Materials[max];
                    for (int y = 0; y < children.Count - max; y++)
                    {
                        Destroy(children[0].gameObject);
                        children.RemoveAt(0);
                        transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<MiniWall>().UpgradeMaterial();
                    }


                }
            }

        }
       
    }


}
