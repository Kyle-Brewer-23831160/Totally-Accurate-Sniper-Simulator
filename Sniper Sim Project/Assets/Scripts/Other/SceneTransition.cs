using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Material TransMat;
    [SerializeField] private float TransTimeIn = 1f;
    private GameObject Activatable;
    private GameObject DeActivatable;
    private string PropertyName = "_Progress";
    private bool Transitioning;
    private int NextSceneIndex;

    public UnityEvent OnTransitionHalfComplete;

    public int ChosenMapIndex;

    private void Start()
    {
        print("starting");
        SceneManager.activeSceneChanged += SceneAwake;
        StartCoroutine(TransitionIn());
        DontDestroyOnLoad(gameObject); ;
    }
    private void SceneAwake(Scene current, Scene next)
    {
        print("transitioning");
        StartCoroutine(TransitionIn());
    }

    public void SetActivatable(GameObject ObjectToActive)
    {
        Activatable = ObjectToActive;
    }
    public void SetDeActivatable(GameObject ObjectToDeActive)
    {
        DeActivatable = ObjectToDeActive;
    }

    public void StartTransitionIn()
    {
        if (Transitioning) { return; }
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
        yield return new WaitForSecondsRealtime(0.5f);
        Transitioning = false;
        OnTransitionHalfComplete?.Invoke();
    }

    public void DisplayNewMenu()
    {
        ChangeScene();
        DeActivatable.SetActive(false);
        Activatable.SetActive(true);
    }

    public void ChangeScene()
    {
        if (NextSceneIndex == 67) { return; }
        SceneManager.LoadScene(NextSceneIndex);
    }

    public void ChangeSceneIndex(int index)
    {
        if (ChosenMapIndex != 0)
        {
            NextSceneIndex = ChosenMapIndex;
        }
        else
        {
            NextSceneIndex = index;
        }
    }
    public void StartSetChosenNoDelay(int index)
    {
        ChosenMapIndex = index;
    }

    public void StartSetChosen(int index)
    {
        StartCoroutine(SetChosen(index));
    }

    public IEnumerator SetChosen(int index)
    {
        yield return OnTransitionHalfComplete;
        ChosenMapIndex = index;
    }
}
