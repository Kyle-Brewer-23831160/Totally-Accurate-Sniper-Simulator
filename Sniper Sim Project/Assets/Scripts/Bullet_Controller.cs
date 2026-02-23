using UnityEditorInternal;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Life_Time;
    [SerializeField] private Vector3 OldPos;
    [SerializeField] private Vector3 New;
    private RaycastHit hit;
    private Rigidbody rb;
    [SerializeField] private Transform ForcePosition;
    [SerializeField] private float CoriolisStrength;

    public void Initialize(Transform StartPos)
    {
        New = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);
    }

    private void RayCheck()
    {
        OldPos = New;
        New = transform.position;
        Ray ray = new Ray(OldPos, (New - OldPos).normalized);

        if (Physics.Raycast(ray, out hit, Vector3.Distance(OldPos, New)))
        {
            if (hit.transform.CompareTag("Target"))
            {
                Destroy(gameObject);
            }
            print("Hit " + hit.transform.name);
            Destroy(gameObject);
        }
    }

    private void timer()
    {
        Life_Time -= Time.deltaTime;
        if (Life_Time < 0)
        {
            Destroy(gameObject);
        }
    }

    private void CoriolisAccount()
    {
        rb.AddForce(Vector3.left * (CoriolisStrength / 100), ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        RayCheck();
        CoriolisAccount();
        timer();
    }

    //notes
    //if shot in north hemisphere, will tilt to the right
    //if shot in south hemisphere, will tilt to the left
    //if shot facing east, bullet will hit higher
    //if shot facing west, bullet withh hit lower
}
