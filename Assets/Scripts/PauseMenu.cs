using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        transform.GetChild(0).gameObject.SetActive(paused);

        if(paused)
        {
            Time.timeScale = 0.000001f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        TogglePause();
        GameManager.instance.RestartScene();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
