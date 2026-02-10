using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Material TransMat;
    [SerializeField] private float TransTime = 1f;
    private string PropertyName = "_Progress";
    private bool Transitioning;

    public UnityEvent OnTransitionComplete;

    public void StartTransition()
    {
        if(Transitioning) { return; }
        StartCoroutine(TransitionIn());
    }
    public IEnumerator TransitionIn()
    {
        Transitioning = true;
        float curTime = 0;
        while (curTime < TransTime)
        {
            curTime += Time.deltaTime;
            TransMat.SetFloat(PropertyName, Mathf.Clamp01(curTime / TransTime));
            yield return null;
        }
        Transitioning = false;
        OnTransitionComplete?.Invoke();
    }
}
