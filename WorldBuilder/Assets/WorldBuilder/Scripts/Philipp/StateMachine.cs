//@Author Philipp Thayer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{

    public GameObject currentObject;
    public enum State
    {
        Idle,
        EditingRotation,
        EditingScaleAllAxis,
        EditingScaleIndividualAxis,
        EditingTranslation
    }

    [HideInInspector] public bool primaryPressed = false;
    [HideInInspector] public bool primaryReleased = false;
    [HideInInspector] public bool secondaryPressed = false;
    [HideInInspector] public bool secondaryReleased = false;
    [HideInInspector] public bool leftTriggerPressed = false;
    [HideInInspector] public bool rightTriggerPressed = false;
    [HideInInspector] public bool leftGrabPressed = false;
    [HideInInspector] public bool rightGrabPressed = false;
    [HideInInspector] public bool leftGrabReleased = false;
    [HideInInspector] public bool rightGrabReleased = false;
    
    public InputActionProperty lGrabAction;
    public InputActionProperty rGrabAction;
    public InputActionProperty lGrabReleaseAction;
    public InputActionProperty rGrabReleaseAction;
    public InputActionProperty primaryButtonRightHand;
    public InputActionProperty secondaryButtonRightHand;
    public InputActionProperty primaryButtonLeftHand;
    public InputActionProperty secondaryButtonLeftHand;
    public InputActionProperty primaryButtonRightHandReleased;
    public InputActionProperty secondaryButtonRightHandReleased;
    public InputActionProperty primaryButtonLeftHandReleased;
    public InputActionProperty secondaryButtonLeftHandReleased;

    [HideInInspector] public State state;
    
    // Start is called before the first frame update
    void Awake()
    {
        state = State.Idle;
    }
    
    public void OnEnable()
    {
        //Subscribing input actions to corresponding methods
        if (lGrabAction.action != null) lGrabAction.action.Enable();
        if (lGrabAction.action != null) lGrabAction.action.performed += lGrab;
        if (rGrabAction.action != null) rGrabAction.action.Enable();
        if (rGrabAction.action != null) rGrabAction.action.performed += rGrab;
        if (lGrabReleaseAction.action != null) lGrabReleaseAction.action.Enable();
        if (lGrabReleaseAction.action != null) lGrabReleaseAction.action.performed += lGrabRelease;
        if (rGrabReleaseAction.action != null) rGrabReleaseAction.action.Enable();
        if (rGrabReleaseAction.action != null) rGrabReleaseAction.action.performed += rGrabRelease;
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.Enable();
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.performed += primaryButtonPressed;
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.Enable();
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.performed += secondaryButtonPressed;
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.Enable();
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.performed += primaryButtonPressed;
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.Enable();
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.performed += secondaryButtonPressed;
        
        if (primaryButtonRightHandReleased.action != null) primaryButtonRightHandReleased.action.Enable();
        if (primaryButtonRightHandReleased.action != null) primaryButtonRightHandReleased.action.performed += primaryButtonReleased;
        if (secondaryButtonRightHandReleased.action != null) secondaryButtonRightHandReleased.action.Enable();
        if (secondaryButtonRightHandReleased.action != null) secondaryButtonRightHandReleased.action.performed += secondaryButtonReleased;
        if (primaryButtonLeftHandReleased.action != null) primaryButtonLeftHandReleased.action.Enable();
        if (primaryButtonLeftHandReleased.action != null) primaryButtonLeftHandReleased.action.performed += primaryButtonReleased;
        if (secondaryButtonLeftHandReleased.action != null) secondaryButtonLeftHandReleased.action.Enable();
        if (secondaryButtonLeftHandReleased.action != null) secondaryButtonLeftHandReleased.action.performed += secondaryButtonReleased;
    }
    
    private void lGrab(InputAction.CallbackContext grab){
        leftGrabPressed = true;
        leftGrabReleased = false;
    }
    
    private void rGrab(InputAction.CallbackContext grab){
        rightGrabPressed = true;
        rightGrabReleased = false;
    }
    
    private void rGrabRelease(InputAction.CallbackContext grab){
        rightGrabPressed = false;
        rightGrabReleased = true;
    }
    
    private void lGrabRelease(InputAction.CallbackContext grab){
        leftGrabPressed = false;
        leftGrabReleased = true;
    }

    private void primaryButtonPressed(InputAction.CallbackContext button)
    {
        primaryPressed = true;
    }
    
    private void primaryButtonReleased(InputAction.CallbackContext button)
    {
        primaryPressed = false;
        primaryReleased = true;
    }
    private void secondaryButtonPressed(InputAction.CallbackContext button)
    {
        secondaryPressed = true;
    }
    
    private void secondaryButtonReleased(InputAction.CallbackContext button)
    {
        secondaryPressed = false;
        secondaryReleased = true;
    }
}
