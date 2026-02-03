using UnityEditor.Rendering.Analytics;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Weight;
    [SerializeField] private float Life_Time;
    [SerializeField] private Vector3 Start_Pos;
    [SerializeField] private Vector3 Forward;
    private RaycastHit hit;
    public void Initialize(Transform StartPos)
    {
        Start_Pos = StartPos.position;
        Forward = StartPos.forward;
    }

    private Vector3 GetPositionOnArc(float Time)
    {
        Vector3 point = Start_Pos + (Forward *Speed * Life_Time);
        Vector3 GravityVector = Vector3.down * Weight * Time * Time;
        return point + GravityVector;
    }

    private bool CastRayBetweenPoints(Vector3 start, Vector3 End, out RaycastHit hit)
    {
        return Physics.Raycast(start, End - start, out hit, (End-start).magnitude);
    }

    private void FixedUpdate()
    {
        if(Life_Time > 0) {Life_Time -= Time.deltaTime; }

        float currentTime = Time.time - Life_Time;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPoint = GetPositionOnArc(currentTime);
        Vector3 nextPoint = GetPositionOnArc(nextTime);

        if(CastRayBetweenPoints(currentPoint, nextPoint, out hit))
        {
            print("hit");
        }
    }

    private void Update()
    {
        float currentTime = Time.time - Life_Time;
        Vector3 currentPoint = GetPositionOnArc(currentTime);
        transform.position = currentPoint;
    }
}
