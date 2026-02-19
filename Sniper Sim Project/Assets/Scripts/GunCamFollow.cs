using System.Runtime.CompilerServices;
using UnityEngine;

public class GunCamFollow : MonoBehaviour
{
    [SerializeField] private Transform CamToFollow;
    [SerializeField] private Transform AdjustTransform;
    [SerializeField] private float speed;
    void Update()
    {
        float AngleDistance = Quaternion.Angle(transform.localRotation, (CamToFollow.transform.localRotation * AdjustTransform.localRotation));
        AngleDistance = AngleDistance / 180;
        print(AngleDistance);
        Quaternion angleToFollow = CamToFollow.transform.localRotation * AdjustTransform.localRotation;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, angleToFollow, ((speed * 10) * AngleDistance) * Time.deltaTime);
    }
}
