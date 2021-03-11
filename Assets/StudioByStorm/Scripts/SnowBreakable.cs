using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBreakable : MonoBehaviour
{
    // Desired duration of the shake effect
    protected float originalShakeDuration = 0.75f;
    protected float shakeDuration;
    // Time til shake
    protected float originalTimeTilShake = 0.25f;
    protected float timeTilShake;
    // A measure of magnitude for the shake. Tweak based on your preference
    protected float shakeMagnitude = 0.1f;
    // A measure of how quickly the shake effect should evaporate
    protected float dampingSpeed = 1.0f;
    // The initial position of the GameObject
    protected Vector3 initialPosition;

    protected bool bBreak;

    void OnEnable()
    {
        initialPosition = transform.localPosition;
        bBreak = false;
        shakeDuration = originalShakeDuration;
        timeTilShake = originalTimeTilShake;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            if (!bBreak) {
                bBreak = true;
            }
        }
    }

    void Update()
    {
        if (bBreak) {
            if (shakeDuration > 0) {

                if (timeTilShake <= 0) {
                    transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                    shakeDuration -= Time.deltaTime * dampingSpeed;
                } else {
                    timeTilShake -= Time.deltaTime;
                }

            } else {

                shakeDuration = 0f;
                transform.localPosition = initialPosition;
                gameObject.SetActive(false);
                
            }
        }
    }
}
