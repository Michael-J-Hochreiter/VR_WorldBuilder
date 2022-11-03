using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{

    [HideInInspector] public GameObject currentObject;
    public enum State
    {
        Idle,
        EditingRotation,
        EditingScaleAllAxis,
        EditingScaleIndividualAxis,
        EditingTranslation
    }

    [HideInInspector] public bool primaryPressed = false;
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

    [HideInInspector] public State state;
    
    // Start is called before the first frame update
    void Awake()
    {
        state = State.Idle;
    }
    
    public void OnEnable()
    {
        if (lGrabAction.action != null) lGrabAction.action.Enable();
        if (lGrabAction.action != null) lGrabAction.action.performed += lGrab;
        if (rGrabAction.action != null) rGrabAction.action.Enable();
        if (rGrabAction.action != null) rGrabAction.action.performed += rGrab;
        if (lGrabReleaseAction.action != null) lGrabReleaseAction.action.Enable();
        if (lGrabReleaseAction.action != null) lGrabReleaseAction.action.performed += lGrabRelease;
        if (rGrabReleaseAction.action != null) rGrabReleaseAction.action.Enable();
        if (rGrabReleaseAction.action != null) rGrabReleaseAction.action.performed += rGrabRelease;
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.Enable();
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.performed += buttonPressed;
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.Enable();
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.performed += buttonPressed;
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.Enable();
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.performed += buttonPressed;
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.Enable();
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.performed += buttonPressed;
    }
    
    private void lGrab(InputAction.CallbackContext grab){
        leftGrabPressed = true;
    }
    
    private void rGrab(InputAction.CallbackContext grab){
        rightGrabPressed = true;
    }
    
    private void rGrabRelease(InputAction.CallbackContext grab){
        rightGrabPressed = false;
        rightGrabReleased = true;
    }
    
    private void lGrabRelease(InputAction.CallbackContext grab){
        leftGrabPressed = false;
        leftGrabReleased = true;
    }

    private void buttonPressed(InputAction.CallbackContext button)
    {
        primaryPressed = true;
    }
    
}
