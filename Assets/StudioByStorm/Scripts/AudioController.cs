using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Singleton;
    public AudioSource SlideSound;
    public AudioSource JumpSound;
    public AudioSource WaterSplash;
    public AudioSource WolfAttack;
    public AudioSource EnemyDeath;
    public List<AudioSource> SnowWalkingSounds = new List<AudioSource>();
    protected bool isWalkingSoundPlaying;

    void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySlideSound()
    {
        if (! SlideSound.isPlaying) SlideSound.Play();
    }

    public void StopSlideSound()
    {
        if (SlideSound.isPlaying) SlideSound.Stop();
    }

    public void PlaySnowWalkingSound()
    {
        isWalkingSoundPlaying = false;

        for(int i = 0; i < SnowWalkingSounds.Count; i++) {
            if (SnowWalkingSounds[i].isPlaying) {
                isWalkingSoundPlaying = true;
            }
        }

        if (! isWalkingSoundPlaying) {
            int index = Random.Range(0, SnowWalkingSounds.Count);
            SnowWalkingSounds[index].Play();
            isWalkingSoundPlaying = true;
        }
    }

    public void StopSnowWalkingSound()
    {
        for(int i = 0; i < SnowWalkingSounds.Count; i++) {
            if (SnowWalkingSounds[i].isPlaying) {
                SnowWalkingSounds[i].Stop();
                isWalkingSoundPlaying = false;
            }
        }
    }

    public void PlayJumpSound()
    {
        if (! JumpSound.isPlaying) JumpSound.Play();
    }

    public void StopJumpSound()
    {
        if (JumpSound.isPlaying) JumpSound.Stop();
    }

    public void PlayWaterSplashSound()
    {
        if (! WaterSplash.isPlaying) WaterSplash.Play();
    }

    public void PlayWolfAttackSound()
    {
        if (! WolfAttack.isPlaying) WolfAttack.Play();
    }

    public void PlayEnemyDeathSound()
    {
        if (! EnemyDeath.isPlaying) EnemyDeath.Play();
    }
}
