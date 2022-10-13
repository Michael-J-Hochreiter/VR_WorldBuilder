// AUTHOR: MICHAEL HOCHREITER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public InputActionProperty lTriggerPressed;
    public InputActionProperty lTriggerReleased;

    public InputActionProperty rTriggerPressed;
    public InputActionProperty rTriggerReleased;

    public StateMachine stateMachine;
    public Transform lHand;
    public Transform rHand;
    public Transform controllerHead;
    public GameObject selectionUI;

    public void OnEnable()
    {
        if (lTriggerPressed.action != null) lTriggerPressed.action.Enable();
        if (lTriggerPressed.action != null) lTriggerPressed.action.performed += TriggerPressed;
        if (lTriggerReleased.action != null) lTriggerReleased.action.Enable();
        if (lTriggerReleased.action != null) lTriggerReleased.action.performed += TriggerReleased;

        if (rTriggerPressed.action != null) rTriggerPressed.action.Enable();
        if (rTriggerPressed.action != null) rTriggerPressed.action.performed += TriggerPressed;
        if (rTriggerReleased.action != null) rTriggerReleased.action.Enable();
        if (rTriggerReleased.action != null) rTriggerReleased.action.performed += TriggerReleased;
    }

    void TriggerPressed(InputAction.CallbackContext trigger)
    {
        Debug.Log("Trigger Pressed (L or R)");

        //WENN stateMachine.state == State.idle, dann: -> raycast machen, der dann checkt ob ein objekt mit dem tag "editable" getroffen wurde. Wenn das passiert, dann wird das SelectionUI an der richtigen stelle gespawned. (Soll das SelectionUI seine rotation selber bestimmen? wäre wahrscheinlich cool). Sobald der trigger dann released wird, wird gechecked ob er eines der segmente vom ui hittet. Falls das passiert wird das ui despawned und in der state machine der state auf editing... gesetzt. außerdem wird in der statemachine das currentObject gesetzt. 
    }

    void TriggerReleased(InputAction.CallbackContext trigger)
    {
        Debug.Log("Trigger Released (L or R)");
    }
}