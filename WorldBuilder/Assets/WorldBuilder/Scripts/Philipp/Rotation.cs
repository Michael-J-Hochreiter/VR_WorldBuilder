//@Author Philipp Thayer

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
    private float angleChangeX;
    private float angleChangeY;
    private float angleChangeZ;
    private Vector3 previousRotationVector;
    private Vector3 currentRotationVector;
    
    private Vector2 previousRotationVectorX;
    private Vector2 currentRotationVectorX;
    private Vector2 previousRotationVectorY;
    private Vector2 currentRotationVectorY;
    private Vector2 previousRotationVectorZ;
    private Vector2 currentRotationVectorZ;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHandPosition = GameObject.FindWithTag("LeftController").transform.position;
        rightHandPosition = GameObject.FindWithTag("RightController").transform.position;
        
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle)
        {
            leftHandPosition = GameObject.FindWithTag("LeftController").transform.position;
            rightHandPosition = GameObject.FindWithTag("RightController").transform.position;
            //Handle rotation
            if (stateMachine.leftGrabPressed && stateMachine.rightGrabPressed && zoomObject.transform.childCount > 0)
            {
                calculate();
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
            currentRotationVectorX = new Vector2(rightHandPosition.y, rightHandPosition.z)  - new Vector2(leftHandPosition.y, leftHandPosition.z);
            currentRotationVectorY = new Vector2(rightHandPosition.x, rightHandPosition.z)  - new Vector2(leftHandPosition.x, leftHandPosition.z);
            currentRotationVectorZ = new Vector2(rightHandPosition.x, rightHandPosition.y)  - new Vector2(leftHandPosition.x, leftHandPosition.y);

            initialize = false;
        }
        
        //setting current values needed for calculation
        currentRotationVectorX = new Vector2(rightHandPosition.y, rightHandPosition.z)  - new Vector2(leftHandPosition.y, leftHandPosition.z);
        currentRotationVectorY = new Vector2(rightHandPosition.x, rightHandPosition.z)  - new Vector2(leftHandPosition.x, leftHandPosition.z);
        currentRotationVectorZ = new Vector2(rightHandPosition.x, rightHandPosition.y)  - new Vector2(leftHandPosition.x, leftHandPosition.y);
        
        //calculating change to initial vectors and total value changed
        angleChangeX = Vector2.SignedAngle(previousRotationVectorX, currentRotationVectorX);
        angleChangeY = Vector2.SignedAngle(previousRotationVectorY, currentRotationVectorY);
        angleChangeZ = Vector2.SignedAngle(previousRotationVectorZ, currentRotationVectorZ);
        print("y " + angleChangeY);
        print("x " + angleChangeX);
        print("z " + angleChangeZ);
        
        //setting new rotation with calculated values if max/min scale isn't reached yet
        zoomObject.transform.Rotate(new Vector3(-angleChangeX, -angleChangeY, -angleChangeZ) * 1.2f);
        
        //Resetting previous Vectors
        previousRotationVectorX = currentRotationVectorX;
        previousRotationVectorY = currentRotationVectorY;
        previousRotationVectorZ = currentRotationVectorZ;
    }
}
