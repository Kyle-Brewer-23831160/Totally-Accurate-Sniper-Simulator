using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomizeTarget : MonoBehaviour
{

    [SerializeField] private List<TargetDataObj> TargetsData = new List<TargetDataObj>();
    [SerializeField] private TextMeshProUGUI TargetInfoText;
    private TargetDataObj SelectedTarget;

    private void Start()
    {
        SelectedTarget = TargetsData[Random.Range(0, TargetsData.Count)];
        TargetInfoText.text = "Target Position: " + SelectedTarget.Position + "\n \n" + "Target Action: " + SelectedTarget.action;
    }

    public TargetDataObj GetTarget() { return SelectedTarget; }
}
