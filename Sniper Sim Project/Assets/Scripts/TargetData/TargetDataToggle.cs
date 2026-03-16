using System.Collections;
using UnityEditor;
using UnityEngine;

public class TargetDataToggle : MonoBehaviour
{
    private Vector3 StartPos;
    [SerializeField] private float EndposX;
    [Range(0, 1)]
    public float progress;
    private bool transitioning;
    private bool forward;

    private void Start()
    {
        StartPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        Hide();
        if (!transitioning)
        {
            if (Input.GetKey(KeyCode.G)) { StartCoroutine(MoveTab()); }
        }
    }

    private void Hide()
    {
      transform.localPosition = Vector3.Lerp(StartPos, new Vector3(EndposX, StartPos.y, StartPos.z), progress);
    }

    private IEnumerator MoveTab()
    {
        transitioning = true;
        if(progress == 0) { forward = true; }

        if (forward)
        {
            while (progress < 1)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                progress += Time.deltaTime;
            }

            progress = 1;
        }
        else
        {
            while (progress > 0)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                progress -= Time.deltaTime;
            }

            progress = 0;
        }
        
        forward = false;
        transitioning = false;
    }
}
