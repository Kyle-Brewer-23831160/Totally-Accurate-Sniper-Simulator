using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Target Data Asset")]
public class TargetDataObj : ScriptableObject
{
    public int TargetIndex;
    public string Position;
    public string action;
}
