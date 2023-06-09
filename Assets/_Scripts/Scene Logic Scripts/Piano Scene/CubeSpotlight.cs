using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace PianoSceneLogic
{
    public class CubeSpotlight : MonoBehaviour
    {
        public PianoScene script;
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                script.CubeSpotlightReached();
            }
        }
    }
}
