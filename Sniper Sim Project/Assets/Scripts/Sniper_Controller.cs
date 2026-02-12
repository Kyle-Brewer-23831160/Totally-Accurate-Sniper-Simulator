using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject cam;
    private float IdleTimer;
    [SerializeField] private float IdleTimerStartValue;
    private bool reloading;

    [Header("Bullet Variables")]
    [SerializeField] private GameObject Bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Animator = GetComponent<Animator>();
        IdleTimer = IdleTimerStartValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reloading)
        {
            Fire();
            ScopeAnimController();
            TryIdle();
            StartReload();
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Shell_In_Chamber && Mag_Ammo >= 1)
        {
            if (_Animator.GetFloat("Blend") == 2 || _Animator.GetFloat("Blend") > 0.9f)
            {
                Mag_Ammo -= 1;
                Shell_In_Chamber = false;
                GameObject bullet = Instantiate(Bullet, ShotPoint.transform.position, transform.rotation);
                Bullet_Controller BulletScript = bullet.GetComponent<Bullet_Controller>();
                BulletScript.Initialize(transform);
                _Animator.Play("Recoil");
            }
        }
    }  

    private void ScopeAnimController()
    {
        if (_Animator.GetFloat("Blend") == 2)
        {
                cam.GetComponent<Camera_Controller>().ChangeSpeed(true);
        }
        else
        {
                cam.GetComponent<Camera_Controller>().ChangeSpeed(false);
        }

        if (Input.GetMouseButton(1))
        {
            if (_Animator.GetFloat("Blend") >= 2)
            {
                _Animator.SetFloat("Blend", 2);
                _Animator.SetBool("Scoped", true);
                return;
            }
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") + Speed);
        }
        else
        {
            _Animator.SetBool("Scoped", false);

            if (_Animator.GetFloat("Blend") > 1)
            {
                _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed);
            }
        }
    }

    private void TryIdle()
    {
        if (IdleTimer > 0) { IdleTimer -= Time.deltaTime; }
        else
        {
            if (_Animator.GetFloat("Blend") > 0) { _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed); }
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            IdleTimer = IdleTimerStartValue;
        }

        if (!Input.GetMouseButton(1) && _Animator.GetFloat("Blend") < 0.9f && _Animator.GetFloat("Blend") > 0)
        {
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed);
        }

        if(_Animator.GetFloat("Blend") < 0)
        {
            _Animator.SetFloat("Blend", 0);
        }
    }

    private void StartReload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!Shell_In_Chamber)
            {
                if (!reloading)
                {
                    StartCoroutine(MoveToReload());
                }
            }
        }
    }

    private IEnumerator MoveToReload()
    {
        while(_Animator.GetFloat("Blend") > -1)
        {
            reloading = true;
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed);
            yield return null;
        }

        if (Mag_Ammo >= 1)
        {
            _Animator.Play("Reload");
        }
        else
        {
            _Animator.Play("ReplaceMag");
        }

        yield return null;
    }

    private void StopReload()
    {
        reloading = false;
        Shell_In_Chamber = true;
        _Animator.SetFloat("Blend", 0);
    }

    private void StopMagReload()
    {
        reloading = false;
        Shell_In_Chamber = true;
        Mag_Ammo = 5;
        _Animator.SetFloat("Blend", 0);
    }
}
