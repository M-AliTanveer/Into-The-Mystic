using IntoTheMystic.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkPointNumber;
    public string NextInstructionsSimple;
    public string NextInstructionsDetailed;
    bool isCheckpointTriggered;
    void OnTriggerEnter(Collider other)
    {
        if (!isCheckpointTriggered)
        {
            isCheckpointTriggered= true;
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                SaveScript saveScript = player.GetComponent<SaveScript>();
                if (saveScript != null)
                {
                    GameObject taskTrackerExpanded = player.transform.Find("Main Camera/Canvas/Task Tracker Opened").gameObject;
                    if (taskTrackerExpanded != null)
                    {
                        taskTrackerExpanded.transform.Find("Task Tracker BG/TaskText").gameObject.GetComponent<TMP_Text>().text = NextInstructionsDetailed;
                    }
                    GameObject taskTrackerCollapsed = player.transform.Find("Main Camera/Canvas/Task Tracker Collapsed").gameObject;
                    if (taskTrackerCollapsed != null)
                    {
                        taskTrackerCollapsed.transform.Find("Task Tracker BG/TaskText").gameObject.GetComponent<TMP_Text>().text = NextInstructionsSimple;

                    }
                    Canvas.ForceUpdateCanvases();
                    player.transform.Find("Main Camera/Canvas/Checkpoint Update").gameObject.SetActive(true);
                    Animator checkpointUpdateAnimatiopn = player.transform.Find("Main Camera/Canvas/Checkpoint Update").gameObject.GetComponent<Animator>();

                    checkpointUpdateAnimatiopn.SetTrigger("Stop");
                    StartCoroutine(DisableEvent(1f,player));
                    saveScript.AutoSave(checkPointNumber, NextInstructionsSimple +','+ NextInstructionsDetailed);
                    Debug.Log("saved");

                }
            }
        }
    }

    IEnumerator DisableEvent(float delay, GameObject player)
    {
        yield return new WaitForSeconds(delay);
        player.transform.Find("Main Camera/Canvas/Checkpoint Update").gameObject.SetActive(false);

    }
}
