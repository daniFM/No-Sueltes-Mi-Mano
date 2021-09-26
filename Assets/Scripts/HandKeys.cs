using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandKeys : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Keys")
        {
            Debug.Log("END GAME");
            GameManager.instance.EndGame();
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
}
