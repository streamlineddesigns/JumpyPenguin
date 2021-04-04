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
    public bool bFadeOutPenguinByScale;
    protected bool bPlayedFirstAnimation;
    protected bool bPlayedSecondAnimation;

    public GameObject ObjectToActivate;

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

                if (bFadeOutPenguinByScale && ! fadeOutPenguin.isPlaying && !bPlayedSecondAnimation) {

                    fadeOutPenguin.Play();
                    bPlayedSecondAnimation = true;

                } else if (! bFadeOutPenguinByScale || (! fadeOutPenguin.isPlaying && bPlayedSecondAnimation)) {
                    ObjectToActivate.SetActive(true);
                    gameObject.SetActive(false);
                }

            }
        }
    }
}
