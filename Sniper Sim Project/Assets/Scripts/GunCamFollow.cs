using UnityEngine;

public class GunCamFollow : MonoBehaviour
{
    [SerializeField] private Transform CamToFollow;
    [SerializeField] private Transform AdjustTransform;
    [SerializeField] private float speed;
    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, (CamToFollow.transform.localRotation * AdjustTransform.localRotation), speed * Time.fixedDeltaTime);
    }
}
