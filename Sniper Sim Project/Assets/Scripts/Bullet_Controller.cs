using UnityEditor;
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
    public float CoriolisStrength;
    public Transform CoriolisDir;
    public float WindSpeed;
    public Transform WindDir;
    public GameObject Marker;

    public Transform East;
    public Transform West;

    private bool FacingEast;
    private float StrengthMultiplier;

    public void Initialize(Transform StartPos)
    {
        New = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);

        Vector3 Dir = (CoriolisDir.position - transform.position).normalized;

        float AngleFromWest = Vector3.SignedAngle(transform.forward, (West.position - transform.position), Vector3.up);
        float AngleFromEast = Vector3.SignedAngle(transform.forward, (East.position - transform.position), Vector3.up);

        FacingEast = AngleFromEast < AngleFromWest;

        StrengthMultiplier = FacingEast ? Mathf.InverseLerp(-180, 180, AngleFromEast) : Mathf.InverseLerp(-180, 180, AngleFromWest);
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
            print("hit floor");
            Instantiate(Marker, hit.transform);
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
        //Hemisphere Effects
        Vector3 HemisphereEffect = CoriolisDir.name == "North" ? Vector3.right : Vector3.left;
        rb.AddForce(HemisphereEffect * (CoriolisStrength / 100), ForceMode.Acceleration);

        if(FacingEast) { rb.AddForce((East.position - transform.position) * (100 * StrengthMultiplier), ForceMode.Acceleration); }
        else { rb.AddForce((West.position - transform.position) * (100 * StrengthMultiplier), ForceMode.Acceleration); }
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
