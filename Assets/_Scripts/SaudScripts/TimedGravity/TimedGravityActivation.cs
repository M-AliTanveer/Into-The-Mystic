using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGravityActivation : MonoBehaviour
{
    Rigidbody _rb;
    public float TimeUntilGravityActivation = 10f;
    public float TimeUntilDestruction = 0f;
    public bool destroyAfterGravity = false;
    private Renderer renderer;
    //private bool _gravity=true;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        renderer = gameObject.GetComponent<Renderer>();

        Invoke("GravityActivation", TimeUntilGravityActivation);
        if(destroyAfterGravity)
        {
            Invoke("DestructionObject", TimeUntilDestruction);
        }
    }
    //void Update()
    //{
    //    if(_gravity)
    //    {

    //        _rb.AddForce(-9.81f * transform.up * Time.deltaTime * 60f);
    //    }
    //}
    private void GravityActivation()
    {
        //_gravity = true;
        //_rb.useGravity = true;
        Invoke("RB_Collider_Disabler", 3f);
        //Invoke("Kinematics_Activation", 4f);
        Material material = new Material(Shader.Find("Standard (Specular setup)"));
        Color randomColor = new Color(Random.Range(0.2f, 0.4f), Random.Range(0.2f, 0.4f), Random.Range(0.2f, 0.4f));
        material.SetColor("_EmissionColor", randomColor);
        material.SetFloat("_EmissionScaleUI", -0.2f); // Set the emission intensity
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        Color randomAlbedo = new Color(Random.Range(0.8f, 1f), Random.Range(0.1f, 0.2f), Random.Range(0.1f, 0.2f));
        material.color = randomAlbedo; // Set the albedo color
        renderer.material = material;
    }
    void RB_Collider_Disabler()
    {
        _rb.isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        
    }
    private void DestructionObject()
    {
        Destroy(gameObject);

    }
    //private void Kinematics_Activation()
    //{
    //    _rb.isKinematic = true;
    //}

}
