using UnityEngine;

public class RigidbodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;

    [SerializeField] private Transform FeetTransform;
    [SerializeField] private LayerMask FloorMask;

    public float Speed;
    public float defaultSpeed;
    public float Sensitivity;
    public float Jumpforce;
    public float crouchSpeed;
    public float xRot;

    public bool grounded;
    public bool running;
    public bool air;
    public bool crouching;

    public int Jumps = 1;

    public Transform PlayerCamera;
    public Rigidbody PlayerBody;
    public Transform orientation;
    
    //Crouch & Slide
    [SerializeField] private float crouchHeight;
    [SerializeField] private float startScale;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startScale = transform.localScale.y;

    }
      
        
    private void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
        Running();
        doubleJump();
        Crouch();
        Backflip();
    }


    private void MovePlayer()
    {
        defaultSpeed = 7f;
        crouchSpeed = 4f;
        
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        
        grounded = (Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded == true)
            {
                PlayerBody.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
            }
        }
   
        if (grounded == true)
        {
            air = false;
        }
        if (grounded == false)
        {
            air = true;
        }
        
        
    }

    private void MovePlayerCamera()
    {
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        xRot -= PlayerMouseInput.y * Sensitivity;


        transform.Rotate(new Vector3(0f, PlayerMouseInput.x * Sensitivity, 0));
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void Running()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Speed = 12f;
            running = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Speed = 7f;
            running = false;
        }
    }

    private void doubleJump()
    {
        Jumps = Mathf.Clamp(Jumps, 0, 1);
        
        if (Input.GetKeyDown(KeyCode.Space) && Jumps == 1 && air == true)
        {
            Jumps--;
           
            PlayerBody.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        }

        if (Jumps == 0)
        {
            Jumps = 0;
        }

        if (grounded == true)
        {
            Jumps = 1;
        }
    
    }
    
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            YCrouched();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            NCrouched();
        }
    }
    void YCrouched()
    {
        crouching = true;
        PlayerBody.gameObject.transform.localScale = new Vector3(0.8f, crouchHeight, 0.8f);
        Speed = crouchSpeed;
    }
    void NCrouched()
    {
        crouching = false;
        PlayerBody.gameObject.transform.localScale = new Vector3(1, 1, 1);
        Speed = defaultSpeed;
    }

    void Backflip()
    {
        if(air == true && grounded == false)
        {
            xRot = Mathf.Clamp(xRot, -360, 360);
        }
        if(air == false && grounded == true)
        {
            xRot = Mathf.Clamp(xRot, -90f, 90f);
        }
    }
}
