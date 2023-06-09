using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWall : MonoBehaviour
{
    GameObject sister;
    Material[] m_Materials ;
    private int size = 0, i=0;
    // Start is called before the first frame update
    void Start()
    {
        sister = transform.parent.GetChild(0).gameObject;

        size = sister.GetComponent<NumberSwitchingWall>().ReturnSize();
        m_Materials = new Material[size];
        m_Materials = (Material[])sister.GetComponent<NumberSwitchingWall>().m_Materials.Clone();
        GetComponent<MeshRenderer>().material = m_Materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        //if(GetComponent<MeshRenderer>().enabled==true)
        //{             

        //}
    }
    public void UpgradeMaterial()
    {
        i++;
        GetComponent<MeshRenderer>().material = m_Materials[i % size];
    }
}
