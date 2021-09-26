using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneController : MonoBehaviour
{
    public float messageDelay = 1;
    public GameObject messageL;
    public GameObject messageR;
    public ScrollRect scroll;
    public AudioSource source;
    public Transform chatContainer;
    public List<LevelChat> levelChats;

    void Start()
    {
        PerformChat(GameManager.instance.levelIndex);
    }

    public void PerformChat(int level)
    {
        StartCoroutine(ChatRoutine(level));
    }

    public IEnumerator ChatRoutine(int level)
    {
        Debug.Log("Performing chat: " + level);
        LevelChat lchat = levelChats[level];

        for(int i = 0; i < lchat.clips.Count; ++i)
        {
            yield return new WaitForSeconds(messageDelay);
            AudioClip clip = lchat.clips[i];
            source.PlayOneShot(clip);

            GameObject message;
            if(clip.name.Contains("_M"))
            {
                message = Instantiate(messageR, chatContainer);
            }
            else
            {
                message = Instantiate(messageL, chatContainer);
            }
            //message.transform.SetSiblingIndex(0);
            message.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = lchat.messages[i];

            yield return new WaitForSeconds(0.1f);
            scroll.verticalNormalizedPosition = 0;

            yield return new WaitForSeconds(clip.length);
        }

        yield return new WaitForSeconds(5);
        GameManager.instance.NextLevel();
    }
}

[System.Serializable]
public class LevelChat
{
    public List<AudioClip> clips;
    public List<string> messages;
}
