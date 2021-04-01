using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlatform : MonoBehaviour
{
    public AudioSource rockSound;

    public void playRockSound()
    {
        rockSound.Play();
    }
}
