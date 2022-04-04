using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerTheifControler : MonoBehaviour
{
    [SerializeField] GameData dattaRef;

    [SerializeField] Transform playerCamera = null;
    [SerializeField] Transform LeanPivot = null;


    [SerializeField] float mouseSensitivity = 3.5f;

    [SerializeField] float moveSpeed = 4.0f;
    [SerializeField] float walkSpeed = 4.0f;
    [SerializeField] float creepSpeed;
    [SerializeField] float crouceSpeed = 2.0f;


    [SerializeField] float gravity = 13.0f;
    [SerializeField] float jumpHeight = 1.5f;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;

    [SerializeField] bool lockCursor = true;

    [SerializeField] [Range(0.0f, 0.5f)] float moveSmotheTime = 0.3f;

    enum LeanState { Neutral, Forward, Left, Right}

    LeanState curentLean;

    public Quaternion currentPivot;
    public Vector3 curentPosition;

    CharacterController controller = null;

    Vector2 curDirection = Vector2.zero;
    Vector2 curDirVelocity = Vector2.zero;


    private bool m_crouch = false;
    private float m_originalHeight;
    [SerializeField] private float m_CrouchHeight = 0.5f;

    public bool canMove = true;

    public Transform PlayerCamera { get { return playerCamera; } }

    [SerializeField] FootStepsNoiseMaker noiseMaker;

    [SerializeField] Transform floorChecker;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        dattaRef = GameData.dataInstance;
        m_originalHeight = controller.height;

        noiseMaker = GetComponent<FootStepsNoiseMaker>();
        creepSpeed = walkSpeed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        lastpositionPreJump.z = transform.position.z;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            if (Input.GetKeyDown(KeyCode.Escape))
                lockCursor = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Escape))
                lockCursor = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = creepSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_crouch = !m_crouch;

            CheckCrouch();
        }

        if(dattaRef.ToggleCrouch == false)
        {
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                m_crouch = !m_crouch;
                CheckCrouch();
            }

        }
        
           UpdateMouseLook(mouseX, mouseY);

        if (canMove)
        {

            if (!LedgeDetected)
            {
                UpdateMovement(InputX, InputZ);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W))
                    FinnishLedgeClimb();
                else if (Input.GetKeyDown(KeyCode.S))
                    DropFromLedge();

            }
        }

        if (!isTouchingLedge)
            LeanSetUp();

        CheckLedgeClimb();
    }

    private void FixedUpdate()
    {
        CheckSuroundings();
    }

    void UpdateMouseLook(float x, float y)
    {

        cameraPitch -= y ;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        if (!LedgeDetected)
            transform.Rotate(Vector3.up * x);
    }


    private float positionPlacement = .5f;
    Vector3 ForwardLeanPlacement = new Vector3(0f, 0f, .7f);
    Vector3 RightLeanPlacement = new Vector3(-.7f, 0f, 0f);
    Vector3 LeftLeanPlacement = new Vector3(.7f, 0f, 0f);
    Vector3 BackLeanPlacement = new Vector3(0f, 0f, -.7f);

    float rotationPlacement = 15.0f;
    Quaternion NutralAngle = Quaternion.Euler(0f, 0f, 0f);

    Quaternion FowardLeanRotation = Quaternion.Euler(15f, 0f, 0f);
    Quaternion RightLeanRotation = Quaternion.Euler(0f, 0f, 15f);
    Quaternion LeftLeanRotation = Quaternion.Euler(0f, 0f, -15f);
    Quaternion BackLeanRotation = Quaternion.Euler(-15f, 0f, 0f);

    private float m_rotationPower = 0f;
    private float m_TransformPower = 0f;
    void LeanSetUp()
    {
        LeanPivot.localRotation = Quaternion.Slerp(LeanPivot.localRotation, currentPivot, 0.1f);
        LeanPivot.localPosition = Vector3.Lerp(LeanPivot.localPosition, curentPosition, 0.1f);

        switch (curentLean)
        {
            case LeanState.Neutral:

                if (Input.GetKeyDown(KeyCode.Q))              
                    curentLean = LeanState.Right;               
                else if (Input.GetKeyDown(KeyCode.E))
                    curentLean = LeanState.Left;
                else if (Input.GetKeyDown(KeyCode.X) && !LedgeDetected)
                    curentLean = LeanState.Forward;

                break;
            case LeanState.Forward:

                if(m_TransformPower != positionPlacement)
                {
                    m_TransformPower += .05f * Time.deltaTime;

                    if (m_TransformPower > positionPlacement)
                        m_TransformPower = positionPlacement;
                }

                        currentPivot = FowardLeanRotation;
                        curentPosition = ForwardLeanPlacement;               

                if (Input.GetKeyUp(KeyCode.X))
                {
                    currentPivot = NutralAngle;
                    curentPosition = Vector3.zero;
                    curentLean = LeanState.Neutral;
                }          
                break;
            case LeanState.Left:

                        currentPivot = LeftLeanRotation;
                        curentPosition = LeftLeanPlacement;              

                if (Input.GetKeyUp(KeyCode.E))
                {
                    currentPivot = NutralAngle;
                    curentPosition = Vector3.zero;
                    curentLean = LeanState.Neutral;
                }
                break;
            case LeanState.Right:

                        currentPivot = RightLeanRotation;
                        curentPosition = RightLeanPlacement;

                if (Input.GetKeyUp(KeyCode.Q))
                {
                    currentPivot = NutralAngle;
                    curentPosition = Vector3.zero;
                    curentLean = LeanState.Neutral;
                }
                break;
        }
    }

    Vector3 lastpositionPreJump;
    Vector3 pointforDropingOff;

    bool inAir = false;

    void UpdateMovement(float x, float z) //Fix
    {

        Vector3 targDir = transform.right * x + transform.forward * z;

        targDir.Normalize();

        curDirection = Vector2.SmoothDamp(curDirection, targDir, ref curDirVelocity, moveSmotheTime);


        controller.Move(targDir * moveSpeed * Time.deltaTime);

        if (x < 0 || x > 0 || z < 0 || z > 0)
        {
            if (!isWalking && isGrounded() && !inAir)
                PlayFootSound();
        }

        if (isGrounded() && velocityY < 0)
        {
            velocityY = -2.0f;
        }

        if(isGrounded() && inAir)
        {
            Invoke("PlayJumpLandingNoise", .045f);
            //PlayJumpLandingNoise();
            inAir = false;
        }


        velocityY -= gravity * Time.deltaTime;

        controller.Move(new Vector3(0, velocityY) * Time.deltaTime);
        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            lastpositionPreJump = transform.position;
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
            inAir = true;
        }


    }

    void CheckCrouch() //Fix keeps making the player sink in the floor ruining groundCheck
    {
        if (m_crouch == true)
        {
            controller.height = m_CrouchHeight;
            moveSpeed = crouceSpeed;
        }
        else
        {
            controller.height = m_originalHeight;
            moveSpeed = walkSpeed;
        }
    }

    [SerializeField]bool isWalking = false;
    [SerializeField] float footStepTimer = 3.5f;
    void PlayFootSound()
    {
        float timerBasedOnSpeed =  footStepTimer / moveSpeed;
        Debug.Log(timerBasedOnSpeed);
        StartCoroutine("PlayStepSound", timerBasedOnSpeed);
        noiseMaker.stopFireingNoises();
    }

    IEnumerator PlayStepSound(float timer)
    {
        var randomIndex = Random.Range(0, 5);
        noiseMaker.SetAudioBasedOnFloor(randomIndex);

        noiseMaker.audioSource.pitch = Mathf.Clamp((moveSpeed / 5), .6f, 1.3f);
        noiseMaker.audioSource.volume = Mathf.Clamp((moveSpeed / 5), .3f, 1.5f);
        noiseMaker.audioSource.Play();
        noiseMaker.FireNoise(noiseMaker.audioSource.volume);
        isWalking = true;

        yield return new WaitForSeconds(timer);
        isWalking = false;
    }


    void PlayJumpLandingNoise()
    {
        var randomInd = Random.Range(0, 5);
        noiseMaker.SetAudioBasedOnFloor(randomInd);

        noiseMaker.audioSource.pitch = .5f;
        noiseMaker.audioSource.volume = 1;

        noiseMaker.audioSource.Play();
        noiseMaker.FireNoise(noiseMaker.audioSource.volume);

        noiseMaker.stopFireingNoises();
    }

    bool isTouchingWall;
    bool isTouchingLedge;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallcheckDistance = 0.4f;
    [SerializeField] Transform LedgeCheck;
    [SerializeField] float ledgeCheckDistance = 0.4f;
    [SerializeField] LayerMask whatisGround;

    private bool canClimbLedge = false;
    private bool LedgeDetected;

    private Vector3 LedgePosBot;
    private Vector3 LedgePos1;
    private Vector3 LedgePos2;


    public float ledgeClimbOffsetZ = 0f;
    public float ledgeClimbOffsetY = 0f;

    public float LedgeClimbOffsetZ2 = 0f;
    public float LedgeClimbOffsetY2 = 0f;

    private void CheckSuroundings()
    {
        isTouchingWall = Physics.Raycast(wallCheck.position, transform.forward, wallcheckDistance, whatisGround);
        isTouchingLedge = Physics.Raycast(LedgeCheck.position, transform.forward, ledgeCheckDistance, whatisGround);

        if(isTouchingWall &&  !isTouchingLedge && !LedgeDetected)
        {
            LedgeDetected = true;

            LedgePosBot = wallCheck.position;
        }
    }

    private void CheckLedgeClimb()
    {
        if(LedgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            LedgePos1 = new Vector3(transform.position.x, Mathf.Floor(LedgePosBot.y + wallcheckDistance) + ledgeClimbOffsetY,
                Mathf.Floor(LedgePosBot.z + wallcheckDistance) - ledgeClimbOffsetZ);

            LedgePos2 = new Vector3(transform.position.x, Mathf.Floor(LedgePosBot.y + wallcheckDistance) + LedgeClimbOffsetY2,
                Mathf.Floor(LedgePosBot.z + wallcheckDistance) + LedgeClimbOffsetZ2);

            velocityY = 0.0f;
        }
    }

    public void FinnishLedgeClimb()
    {
        canClimbLedge = false;
        //transform.position = Vector3.Slerp(transform.position, LedgePos2, 3.0f);

        transform.DOMove(LedgePos2, 0.2f).SetEase(Ease.InOutQuad);

        LedgeDetected = false;
    }

    public void DropFromLedge()
    {
        canClimbLedge = false;
        //transform.position = Vector3.Slerp(transform.position, lastpositionPreJump, 3.0f) ;

        Vector3 backW = transform.TransformDirection(-Vector3.forward);

        transform.DOMove(transform.position + backW, 0.2f).SetEase(Ease.InOutQuad);
        
        LedgeDetected = false;
    }

    float groundDistance = 0.4f;

    bool isGrounded()
    {
        return Physics.CheckSphere(floorChecker.position, groundDistance, whatisGround) || m_crouch == true && velocityY >= -4;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3.forward * wallcheckDistance));
        Gizmos.DrawLine(LedgeCheck.position, LedgeCheck.position + (Vector3.forward * ledgeCheckDistance));
    }
}
