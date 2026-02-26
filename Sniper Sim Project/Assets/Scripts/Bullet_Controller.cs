using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    [Header("Base Bullet Variables")]
    [SerializeField] private float Speed;
    [SerializeField] private float Life_Time;
    private Rigidbody rb;
    [SerializeField] private Transform ForcePosition;

    [Header("Hemispherical Variables")]
    private Transform CurrentHemisphere;
    [SerializeField] private float HemisphereicalStrength;

    [Header("Wind Variables")]
    [SerializeField] private float WindSpeed;
    [SerializeField] private Transform WindDir;

    [Header("Coriolis Variables")]
    [SerializeField] private float CoriolisStrength;
    [SerializeField] private float StrengthMultiplier;
    private Transform East;
    private Transform West;
    private bool FacingEast;

    [Header("Hit raycast Variables")]
    private Vector3 OldPos;
    private Vector3 New;
    private RaycastHit hit;

    [Header("Debugging")]
    public GameObject Marker;

    public void Initialize(Transform StartPos, MapDataStore _MapDataStore)
    {
        WindSpeed = _MapDataStore.WindSpeedGetter();

        if (GameObject.FindGameObjectWithTag("Marker"))
        {
            Destroy(GameObject.FindGameObjectWithTag("Marker"));
        }

        New = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Impulse);

        FacingDirectionCalculations();
    }

    public void DirectionsSetter(Transform[] DirArray, Transform[] Hemispheres, MapDataStore _MapDataStore)
    {
        East = DirArray[1];
        West = DirArray[3];
     
        CurrentHemisphere = Hemispheres[_MapDataStore.GetHemisphereIndex()];

        WindDir = DirArray[_MapDataStore.DirectionGetter()];
    }

    private void FacingDirectionCalculations()
    {
        float AngleFromWest = 0;
        float AngleFromEast = 0;

        AngleFromWest = Vector3.SignedAngle(transform.forward, (West.position - transform.position), Vector3.up);
        AngleFromEast = Vector3.SignedAngle(transform.forward, (East.position - transform.position), Vector3.up); 

        AngleFromEast = Mathf.Abs(AngleFromEast);
        AngleFromWest = Mathf.Abs(AngleFromWest);

        if(AngleFromEast == AngleFromWest) { StrengthMultiplier = 0; return; }
        else if(AngleFromEast > AngleFromWest) { FacingEast = true; }

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
            GameObject mark = Instantiate(Marker, hit.point, transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }

    private void HemisphereAccount()
    {
        Vector3 HemisphereEffect = CurrentHemisphere.name == "North" ? Vector3.right : Vector3.left;
        rb.AddForce(HemisphereEffect * (HemisphereicalStrength), ForceMode.Acceleration);
    }

    private void CoriolisAccount()
    {
        //Directional Effects
        float multiplier = (CoriolisStrength / 100 * StrengthMultiplier);
        Vector3 direction = Vector3.zero;

        if (FacingEast) { direction = transform.up; }
        else { direction = -transform.up; }

        rb.AddForce(direction * multiplier, ForceMode.Force);
    }


    private void FixedUpdate()
    {
        //Life Timer
        Life_Time -= Time.deltaTime;
        if (Life_Time < 0)
        {
            Destroy(gameObject);
        }

        RayCheck();
        if (CurrentHemisphere != null) { HemisphereAccount(); }
        if(East != null) { CoriolisAccount(); }
    }
}

