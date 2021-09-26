using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public PhoneController phone;

    private void Awake()
    {
        instance = this;
    }

    public void GrabHand()
    {
        Debug.Log("HAND GRABBED");
        GameManager.instance.GoToBedroom();
    }

    public void GrabPhone()
    {
        Debug.Log("PHONE GRABBED");
        //GameManager.instance.NextLevel();
        //phone.PerformChat(GameManager.instance.levelIndex);
        phone.gameObject.SetActive(true);
    }
}
