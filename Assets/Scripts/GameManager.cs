using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    MainMenu,
    Picnic,
    Dinner
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartGame()
    {
        Fade.instance.FadeIn(1, OnFadeFinished);
        
    }

    private void OnFadeFinished()
    {
        SceneManager.LoadScene((int)Scene.Picnic);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded");
        StartCoroutine(ExecuteAfterSeconds(0.5f));
    }

    public IEnumerator ExecuteAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Fade.instance.FadeOut(1);
    }
}
