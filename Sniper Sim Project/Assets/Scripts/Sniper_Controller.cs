using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Sniper_Controller : MonoBehaviour
{
    [Header("Sniper Variables")]
    [SerializeField] private int Mag_Ammo;
    [SerializeField] private GameObject ShotPoint;
    [SerializeField] private float Speed;
    private bool Shell_In_Chamber = true;

    [Header("Bullet Variables")]
    [SerializeField] private GameObject Bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (Input.GetMouseButton(1))
        {
            if (GetComponent<Animator>().GetFloat("Blend") >= 1) return;
            GetComponent<Animator>().SetFloat("Blend", GetComponent<Animator>().GetFloat("Blend") + Speed);
        }
        else
        {
            if (GetComponent<Animator>().GetFloat("Blend") <= 0) return;
            GetComponent<Animator>().SetFloat("Blend", GetComponent<Animator>().GetFloat("Blend") - Speed);
        }
    }
}
