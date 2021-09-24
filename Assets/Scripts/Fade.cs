using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;

    public bool instantiated;

    private RawImage image;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        image = GetComponentInChildren<RawImage>();
        image.enabled = true;
        image.canvasRenderer.SetAlpha(0);
        instantiated = true;
    }

    public void FadeIn(float time = 1, Action callback = null)
    {
        if(instantiated)
        {
            image.canvasRenderer.SetAlpha(0);
            image.CrossFadeAlpha(1, time, true);
            StartCoroutine(FadeRoutine(time, callback));
        }
    }

    public void FadeOut(float time = 1, Action callback = null)
    {
        if(instantiated)
        {
            image.canvasRenderer.SetAlpha(1);
            image.CrossFadeAlpha(0, time, true);
            StartCoroutine(FadeRoutine(time, callback));
        }
    }

    public IEnumerator FadeRoutine(/*float from, float to, */float time, Action callback)
    {
        yield return new WaitForSeconds(time);

        callback?.Invoke();

        //float a = from;

        //while(a < to)
        //{
        //    a = from < to ? a + time * Time.deltaTime : a - time * Time.deltaTime;
        //    Debug.Log(a);
        //    image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        //    yield return null;
        //}

        //image.color = new Color(image.color.r, image.color.g, image.color.b, to);
    }
}
