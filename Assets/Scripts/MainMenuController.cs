using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        GameManager.instance.StartGame();
    }

    public void Credits()
    {

    }

    public void Exit()
    {
        //Application.Quit();
        Fade.instance.FadeOut(1, Application.Quit);
    }
}
