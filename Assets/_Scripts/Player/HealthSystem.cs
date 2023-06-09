using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    //Health related variables
    private float maxPlayerHealth = 100f;
    public float HealthBar = 100f;
    public Slider healthBarSlider;

    //Player movement variable
    public static bool PlayerSlowDown = false;
    [Tooltip("Controls knockback force on the player")]
    public float forceMagnitude = 10f;

    //Health regeneration
    private bool canRegen = false;
    private bool startCooldown = false;
    private bool dead = false;

    [SerializeField]
    private float maxHealCooldown = 3f;
    [SerializeField]
    private float regenRate = 3f;
    private float healCooldownTimer = 3f;
    
    [SerializeField]
    private Image bloodImage = null;

    //Reference to scripts
    private FPSController fpsController;
    private turrentController turrentController;


    // Start is called before the first frame update
    void Start()
    {
        healthBarSlider.value = maxPlayerHealth;
        fpsController = GetComponent<FPSController>();
    }


    void Update()
    {
        if (startCooldown)
        {
            healCooldownTimer -= Time.deltaTime;
            if (healCooldownTimer < 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (HealthBar < maxPlayerHealth)
            {
                HealthBar += Time.deltaTime * regenRate;
                healthBarSlider.value = HealthBar;
                updateHealth();
            }
            else
            {
                HealthBar = maxPlayerHealth;
                healCooldownTimer = maxHealCooldown;
                canRegen = false;
            }
        }
    }

    public void updateHealth()
    {
        Color bloodSplatterAlpha = bloodImage.color;
        bloodSplatterAlpha.a = 1 - (HealthBar / maxPlayerHealth);
        bloodImage.color = bloodSplatterAlpha;
    }

    private void OnParticleCollision(GameObject other)
    {

        if (HealthBar > 0)
        {
            Transform parentTransform = other.transform.parent.parent.parent;
            turrentController = parentTransform.GetComponent<turrentController>();

            // Calculate the direction opposite to where the particle was collided
            Vector3 collisionDirection = transform.position - parentTransform.transform.position;
            // Normalize the collision direction and scale it to get the desired force magnitude
            Vector3 forceDirection = collisionDirection.normalized * forceMagnitude;
            forceDirection.y = 0;

            if (turrentController != null)
            {
                HealthBar -= turrentController.beamDamage;
                healthBarSlider.value = HealthBar;

                canRegen = false;
                healCooldownTimer = maxHealCooldown;
                startCooldown = true;

                updateHealth();

                PlayerSlowDown = true;
                StartCoroutine(fpsController.KnockBack(forceDirection, 0.5f, forceMagnitude, true));

            }

        }
        else if (dead == false)
        {
            PerformDeath();
        }
    }
    
    public void PerformDeath()
    {
        dead = true;
        fpsController.OnDeath();
    }

}