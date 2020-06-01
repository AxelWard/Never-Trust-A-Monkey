using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioPlayer : MonoBehaviour
{
    public AudioClip hover;
    public AudioClip click;
    public AudioClip slide;

    public void playHover()
    {
        AudioSource a = gameObject.GetComponent<AudioSource>();
        a.clip = hover;
        a.Play();
    }

    public void playClick()
    {
        AudioSource a = gameObject.GetComponent<AudioSource>();
        a.clip = click;
        a.Play();
    }

    public void playSlide()
    {
        AudioSource a = gameObject.GetComponent<AudioSource>();
        a.clip = slide;
        a.Play();
    }
}
