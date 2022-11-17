//@Author Philipp Thayer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationForce = 1.0f;
    
    private  GameObject zoomObject;
    private  Vector3 leftHandPosition;
    private  Vector3 rightHandPosition;

    private StateMachine stateMachine;

    private bool initialize = true;
    private float angleChangeX;
    private float angleChangeY;
    private float angleChangeZ;

    private float xDistance;
    private float yDistance;
    private float zDistance;
    private Vector3 initialPosition;

    private float totalAngleChangeX;
    private float totalAngleChangeY;
    private float totalAngleChangeZ;

    private Quaternion initialRotation;
    private String currentAxis = "";
    private String previousAxis = "";
    private bool initializeAxis = true;

    private Vector2 previousRotationVectorX;
    private Vector2 currentRotationVectorX;
    private Vector2 previousRotationVectorY;
    private Vector2 currentRotationVectorY;
    private Vector2 previousRotationVectorZ;
    private Vector2 currentRotationVectorZ;
    private UpdateParentPosition updateParent;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHandPosition = GameObject.FindWithTag("LeftController").transform.position;
        rightHandPosition = GameObject.FindWithTag("RightController").transform.position;
        
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
        updateParent = GetComponent<UpdateParentPosition>();
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
                if (initialize)
                {
                    updateParent.updateParent();
                    initialization();
                }
                updateVectors();
                determineAxis();
                applyRotation();
            }
            else if (initialize == false)
            {
                initialize = true;
                initializeAxis = true;
            }
            
        }
    }

    private void initialization()
    {
        previousRotationVectorX = new Vector2(rightHandPosition.y, rightHandPosition.z)  - new Vector2(leftHandPosition.y, leftHandPosition.z);
        previousRotationVectorY = new Vector2(rightHandPosition.x, rightHandPosition.z)  - new Vector2(leftHandPosition.x, leftHandPosition.z);
        previousRotationVectorZ = new Vector2(rightHandPosition.x, rightHandPosition.y)  - new Vector2(leftHandPosition.x, leftHandPosition.y);
        totalAngleChangeX = 0;
        totalAngleChangeY = 0;
        totalAngleChangeZ = 0;
        initialRotation = zoomObject.transform.localRotation;
        initialPosition = zoomObject.transform.position;

        initialize = false;
    }

    private void determineAxis()
    {
        if (Math.Abs(totalAngleChangeX) >= Math.Abs(totalAngleChangeY) && Math.Abs(totalAngleChangeX) >= Math.Abs(totalAngleChangeZ))
        {
            currentAxis = "x";
        }
        else if (Math.Abs(totalAngleChangeY) >= Math.Abs(totalAngleChangeX) && Math.Abs(totalAngleChangeY) >= Math.Abs(totalAngleChangeZ))
        {
            currentAxis = "y";
        }
        else
        {
            currentAxis = "z";
        }
        if (initializeAxis)
        {
            previousAxis = currentAxis;
            initializeAxis = false;
        }    
    }

    private void updateVectors()
    {
        //setting current values needed for calculation
        currentRotationVectorX = new Vector2(rightHandPosition.y, rightHandPosition.z)  - new Vector2(leftHandPosition.y, leftHandPosition.z);
        currentRotationVectorY = new Vector2(rightHandPosition.x, rightHandPosition.z)  - new Vector2(leftHandPosition.x, leftHandPosition.z);
        currentRotationVectorZ = new Vector2(rightHandPosition.x, rightHandPosition.y)  - new Vector2(leftHandPosition.x, leftHandPosition.y);
        
        xDistance = Vector3.Distance(rightHandPosition,
            new Vector3(leftHandPosition.x, rightHandPosition.y, rightHandPosition.z));
        yDistance = Vector3.Distance(rightHandPosition,
            new Vector3(rightHandPosition.x, leftHandPosition.y, rightHandPosition.z));
        zDistance = Vector3.Distance(rightHandPosition,
            new Vector3(rightHandPosition.x, rightHandPosition.y, leftHandPosition.z));
        
        //calculating change to initial vectors and total value changed
        angleChangeX = Vector2.SignedAngle(previousRotationVectorX, currentRotationVectorX) * yDistance * 2;
        angleChangeY = Vector2.SignedAngle(previousRotationVectorY, currentRotationVectorY) * zDistance * 2;
        angleChangeZ = Vector2.SignedAngle(previousRotationVectorZ, currentRotationVectorZ) * xDistance * 2;

        //Updating the total angle change to later determine correct axis for rotation
        totalAngleChangeX += angleChangeX;
        totalAngleChangeY += angleChangeY;
        totalAngleChangeZ += angleChangeZ;
    }

    private void applyRotation()
    {
        //setting new rotation with calculated values
        if (currentAxis == previousAxis)
        {
            print(currentAxis + "axis");
            switch (currentAxis)
            {
                case "x":
                    zoomObject.transform.Rotate(new Vector3(angleChangeX, 0, 0) * rotationForce);
                    break;
                case "y":
                    zoomObject.transform.Rotate(new Vector3(0, angleChangeY, 0) * rotationForce);
                    break;
                case "z":
                    zoomObject.transform.Rotate(new Vector3(0, 0, angleChangeZ) * rotationForce);
                    break;
            }
        }
        else
        {
            zoomObject.transform.localRotation = initialRotation;
        }
        
        //Resetting previous Vectors
        previousRotationVectorX = currentRotationVectorX;
        previousRotationVectorY = currentRotationVectorY;
        previousRotationVectorZ = currentRotationVectorZ;
        previousAxis = currentAxis;
    }
}
