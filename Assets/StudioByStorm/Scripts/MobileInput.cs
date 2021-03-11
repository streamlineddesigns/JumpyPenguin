using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Singleton;
    public LeanJoystick LeanJoyStick;
    public Button JumpButton;
    public bool JumpButtonPressed = false;

    //swipe type
    public float previousSwipeType;//-1 for left : 1 for right

    //initial timer used for initialization and resets
    protected float initialSwipeTimer = 0.1f;
    //right swipe
    protected float swipeRightTimer;
    public bool SwipeRight = false;
    //left swipe
    protected float swipeLeftTimer;
    public bool SwipeLeft = false;
    //swipe up
    protected float previousJumpPosition;//-1 for down : 1 for up
    protected float swipeUpTimer;
    public bool SwipeUp = false;
    
    void Awake()
    {
        Singleton = this;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        //JumpButton.onClick.AddListener(JumpEvent);
        swipeRightTimer = initialSwipeTimer;
        swipeLeftTimer = initialSwipeTimer;
        swipeUpTimer = initialSwipeTimer;
        previousJumpPosition = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (JumpButtonPressed) {
            //StartCoroutine(OnJumpEventEnd());
        }

        //swipe right
        if ((! (MobileInput.Singleton.LeanJoyStick.ScaledValue.y > 0.75f)) && (previousSwipeType != 1)) {
            swipeRightInputListener();
        }

        //swipe left
        if ((! (MobileInput.Singleton.LeanJoyStick.ScaledValue.y > 0.75f)) && (previousSwipeType != -1)) {
            swipeLeftInputListener();
        }


        swipeUpInputListener();

    }

    IEnumerator OnJumpEventEnd()
    {
        yield return new WaitForSeconds(0.1f);
        JumpButtonPressed = false;
    }

    public void JumpEvent() {
        JumpButtonPressed = true;
    }


    protected void swipeRightInputListener() {

        if (MobileInput.Singleton.LeanJoyStick.ScaledValue.x > 0.0f) {

            swipeRightTimer -= Time.deltaTime;
            if (swipeRightTimer <= 0.0f) {
                SwipeRight = true;
                previousSwipeType = 1;
            }

        } else {
            swipeRightTimer = initialSwipeTimer;
            SwipeRight = false;
        }

    }

    protected void swipeLeftInputListener() {

        if (MobileInput.Singleton.LeanJoyStick.ScaledValue.x < 0.0f) {

            swipeLeftTimer -= Time.deltaTime;
            if (swipeLeftTimer <= 0.0f) {
                SwipeLeft = true;
                previousSwipeType = -1;
            }

        } else {
            swipeLeftTimer = initialSwipeTimer;
            SwipeLeft = false;
        }

    }

    protected void swipeUpInputListener() {

        if (MobileInput.Singleton.LeanJoyStick.ScaledValue.y > 0.75f) {

            swipeUpTimer -= Time.deltaTime;
            if (swipeUpTimer <= 0.0f) {
                if (previousJumpPosition == -1) {
                    SwipeUp = true;
                } else {
                    SwipeUp = false;
                }
                previousJumpPosition = 1;
            }

        } else if (MobileInput.Singleton.LeanJoyStick.ScaledValue.y < 0.75f) {
            previousJumpPosition = -1;
            swipeUpTimer = initialSwipeTimer;
            SwipeUp = false;
        }

    }
}