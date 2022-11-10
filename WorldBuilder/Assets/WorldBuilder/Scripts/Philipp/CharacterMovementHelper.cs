//@Author Philipp Thayer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : MonoBehaviour
{
    private XRRig XrRig;

    private CharacterController characterController;

    private CharacterControllerDriver driver;
    //Helps update the head's position on each frame

    void Awake()
    {
        driver = GetComponent<CharacterControllerDriver>();
        XrRig = GetComponent<XRRig>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterController();
    }
    
    private void UpdateCharacterController()
    {
        if (XrRig == null || characterController == null)
            return;

        var height = Mathf.Clamp(XrRig.cameraInRigSpaceHeight, driver.minHeight, driver.maxHeight);

        Vector3 center = XrRig.cameraInRigSpacePos;
        center.y = height / 2f + characterController.skinWidth;

        characterController.height = height;
        characterController.center = center;
    }
}
