using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    MainMenu,
    Picnic,
    BusStation,
    Dinner,
    Bedroom
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int levelIndex = 1;

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

            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
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
        GoToBedroom();
    }

    public void GoToBedroom()
    {
        Debug.Log("GoToBedroom");
        StartCoroutine(LoadScene(Scene.Bedroom));
    }

    public void NextLevel()
    {
        levelIndex++;
        Debug.Log("NextLevel: " + levelIndex);
        StartCoroutine(LoadScene((Scene)levelIndex));
    }

    private IEnumerator LoadScene(Scene scene)
    {
        Fade.instance.FadeIn(1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene((int)scene);
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
