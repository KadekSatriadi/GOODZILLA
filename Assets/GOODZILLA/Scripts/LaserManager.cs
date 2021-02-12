using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LaserManager : MonoBehaviour
{
    public VRTK_ControllerEvents LeftControllerEvents;
    public VRTK_ControllerEvents RightControllerEvents;
    public GameObject LeftLaserBeam;
    public GameObject RightLaserBeam;
    public AudioSource LaserSoundEffect;

    private bool isLeftGripPressed;
    private bool isRightGripPressed;


    public void Start()
    {
        LeftControllerEvents.GripPressed += LeftGripPressed;
        LeftControllerEvents.GripReleased += LeftGripReleased;
        RightControllerEvents.GripPressed += RightGripPressed;
        RightControllerEvents.GripReleased += RightGripReleased;

        LeftLaserBeam.SetActive(false);
        RightLaserBeam.SetActive(false);
    }

    private void LeftGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        isLeftGripPressed = true;

        if (isLeftGripPressed && isRightGripPressed)
            ShowLaserBeams();
    }

    private void LeftGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        isLeftGripPressed = false;

        HideLaserBeams();
    }

    private void RightGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        isRightGripPressed = true;

        if (isLeftGripPressed && isRightGripPressed)
            ShowLaserBeams();
    }

    private void RightGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        isRightGripPressed = false;

        HideLaserBeams();
    }

    private void ShowLaserBeams()
    {
        LeftLaserBeam.SetActive(true);
        RightLaserBeam.SetActive(true);
        LaserSoundEffect.Play();
    }

    private void HideLaserBeams()
    {
        LeftLaserBeam.SetActive(false);
        RightLaserBeam.SetActive(false);
        LaserSoundEffect.Stop();
    }
}
