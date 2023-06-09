using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Material material = gameObject.GetComponent<Renderer>().material;
        float offset = Time.time * 0.4f;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
