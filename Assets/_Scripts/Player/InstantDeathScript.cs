using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            healthSystem.HealthBar = 0;
            healthSystem.updateHealth();
            healthSystem.PerformDeath();
        }
    }
}
