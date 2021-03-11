using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashView : MonoBehaviour
{
    public Slider progressSlider;
    public GameObject Title;
    protected float increment = (1.0f / 60.0f) / 1.0f;
    public Animation fadeOutView;
    public Animation fadeOutPenguin;
    protected bool bPlayedFirstAnimation;
    protected bool bPlayedSecondAnimation;

    public GameObject StartView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (progressSlider.value < 1.0f) {
            progressSlider.value += increment;
        } else {
            if (! fadeOutView.isPlaying && !bPlayedFirstAnimation) {

                fadeOutView.Play();
                bPlayedFirstAnimation = true;

            } else if (! fadeOutView.isPlaying && bPlayedFirstAnimation) {

                if (! fadeOutPenguin.isPlaying && !bPlayedSecondAnimation) {

                    fadeOutPenguin.Play();
                    bPlayedSecondAnimation = true;

                } else if (! fadeOutPenguin.isPlaying && bPlayedSecondAnimation) {
                    StartView.SetActive(true);
                    gameObject.SetActive(false);
                }

            }
        }
    }
}
