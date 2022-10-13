using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnBlocks : MonoBehaviour
{
    public StateMachine stateMachine;

    public GameObject spawnPoint;
    public GameObject blockToSpawn;
    
    public InputActionProperty primaryButtonRightHand;
    public InputActionProperty secondaryButtonRightHand;
    public InputActionProperty primaryButtonLeftHand;
    public InputActionProperty secondaryButtonLeftHand;
    
    public void OnEnable()
    {
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.Enable();
        if (primaryButtonRightHand.action != null) primaryButtonRightHand.action.performed += buttonPressed;
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.Enable();
        if (secondaryButtonRightHand.action != null) secondaryButtonRightHand.action.performed += buttonPressed;
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.Enable();
        if (primaryButtonLeftHand.action != null) primaryButtonLeftHand.action.performed += buttonPressed;
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.Enable();
        if (secondaryButtonLeftHand.action != null) secondaryButtonLeftHand.action.performed += buttonPressed;
    }

    private void buttonPressed(InputAction.CallbackContext button)
    {
        if (stateMachine.state == StateMachine.State.Idle)
        {
            Instantiate(blockToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
    
}
