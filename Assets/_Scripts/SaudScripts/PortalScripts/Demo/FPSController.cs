using IntoTheMystic.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller
{

    private float turretSlowdownTimer = 0f;
    public float turretSlowdownDuration = 2f;
    public float turretSlowdownSpeed = 1;
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    //public AudioSource source;
    //public AudioClip clip;
    bool jumping; public bool flipgravity = false;
    float lastGroundedTime;
    bool disabled;

    [SerializeField] private GameObject pickedObjectholder;

    void Start()
    {
        cam = Camera.main;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            disabled = !disabled;
        }
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    if(isParented)
        //    {
        //        //pickedObjectholder.GetComponent<Collider>().isTrigger = true;
        //    }
        //    gravityScript.LiftObject();
        //    if (isParented)
        //    {
        //        //BoxCollider holderCollider = pickedObjectholder.GetComponent<BoxCollider>();
        //        //holderCollider.enabled = false;
        //        //holderCollider.center = Vector3.zero;
        //        //holderCollider.size = Vector3.zero;
        //        isParented = false;
        //    }

        //}

        if (disabled)
        {
            return;
        }
        Vector3 inputDir;
        if (flipgravity)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            inputDir = new Vector3(input.x, 0, -input.y).normalized;
        }
        else
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            inputDir = new Vector3(input.x, 0, input.y).normalized;
        }

        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        float currentSpeed;
        //The code is commented for player slow down effect
        //if (HealthSystem.PlayerSlowDown == true || turretSlowdownTimer > 0)
        //{
        //    currentSpeed = turretSlowdownSpeed;
        //    if (HealthSystem.PlayerSlowDown == true)
        //    {
        //        HealthSystem.PlayerSlowDown = false;
        //        turretSlowdownTimer = 0;
        //    }

        //    turretSlowdownTimer += Time.deltaTime;

        //    if (turretSlowdownTimer > turretSlowdownDuration)
        //        turretSlowdownTimer = 0;

        //}
        //else
        //    currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        
        //if(verticalVelocity<-30f)
        //{
        //    source.PlayOneShot(clip);
        //}
        
        currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;

        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below || (flipgravity && flags == CollisionFlags.Above))
        {
            //Debug.Log($"Vertical Velocity :{verticalVelocity}");
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if ((controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f)))
            {
                jumping = true;
                verticalVelocity = jumpForce;
            }
            else if ((flipgravity && flags == CollisionFlags.Above) || (flipgravity && !jumping && timeSinceLastTouchedGround < 0.15f))
            {
                jumping = true;
                verticalVelocity = -jumpForce;
            }
        }

        float mX = Input.GetAxisRaw("Mouse X");
        float mY = Input.GetAxisRaw("Mouse Y");

        // Verrrrrry gross hack to stop camera swinging down at start
        float mMag = Mathf.Sqrt(mX * mX + mY * mY);
        if (mMag > 5)
        {
            mX = 0;
            mY = 0;
        }

        if (flipgravity)
        {
            yaw += -mX * mouseSensitivity;
            pitch -= mY * mouseSensitivity;
        }
        else
        {
            yaw += mX * mouseSensitivity;
            pitch -= mY * mouseSensitivity;
        }

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * smoothYaw;
        cam.transform.localEulerAngles = Vector3.right * smoothPitch;


        //if (gravityScript.grabbedItem && !isParented)
        //{
        //    Vector3 objectCenteredDifference = gravityScript.grabbedItem.GetComponentInChildren<Renderer>().bounds.center - gravityScript.grabbedItem.transform.position;
        //    objectCenteredDifference.x = 0;
        //    Rigidbody rb = gravityScript.grabbedItem.GetComponent<Rigidbody>();
        //    rb.MovePosition(Vector3.Lerp(rb.position, pickedObjectholder.transform.position - objectCenteredDifference, Time.deltaTime * 30.0f));
        //    if (gravityScript.grabbedItem.GetComponent<Collider>().bounds.Contains(pickedObjectholder.transform.position))
        //    {
        //        pickedObjectholder.transform.rotation = Quaternion.identity;
        //        Vector3 pos = gravityScript.grabbedItem.transform.position;
        //        Vector3 localPos = pickedObjectholder.transform.InverseTransformPoint(pos);
        //        gravityScript.grabbedItem.transform.SetParent(pickedObjectholder.transform);
        //        gravityScript.grabbedItem.transform.position = gravityScript.grabbedItem.transform.parent.position;
        //        //gravityScript.grabbedItem.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //        //gravityScript.grabbedItem.transform.localPosition = localPos;
        //        isParented = true;
        //    }
        //}
        //if (isParented && !pickedObjectholder.GetComponent<BoxCollider>().enabled)
        //{
        //    //BoxCollider[] itemColliders = gravityScript.grabbedItem.GetComponentsInChildren<BoxCollider>();
        //    //foreach (BoxCollider itemCollider in itemColliders)
        //    //{
        //    //    itemCollider.isTrigger = true;
        //    //}
        //    //BoxCollider holderCollider = pickedObjectholder.GetComponent<BoxCollider>();
        //    //holderCollider.isTrigger = false;
        //    //holderCollider.center = pickedObjectholder.transform.InverseTransformPoint(gravityScript.grabbedItem.transform.GetComponentInChildren<Renderer>().bounds.center);
        //    //holderCollider.size = itemColliders[0].bounds.size;
        //    //holderCollider.enabled = true;
        //    //foreach (BoxCollider itemCollider in itemColliders)
        //    //{
        //    //    itemCollider.enabled = false;
        //    //}

        //}

    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }

    public IEnumerator KnockBack(Vector3 forceDirection, float duration, float forceMagnitude, bool isPlayer)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Calculate the distance to move based on the elapsed time and force magnitude
            float distanceToMove = forceMagnitude * Time.deltaTime * 0.5f;
            
            if (isPlayer == true)
                transform.position += forceDirection * distanceToMove;
            else
                cam.transform.position += forceDirection * distanceToMove;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (isPlayer == false && elapsedTime >= duration)
        {
            cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.x, cam.transform.rotation.y, 90);
            this.enabled = false;
        }
    }

    public void OnDeath()
    {

        cam.transform.Find("Cube").gameObject.SetActive(false);
        cam.transform.Find("Firepoint").gameObject.SetActive(false);
        cam.transform.Find("ReflectionGuide").gameObject.SetActive(false);

        cam.transform.parent = null;
        Vector3 forceDirection = new Vector3(0, -1, 0);
        Transform Canvas = cam.transform.Find("Canvas");

        for (int i = 0; i < Canvas.childCount; i++)
        {
            if (!(Canvas.GetChild(i).gameObject.name == "blood splatter"))
                Canvas.GetChild(i).gameObject.SetActive(false);
            else
                Canvas.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(KnockBack(forceDirection, 0.2f, 3f, false));
        StartCoroutine(ReloadSave());



    }

    public IEnumerator ReloadSave()
    {
        yield return new WaitForSeconds(3);
        cam.transform.parent = gameObject.transform;
        gameObject.GetComponent<SaveScript>().LoadGame();
        this.enabled = true;
        cam.transform.Find("Canvas/blood splatter").gameObject.SetActive(false);
        cam.transform.Find("Cube").gameObject.SetActive(true);
        cam.transform.Find("Firepoint").gameObject.SetActive(true);
        cam.transform.Find("ReflectionGuide").gameObject.SetActive(true);
        Transform Canvas = cam.transform.Find("Canvas");

        for (int i = 0; i < Canvas.childCount; i++)
        {
            if (!(Canvas.GetChild(i).gameObject.name == "blood splatter") && !(Canvas.GetChild(i).gameObject.name == "Task Tracker Opened"))
                Canvas.GetChild(i).gameObject.SetActive(true);
        }
    }
}