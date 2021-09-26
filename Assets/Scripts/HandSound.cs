using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSound : MonoBehaviour
{
    public enum Sound { hand, contact }
    public AudioSource source;
    public AudioClip[] clips;

    public void PlaySound(Sound sound)
    {
        source.PlayOneShot(clips[(int)sound]);
    }
}
