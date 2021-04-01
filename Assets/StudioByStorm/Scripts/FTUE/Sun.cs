using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts.FTUE
{
    public class Sun : MonoBehaviour
    {
        public AudioSource flareSound;
        public AudioSource flareStartSound;

        public void playFlareSound()
        {
            flareSound.Play();
        }

        public void playFlareStartSound()
        {
            flareStartSound.Play();
        }
    }
}