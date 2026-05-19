using System.Collections;
using TMPro;
using UnityEngine;

public class Sniper_Controller : MonoBehaviour
{
    [Header("Static Variables")]
    [SerializeField] private GameObject ShotPoint;
    [SerializeField] private float Speed;
    [SerializeField] private GameObject cam;
    [SerializeField] private int Max_Mag_Ammo;

    [Header("Animation Variables")]
    private float IdleTimer;
    [SerializeField] private float IdleTimerStartValue;
    private Animator _Animator;

    [Header("State Variables")]
    private int Current_Mag_Ammo;
    private bool Shell_In_Chamber = true;
    private bool reloading;

    [Header("Sniper Audio")]
    [SerializeField] private GameObject[] SniperAudioSources;

    [Header("Bullet Variables")]
    [SerializeField] private GameObject Bullet;
    [Tooltip("0 North, 1 East, 2 South, 3 West")][SerializeField] private Transform[] Directions;
    [Tooltip("0 Northern, 1 Southern")][SerializeField] private Transform[] HemisphereDirections;
    private MapDataStore _mapDataStore;

    [Header("Scope Variables")]
    private RaycastHit hit;
    [SerializeField] private Transform ScopePoint;
    [SerializeField] private TextMeshProUGUI DistanceText;

    void Start()
    {
        Current_Mag_Ammo = Max_Mag_Ammo;
        _Animator = GetComponent<Animator>();
        IdleTimer = IdleTimerStartValue;
        _mapDataStore = FindFirstObjectByType<MapDataStore>();

            // 0 disables VSync. Without this, targetFrameRate is ignored.
            QualitySettings.vSyncCount = 0;

            // Force the Game view to match this specific FPS
         Application.targetFrameRate = 30;
        
    }

    void Update()
    {
        if (!reloading)
        {
            Fire();
            ScopeAnimController();
            TryIdle();
            StartReload();
            ScopeDistanceGetter();
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Shell_In_Chamber && Current_Mag_Ammo >= 1)
        {
            if (_Animator.GetFloat("Blend") == 2 || _Animator.GetFloat("Blend") > 0.9f)
            {
                Current_Mag_Ammo -= 1;
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
        BulletScript.DirectionsSetter(Directions, HemisphereDirections, _mapDataStore);
        BulletScript.Initialize(transform, _mapDataStore);
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
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") + (Speed * Time.deltaTime));
        }
        else
        {
            _Animator.SetBool("Scoped", false);

            if (_Animator.GetFloat("Blend") > 1)
            {
                _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - (Speed * Time.deltaTime));
            }
        }
    }

    private void TryIdle()
    {
        if (IdleTimer > 0) { IdleTimer -= Time.deltaTime; }
        else
        {
            if (_Animator.GetFloat("Blend") > 0) { _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.deltaTime); }
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            IdleTimer = IdleTimerStartValue;
        }

        if (!Input.GetMouseButton(1) && _Animator.GetFloat("Blend") < 0.9f && _Animator.GetFloat("Blend") > 0)
        {
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.deltaTime);
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
            _Animator.SetFloat("Blend", _Animator.GetFloat("Blend") - Speed * Time.deltaTime);
            yield return null;
        }

        if (Current_Mag_Ammo >= 1)
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
        Current_Mag_Ammo = Max_Mag_Ammo;
        _Animator.SetFloat("Blend", 1);
    }

    private void PlaySound(int index)
    {
        SniperAudioSources[index].GetComponent<AudioSource>().Play();
    }

    private void ScopeDistanceGetter()
    {
        Ray ray = new Ray(ScopePoint.position, ScopePoint.transform.up);
        if (Physics.Raycast(ray, out hit, 1200f))
        {
            DistanceText.text = hit.distance.ToString() + "M";
        }
    }
}
