using UnityEngine;

public class DistanceGetter : MonoBehaviour
{
    [SerializeField] private GameObject OBJ1;
    [SerializeField] private GameObject OBJ2;
    void Start()
    {
        print(Vector3.Distance(OBJ1.transform.position, OBJ2.transform.position));
    }
}
