using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBlock : StateMachineBehaviour
{
    [SerializeField]
    private string Forward_Block, Backward_Block;
    [SerializeField]
    private bool Forward_Flag = false, Backward_Flag = false;


    // OnStateExit is called when a transition ends and the state 
    //machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Forward_Block, Forward_Flag);
        animator.SetBool(Backward_Block, Backward_Flag);
    }
}
