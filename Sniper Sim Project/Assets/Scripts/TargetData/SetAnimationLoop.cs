using UnityEngine;

public class SetAnimationLoop : MonoBehaviour
{
    [SerializeField] private int AnimIndex;
    void Start()
    {
        GetComponent<Animator>().SetInteger("Entry", AnimIndex);
    }
}
