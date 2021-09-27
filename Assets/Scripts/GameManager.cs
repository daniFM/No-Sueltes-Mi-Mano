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
    [SerializeField] private AudioSource source;

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

            if(levelIndex == 0 && SceneManager.GetActiveScene().buildIndex != 0)
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
        if(levelIndex < 4)
        {
            Debug.Log("NextLevel: " + levelIndex);
            StartCoroutine(LoadScene((Scene)levelIndex));
        }
        else
        {
            Debug.Log("GAME OVER");
            StartCoroutine(LoadScene((int)Scene.MainMenu, 0));
        }
    }

    public void RestartScene()
    {
        StartCoroutine(LoadScene((Scene)SceneManager.GetActiveScene().buildIndex));
    }

    public void EndGame()
    {
        Fade.instance.FadeIn(3);
        //levelIndex++;
        GameController.instance.phone.gameObject.SetActive(true);
    }

    private IEnumerator LoadScene(Scene scene, float fadeInTime = 1)
    {
        Fade.instance.FadeIn(fadeInTime);
        yield return new WaitForSeconds(fadeInTime);
        SceneManager.LoadScene((int)scene);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded");
        StartCoroutine(ExecuteAfterSeconds(0.5f));
        
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            source.enabled = false;
        }
        else
        {
            source.enabled = true;
        }
    }

    private IEnumerator ExecuteAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Fade.instance.FadeOut(1);
    }
}
