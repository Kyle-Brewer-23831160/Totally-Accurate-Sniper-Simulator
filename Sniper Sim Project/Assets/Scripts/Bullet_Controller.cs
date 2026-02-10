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
    [SerializeField] private GameObject Marker;

    public void Initialize(Transform StartPos)
    {
        New  = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 350, ForceMode.Impulse);
    }

    private void RayCheck()
    {
        OldPos = New;
        New = transform.position;
        Ray ray = new Ray(OldPos, New - OldPos);

        if (Physics.Raycast(ray, out hit, 25))
        {
            if(hit.transform.CompareTag("Target"))
            {
                print("Hit " + hit.transform.name);
                Instantiate(Marker, OldPos, Marker.transform.rotation);
                Instantiate(Marker, ray.GetPoint(25), Marker.transform.rotation);
                Destroy(gameObject);
                
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    { 
        RayCheck();
    }
}
