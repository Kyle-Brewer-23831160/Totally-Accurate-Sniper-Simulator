using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement_Controller : MonoBehaviour
{
    [Header("Directional Movement Variables")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float Sprint_Speed;
    [SerializeField] private float Walk_Speed;
    private float Walk_Speed_Save;
    private float Horiz_Input;
    private float Vert_Input;
    private Transform Orientation;
    private Vector3 Dir;


    [Header("Keybinds")]
    [SerializeField] private KeyCode Jump_key = KeyCode.Space;
    [SerializeField] private KeyCode Sprint_Key = KeyCode.LeftShift;
    [SerializeField] private KeyCode Prone_Key = KeyCode.C;

    [Header("Gravity Values")]
    [SerializeField] private Transform GravityRaycastPoint;
    [SerializeField] private float RaycastDistance;
    [SerializeField] private float GravityValue;
    private RaycastHit Hit;
    [SerializeField]  private bool falling;

    [Header("Collider Values")]
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private float ProneSpeed;
    public bool proning;
    private float DefaultHeight;
    private float NewHeight;

    private void Awake()
    {
        Walk_Speed_Save = Walk_Speed;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Orientation = GetComponent<Transform>();

        DefaultHeight = capsuleCollider.height;
    }

    private void Update()
    {
        Inputs();
        ProneStateCheck();
    }

    private void FixedUpdate()
    {
        Movement();
        CustomGravity();
    }

    private void Inputs()
    {
        Horiz_Input = Input.GetAxisRaw("Horizontal");
        Vert_Input = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Movement()
    {
        if (!falling)
        {
            Dir = Orientation.forward * Vert_Input + Orientation.right * Horiz_Input; //calculates what direction to move the player based on orientation and key inputs
        }

        rb.linearVelocity = AdjustForSlopes();

        if (Input.GetKey(Sprint_Key))
        {
            Walk_Speed = Sprint_Speed; //swaps values
        }
        else
        {
            Walk_Speed = Walk_Speed_Save;
        }
    }

    private void ProneStateCheck()
    {
        if (Input.GetKeyDown(Prone_Key))
        {
            proning = !proning;
            NewHeight = capsuleCollider.height;
        }

        if (proning)
        {
            if (NewHeight > 0)
            {
                NewHeight = Mathf.Lerp(NewHeight, 0, ProneSpeed * Time.fixedDeltaTime);
                capsuleCollider.height = NewHeight;
            }
        }
        else
        {
            if (NewHeight < DefaultHeight)
            {
                NewHeight = Mathf.Lerp(NewHeight, DefaultHeight, ProneSpeed * Time.fixedDeltaTime);
                capsuleCollider.height = NewHeight;
            }
        }

    }

    private Vector3 AdjustForSlopes()
    {
        Ray ray = new Ray(GravityRaycastPoint.position, Vector3.down);

        if (Physics.Raycast(ray, out Hit, RaycastDistance))
        {
            Quaternion sloperotation = Quaternion.FromToRotation(Vector3.up, Hit.normal);
            Vector3 SlopeCheck = sloperotation * (Dir.normalized * Walk_Speed_Save); //check using an unchanging variable
            Vector3 NewVelocity = sloperotation * (Dir.normalized * Walk_Speed);

            if (SlopeCheck.y < 0)
            {
                if (SlopeCheck.y > -3) //slope isnt steep enough to justify forced movement
                {
                    falling = false;
                    return NewVelocity;
                }
                else //slope is steep, force movement
                {
                    falling = true;
                    NewVelocity = sloperotation * (Dir.normalized * 15);
                    return NewVelocity;
                }
            }
            else //player is level or moving up
            {
                falling = false;
                return Dir.normalized * Walk_Speed;
            }
        }

        CustomGravity(); //no floor hit by raycast, player is falling
        return Vector3.zero;
    }

    private void CustomGravity() 
    {
      rb.AddForce(Vector3.down * (GravityValue * 1000) * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}

