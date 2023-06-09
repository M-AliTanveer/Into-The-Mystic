using UnityEngine;

public class MainCamera : MonoBehaviour {

    Portal[] portals;

    void Awake () {
        portals = FindObjectsOfType<Portal> ();
    }

    void OnPreCull () {

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PrePortalRender ();
        }
        for (int i = 0; i < portals.Length; i++) {
            try
            {
                portals[i].Render();

            }
            catch (UnityEngine.UnityException e)
            {
                string errorMessage = e.Message;
                if (errorMessage.Contains("Screen position out of view frustum"))
                {
                    // Ignore the error and continue
                }
                else
                {
                    throw; // Rethrow other exceptions
                }
            }
        }

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PostPortalRender ();
        }

    }

}