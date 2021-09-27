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

        //if(Input.GetMouseButton(0))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        if(Input.GetMouseButtonDown(1))
        {
            Cursor.visible = true;
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        transform.GetChild(0).gameObject.SetActive(paused);
        Cursor.visible = paused;

        if(paused)
        {
            Time.timeScale = 0.000001f;
            Cursor.lockState = CursorLockMode.None;
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
