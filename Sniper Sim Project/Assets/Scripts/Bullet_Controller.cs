using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Weight;
    [SerializeField] private float Life_Time;
    [SerializeField] private Vector3 OldPos;
    [SerializeField] private Vector3 New;
    private RaycastHit hit;
    private Rigidbody rb;
    [SerializeField] private Transform ForcePosition;

    private void Start()
    {

    }

    public void Initialize(Transform StartPos)
    {
        New  = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 500, ForceMode.Impulse);
    }

    private void RayCheck()
    {
        OldPos = New;
        New = transform.position;
        if(Physics.Raycast(OldPos, New, (OldPos - New).magnitude))
        {

        }
    }

    private void FixedUpdate()
    { 
        RayCheck();
    }
}
