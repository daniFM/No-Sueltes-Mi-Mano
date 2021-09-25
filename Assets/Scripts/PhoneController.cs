using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject messageL;
    public GameObject messageR;
    public AudioSource source;
    public Transform chatContainer;
    public List<LevelChat> levelChats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformChat(int level)
    {
        StartCoroutine(ChatRoutine(level));
    }

    public IEnumerator ChatRoutine(int level)
    {
        LevelChat lchat = levelChats[level];

        for(int i = 0; i < lchat.clips.Count; ++i)
        {
            source.PlayOneShot(lchat.clips[i]);
            yield return new WaitForSeconds(lchat.clips[i].length);

            //Instantiate ...
        }
    }
}

[System.Serializable]
public class LevelChat
{
    public List<AudioClip> clips;
    public List<string> messages;
}
