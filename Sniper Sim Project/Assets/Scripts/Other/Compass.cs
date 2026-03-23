using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Compass : MonoBehaviour
{
    [SerializeField] private RectTransform CompassBar;

    [SerializeField] private RectTransform NorthMark;
    [SerializeField] private RectTransform EastMark;
    [SerializeField] private RectTransform SouthMark;
    [SerializeField] private RectTransform WestMark;

    [SerializeField] private Transform Cam;
    [SerializeField] private Transform Objective;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetMarkerPos(NorthMark, Vector3.forward * 10000);
        SetMarkerPos(EastMark, Vector3.right * 10000);
        SetMarkerPos(SouthMark, Vector3.back * 10000);
        SetMarkerPos(WestMark, Vector3.left * 10000);
    }

    private void SetMarkerPos(RectTransform Marker, Vector3 WorldPos)
    {
        Vector3 MarkerDirection = WorldPos - Cam.transform.position;
        float Angle = Vector2.SignedAngle(new Vector2(MarkerDirection.x, MarkerDirection.z), new Vector2(Cam.transform.forward.x, Cam.transform.forward.z));

        float CompassPosX = Mathf.Clamp(2 * Angle / Cam.GetComponent<CinemachineCamera>().Lens.FieldOfView, -1, 1);
        Marker.anchoredPosition = new Vector2(CompassBar.rect.width / 2 * CompassPosX, 0);
    }
}
