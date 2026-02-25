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
    [SerializeField] private float BaseStrength;

    public void Initialize(Transform StartPos)
    {
        if(GameObject.FindGameObjectWithTag("Marker"))
        {
            Destroy(GameObject.FindGameObjectWithTag("Marker"));
        }

        New = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);

        if (CoriolisDir == null) { return; }

        Vector3 Dir = (CoriolisDir.position - transform.position).normalized;

        float AngleFromWest = Vector3.SignedAngle(transform.forward, (West.position - transform.position), Vector3.up);
        float AngleFromEast = Vector3.SignedAngle(transform.forward, (East.position - transform.position), Vector3.up);

        FacingEast = AngleFromEast < AngleFromWest;

        StrengthMultiplier = FacingEast ? Mathf.InverseLerp(-180, 180, AngleFromEast) : Mathf.InverseLerp(-180, 180, AngleFromWest);
        print("multiplier - " + StrengthMultiplier);
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
                print("hit Target");
            }
            print("hit floor");
            GameObject mark = Instantiate(Marker, hit.point, transform.rotation) as GameObject;
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
        rb.AddForce(HemisphereEffect * (CoriolisStrength), ForceMode.Acceleration);

        //Directional Effects
        float multiplier = (BaseStrength / 100) * StrengthMultiplier;
        Vector3 direction = Vector3.zero;

        if (FacingEast) {direction = East.position - transform.position; }
        else {direction = West.position - transform.position; }

        rb.AddForce(direction * multiplier, ForceMode.Force);
    }


    private void FixedUpdate()
    {
        RayCheck();
        if (CoriolisDir != null) { CoriolisAccount(); }
        timer();
    }

    //notes
    //if shot in north hemisphere, will tilt to the right
    //if shot in south hemisphere, will tilt to the left
    //if shot facing east, bullet will hit higher
    //if shot facing west, bullet withh hit lower
}

