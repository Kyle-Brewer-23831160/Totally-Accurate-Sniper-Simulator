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

    //Audio 
    [SerializeField] private GameObject[] SniperAudioSources;

    [Header("Bullet Variables")]
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform[] Directions; //0 North, 1 East, 2 South, 3 West;
    [SerializeField] private Transform[] HemisphereDirections; //0 Northern, 1 Southern
    private MapDataStore _mapDataStore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Animator = GetComponent<Animator>();
        IdleTimer = IdleTimerStartValue;
        _mapDataStore = FindFirstObjectByType<MapDataStore>();
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
                PlaySound(2);
                _Animator.Play("Recoil");
            }
        }
    }  

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(Bullet, ShotPoint.transform.position, transform.rotation);
        Bullet_Controller BulletScript = bullet.GetComponent<Bullet_Controller>();
        if (Directions[0] != null)
        {
            BulletScript.East = Directions[1];
            BulletScript.West = Directions[3];
            BulletScript.CoriolisDir = HemisphereDirections[_mapDataStore.GetHemisphereIndex()];
            BulletScript.WindDir = Directions[_mapDataStore.DirectionGetter()];
            BulletScript.WindSpeed = _mapDataStore.WindSpeedGetter();
        }
        BulletScript.Initialize(transform);
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
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") + Speed * Time.fixedDeltaTime);
        }
        else
        {
            _Animator.SetBool("Scoped", false);

            if (_Animator.GetFloat("Blend") > 1)
            {
                _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.fixedDeltaTime);
            }
        }
    }

    private void TryIdle()
    {
        if (IdleTimer > 0) { IdleTimer -= Time.deltaTime; }
        else
        {
            if (_Animator.GetFloat("Blend") > 0) { _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.fixedDeltaTime); }
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            IdleTimer = IdleTimerStartValue;
        }

        if (!Input.GetMouseButton(1) && _Animator.GetFloat("Blend") < 0.9f && _Animator.GetFloat("Blend") > 0)
        {
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.fixedDeltaTime);
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
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.fixedDeltaTime);
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
        _Animator.SetFloat("Blend", 1);
    }

    private void StopMagReload()
    {
        reloading = false;
        Shell_In_Chamber = true;
        Mag_Ammo = 5;
        _Animator.SetFloat("Blend", 1);
    }

    private void PlaySound(int index)
    {
        SniperAudioSources[index].GetComponent<AudioSource>().Play();
    }
}
