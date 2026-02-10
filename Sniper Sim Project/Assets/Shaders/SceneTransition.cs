using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Material TransMat;
    [SerializeField] private float TransTimeIn = 1f;
    private string PropertyName = "_Progress";
    private bool Transitioning;

    public UnityEvent OnTransitionComplete;

    public void StartTransitionIn()
    {
        if(Transitioning) { return; }
        StartCoroutine(TransitionIn());
    }

    public void StartTransitionOut()
    {
        if (Transitioning) { return; }
        StartCoroutine(TransitionOut());
    }

    public IEnumerator TransitionIn()
    {
        Transitioning = true;
        float curTime = 0;
        while (curTime < TransTimeIn)
        {
            curTime += Time.deltaTime;
            TransMat.SetFloat(PropertyName, Mathf.Clamp01(curTime / TransTimeIn));
            yield return null;
        }
        Transitioning = false;
        OnTransitionComplete?.Invoke();
    }

    public IEnumerator TransitionOut()
    {
        Transitioning = true;
        float curTime = 1f;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            TransMat.SetFloat(PropertyName, curTime);
            yield return null;
        }
        Transitioning = false;
        OnTransitionComplete?.Invoke();
    }
}
