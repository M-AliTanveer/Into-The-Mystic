using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interction : MonoBehaviour
{
    [SerializeField] GameObject ExpandedGoals;
    [SerializeField] GameObject CollapsedGoals;
    [SerializeField] GameObject KeyMappingGuide;
    bool collapsed = true;
    bool isMappingguideOpen = true;
    // Start is called before the first frame update
    void Start()
    {

        ExpandedGoals.SetActive(false);
        CollapsedGoals.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(collapsed)
            {
                ExpandedGoals.SetActive(true);
                CollapsedGoals.SetActive(false);
                collapsed= false;
            }
            else
            {
                ExpandedGoals.SetActive(false);
                CollapsedGoals.SetActive(true);
                collapsed= true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(isMappingguideOpen)
            {
                KeyMappingGuide.SetActive(false);
                isMappingguideOpen= false;
            }
            else
            {
                KeyMappingGuide.SetActive(true);
                isMappingguideOpen= true;
            }
        }
    }
}
