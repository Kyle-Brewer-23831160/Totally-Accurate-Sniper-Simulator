using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Sniper_Controller : MonoBehaviour
{
    [Header("Sniper Variables")]
    [SerializeField] private int Mag_Ammo;
    [SerializeField] private GameObject ShotPoint;
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
}
