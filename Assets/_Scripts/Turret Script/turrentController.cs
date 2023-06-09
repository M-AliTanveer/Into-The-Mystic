using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turrentController : MonoBehaviour
{
    public GameObject procrastinator = null;
    private bool isPlayerDetected = false;
    private float soundAnalyzeTime;

    //variables for detection
    [Tooltip("The max view distance that the enemy can target")]
    public float detectionRadius = 30.0f;
    [Tooltip("How much time to look at the objects that are creating a sound")]
    public float soundAnalyzingTimer = 5.0f;
    [Tooltip("Delay between each shots")]
    public float shootDelay = 2.0f;
    [Tooltip("The damage of this beam")]
    public float beamDamage = 20f;
    [Tooltip("if its on, the turret will rotate about 90 degrees, otherwise 45")]
    public bool highRotationAngle = false;

    //variables for rotation
    [Tooltip("The interval before the next rotation in seconds")]
    public float rotationTime = 3f;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float elapsedTime;
    private GameObject RotorBody;
    private ParticleSystem Beam;

    //For the bullet hit detection
    public static bool hit_detected = false;
    public static Vector3 hit_position = Vector3.zero;
    private float shootDelayCounter = 0f;
    private Vector3 previousPosition = Vector3.zero;


    void Start()
    {
        RotorBody = transform.Find("Gyroscope").Find("Rotor_Body").gameObject;
        Beam = RotorBody.transform.Find("BEAM").gameObject.GetComponent<ParticleSystem>();
        startRotation = RotorBody.transform.rotation;
        if (highRotationAngle)
            targetRotation = Quaternion.Euler(0f, 90f, 0f);
        else
            targetRotation = Quaternion.Euler(0f, 45f, 0f);

        soundAnalyzeTime = 0f;
        shootDelayCounter = shootDelay;
    }

    void Update()
    {
        Vector3 distance = procrastinator.transform.position - RotorBody.transform.position;

        if (distance.magnitude <= detectionRadius)
        {
            RaycastHit clearView;
            if (Physics.Raycast(RotorBody.transform.position, distance.normalized, out clearView, detectionRadius))
            {
                if ((Vector3.Dot(distance.normalized, RotorBody.transform.forward) > Mathf.Cos(45f * Mathf.Deg2Rad) || Input.GetKey(KeyCode.LeftShift)) &&
                    (clearView.transform.gameObject == procrastinator || clearView.transform.gameObject.CompareTag("Pickable")))
                {
                    Debug.Log("Player has been detected!");
                    RotorBody.transform.LookAt(procrastinator.transform);
                    TurretShoot(distance);
                    isPlayerDetected = true;
                }
                else
                    isPlayerDetected = false;
            }

        }
        else
            isPlayerDetected = false;

        if (isPlayerDetected)
            elapsedTime = 0f;
        else
        {
            if ((hit_position != previousPosition ) || soundAnalyzeTime > 0)
            {
                previousPosition = hit_position;
                distance = hit_position - RotorBody.transform.position;
                if (distance.magnitude <= detectionRadius)
                {
                    RotorBody.transform.LookAt(hit_position);
                    soundAnalyzeTime += Time.deltaTime;
                    if (soundAnalyzeTime > soundAnalyzingTimer)
                        soundAnalyzeTime = 0;
                }
            }
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= rotationTime)
        {
            startRotation = RotorBody.transform.rotation;
            RotorBody.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Time.deltaTime * 3.0f);
            
            if (Mathf.Abs(Quaternion.Dot(RotorBody.transform.rotation, targetRotation)) > 0.999f)
            {
                Debug.Log("Rotate");
                elapsedTime = 0f;
                if (highRotationAngle)
                    targetRotation = Quaternion.Euler(0f, RotorBody.transform.rotation.eulerAngles.y + 90f, 0f);
                else
                    targetRotation = Quaternion.Euler(0f, RotorBody.transform.rotation.eulerAngles.y + 45f, 0f);

            }
        }
    }

    void TurretShoot(Vector3 distance)
    {
        if (shootDelayCounter >= shootDelay)
        {
            shootDelayCounter = 0;
            var main = Beam.main;
            main.startLifetime = distance.magnitude / main.startSpeed.constant;
            Beam.Emit(100);
        }

        shootDelayCounter += Time.deltaTime;
    }
}