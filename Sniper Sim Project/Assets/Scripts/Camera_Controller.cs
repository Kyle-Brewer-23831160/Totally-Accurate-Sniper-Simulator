using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    private float RotX;
    public float RotY;
    private Camera Cam;
    private float CurrentSpeed;
    [SerializeField] private float LookSpeed;
    [SerializeField] private float ScopeSpeed;
    [SerializeField] private float MaxXlimit, MinXlimit;
    [SerializeField] private GameObject PlayerParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cam = GetComponent<Camera>();
        CurrentSpeed = LookSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        lookContoller();
    }

    private void lookContoller()
    {
        RotX += Input.GetAxis("Mouse Y") * CurrentSpeed;
        RotX = Mathf.Clamp(RotX, MinXlimit, MaxXlimit);
        Cam.transform.localRotation = Quaternion.Euler(-RotX, 0, 0);

        RotY += Input.GetAxis("Mouse X") * CurrentSpeed;
        PlayerParent.transform.rotation = Quaternion.Euler(0, RotY, 0);

        print(Input.GetAxis("Mouse X"));
    }

    public void ChangeSpeed(bool scoped)
    {
        if(scoped)
        {
            CurrentSpeed = ScopeSpeed;
        }
        else
        {
            CurrentSpeed = LookSpeed;
        }
    }
}
