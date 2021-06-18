using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;
using MoreMountains.NiceVibrations;

public class JoyStickHandle : MonoBehaviour
{
    public LeanJoystick LeanJoyStick;
    public GameObject OnJoyStickImage;
    public GameObject OffJoyStickImage;

    static public JoyStickHandle.HorizontalStateEnum HorizontalState;
    public enum HorizontalStateEnum {
        NA,
        LA,
        RA,
    }

    static public JoyStickHandle.VerticalStateEnum VerticalState;
    public enum VerticalStateEnum {
        OFF,
        ON,
    }

    public Image image;
    public GameObject UAOnImage;
    public GameObject UAOffImage;

    public GameObject LAOnImage;
    public GameObject LAOffImage;

    public GameObject RAOnImage;
    public GameObject RAOffImage;

    private void Start()
    {
        initToggles();
    }

    private void Update()
    {
        if (LeanJoyStick.ScaledValue.x == 0.0 && LeanJoyStick.ScaledValue.y == 0.0) {

            initToggles();
            JoyStickHandle.HorizontalState = HorizontalStateEnum.NA;
            JoyStickHandle.VerticalState = VerticalStateEnum.OFF;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnJoyStickImage.SetActive(false);
        OffJoyStickImage.SetActive(true);

        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);

        switch(collision.gameObject.tag) {
            
            case "UA1" :
                UAOnImage.SetActive(true);
                UAOffImage.SetActive(false);
                JoyStickHandle.VerticalState = VerticalStateEnum.ON;
                break;

            case "LA1" :
                initToggles();
                LAOnImage.SetActive(true);
                LAOffImage.SetActive(false);
                JoyStickHandle.HorizontalState = HorizontalStateEnum.LA;
                break;

            case "RA1" :
                initToggles();
                RAOnImage.SetActive(true);
                RAOffImage.SetActive(false);
                JoyStickHandle.HorizontalState = HorizontalStateEnum.RA;
                break;
        }
    }

    private void initToggles()
    {
        UAOnImage.SetActive(false);
        UAOffImage.SetActive(true);

        LAOnImage.SetActive(false);
        LAOffImage.SetActive(true);

        RAOnImage.SetActive(false);
        RAOffImage.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnJoyStickImage.SetActive(true);
        OffJoyStickImage.SetActive(false);

        if (LeanJoyStick.ScaledValue.x == 0.0 && LeanJoyStick.ScaledValue.y == 0.0) {

            initToggles();
            JoyStickHandle.HorizontalState = HorizontalStateEnum.NA;
            JoyStickHandle.VerticalState = VerticalStateEnum.OFF;

        } else if (collision.gameObject.tag == "UA1") {

            UAOnImage.SetActive(false);
            UAOffImage.SetActive(true);
            JoyStickHandle.VerticalState = VerticalStateEnum.OFF;

        }
    }
}
