using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private  GameObject zoomObject;
    private  Vector3 leftHandPosition;
    private  Vector3 rightHandPosition;

    private StateMachine stateMachine;

    private bool initialize = true;
    
    private Vector3 previousMiddle;
    private Vector3 currentMiddle;
    private float angleChangeX;
    private float angleChangeY;
    private float angleChangeZ;
    private Vector3 previousRotationVector;
    private Vector3 currentRotationVector;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHandPosition = GameObject.FindWithTag("LeftController").transform.position;
        rightHandPosition = GameObject.FindWithTag("RightController").transform.position;
        
        stateMachine = GameObject.FindWithTag("RightHand").GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.EditingRotation)
        {
            //Handle rotation
            
            if (stateMachine.leftGrabPressed && stateMachine.rightGrabPressed)
            {
                calculate();
                //changeSliderValue();
            }
            else if (initialize == false)
            {
                initialize = true;
            }
            
        }
    }

    private void calculate()
    {
        if (initialize) {
            previousRotationVector = rightHandPosition  - leftHandPosition;
            
            initialize = false;
        }
        
        //setting current values needed for calculation
        currentRotationVector = rightHandPosition  - leftHandPosition;
        
        //calculating change to initial vectors and total value changed
        angleChangeY = Vector3.SignedAngle(previousRotationVector, currentRotationVector, Vector3.forward);
        angleChangeY = Vector3.SignedAngle(previousRotationVector, currentRotationVector, Vector3.up);
        angleChangeZ = Vector3.SignedAngle(previousRotationVector, currentRotationVector, Vector3.left);


        //setting new rotation with calculated values if max/min scale isn't reached yet
        zoomObject.transform.Rotate(new Vector3(-angleChangeX, -angleChangeY, -angleChangeZ) * 1.2f);
        
        //Resetting previous Vectors
        previousRotationVector = currentRotationVector;    }
}
