using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Sniper_Controller : MonoBehaviour
{
    [Header("Sniper Variables")]
    [SerializeField] private int Mag_Ammo;
    [SerializeField] private GameObject ShotPoint;
    [SerializeField] private GameObject SniperBody;
    [SerializeField] private float Speed;
    [SerializeField] private GameObject ScopeOverlay;
    private bool Shell_In_Chamber = true;
    private Animator _Animator;
    private GameObject cam;
    private Vector3 Start_Pos;
    private Vector3 Scope_Pos = new Vector3(0.517f, -0.235f, 0);
    private Quaternion Start_Rot;
    private Quaternion Scope_Rot = new Quaternion(0,0,0,0);

    [Header("Bullet Variables")]
    [SerializeField] private GameObject Bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Animator = GetComponent<Animator>();
        cam = Camera.main.gameObject;
        Start_Pos = transform.localPosition;
        Start_Rot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        Reload();
        ScopeAnimController();
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Shell_In_Chamber)
        {
            //call function to spawn bullet
            //feed bullet information such as sniper angle and position
            //Shell_In_Chamber = false;
            GameObject bullet = Instantiate(Bullet, ShotPoint.transform.position, transform.rotation);
            Bullet_Controller BulletScript = bullet.GetComponent<Bullet_Controller>();
            BulletScript.Initialize(transform);
        }
    }

    private void Reload()
    {
        if (!Shell_In_Chamber)
        {
            if (Mag_Ammo > 0)
            {
                //logic to put another bullet into the chamber
            }
            else
            {
                //logic to swap magazine
            }
        }
    }

    private void ScopeAnimController()
    {
        if (_Animator.GetFloat("Blend") == 1)
        {
            if (SniperBody.transform.GetChild(SniperBody.transform.childCount - 1).gameObject.activeInHierarchy)
            {
                for (int i = 0; i < SniperBody.transform.childCount; i++)
                {
                    SniperBody.transform.GetChild(i).gameObject.SetActive(false);
                }
                ScopeOverlay.SetActive(true);
                Camera.main.fieldOfView = 2f;
                cam.GetComponent<Camera_Controller>().ChangeSpeed(true);
                transform.localPosition = Scope_Pos;
                transform.localRotation = Scope_Rot;
            }
        }
        else
        {
            if (!SniperBody.transform.GetChild(SniperBody.transform.childCount - 1).gameObject.activeInHierarchy)
            {
                for (int i = 0; i < SniperBody.transform.childCount; i++)
                {
                    SniperBody.transform.GetChild(i).gameObject.SetActive(true);
                }
                ScopeOverlay.SetActive(false);
                Camera.main.fieldOfView = 35;
                cam.GetComponent<Camera_Controller>().ChangeSpeed(false);
                transform.localPosition = Start_Pos;
                transform.localRotation = Start_Rot;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (_Animator.GetFloat("Blend") >= 1)
            {
                _Animator.SetFloat("Blend", 1);
                _Animator.SetBool("Scoped", true);
                return;
            }
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") + Speed);
        }
        else
        {
            _Animator.SetBool("Scoped", false);

            if (_Animator.GetFloat("Blend") <= 0) 
            { 
                _Animator.SetFloat("Blend", 0);
                return; 
            }
               _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed);
        }
    }
}
