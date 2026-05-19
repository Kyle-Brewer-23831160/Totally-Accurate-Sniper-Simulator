using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private Material TransMat;
    private string PropertyName = "_Progress";

    private void Awake()
    {
        StartCoroutine(TransitionOut());
    }

    public IEnumerator TransitionOut()
    {
        float curTime = 1f;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            TransMat.SetFloat(PropertyName, curTime);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(0);
    }
}
