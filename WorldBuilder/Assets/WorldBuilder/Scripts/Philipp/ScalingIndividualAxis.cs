//@Author Philipp Thayer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScalingIndividualAxis : MonoBehaviour
{
    private GameObject zoomObject;
    private GameObject leftHand;
    private GameObject rightHand;

    private Vector3 rightHandPos;
    private Vector3 leftHandPos;
    private Vector3 initialScale;

    private StateMachine stateMachine;

    private float xPreviousDistance;
    private float yPreviousDistance;
    private float zPreviousDistance;
    private float xCurrentDistance;
    private float yCurrentDistance;
    private float zCurrentDistance;
    private float xDistanceChange;
    private float yDistanceChange;
    private float zDistanceChange;

    private String currentAxis = "";
    private String previousAxis = "";
    
    private Vector3 scaleChange;
    private float xTotalScaleChange = 0;
    private float yTotalScaleChange = 0;
    private float zTotalScaleChange = 0;
    [SerializeField] private float maxScale = 20;

    private bool initialize = true;
    private bool initializeAxis = true;
    
    private List<Vector3> objectScales = new List<Vector3>();


    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHand = GameObject.FindWithTag("LeftController");
        rightHand = GameObject.FindWithTag("RightController");

        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();

        rightHandPos = rightHand.transform.position;
        leftHandPos = leftHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle && stateMachine.leftGrabPressed && stateMachine.rightGrabPressed && zoomObject.transform.childCount > 0)
        {
            rightHandPos = rightHand.transform.position;
            leftHandPos = leftHand.transform.position;
            //Handle scaling on all axis
            if (initialize)
            {
                initialization();
            }
            updateVectors();
            determineAxis();
            applyScale();
        }
        else if (initialize == false)
        {
            //Happens every time the user releases the grab buttons
            initialize = true;
            initializeAxis = true;
            resetParentScale();
        }
    }

    private void updateVectors()
    {
        //setting current distance, needed for calculation
        xCurrentDistance = Vector3.Distance(rightHandPos,
            new Vector3(leftHandPos.x, rightHandPos.y, rightHandPos.z));
        yCurrentDistance = Vector3.Distance(rightHandPos,
            new Vector3(rightHandPos.x, leftHandPos.y, rightHandPos.z));
        zCurrentDistance = Vector3.Distance(rightHandPos,
            new Vector3(rightHandPos.x, rightHandPos.y, leftHandPos.z));
        
        //calculating change to initial vectors and total value changed
        xDistanceChange = xCurrentDistance - xPreviousDistance;
        yDistanceChange = yCurrentDistance - yPreviousDistance;
        zDistanceChange = zCurrentDistance - zPreviousDistance;
        
        //calculating total scale change on each axis to later determine the axis the user wants to scale
        xTotalScaleChange += xDistanceChange;
        yTotalScaleChange += yDistanceChange;
        zTotalScaleChange += zTotalScaleChange;
    }

    private void initialization()
    {
        //Initializes all values every time an object starts being scaled
        xPreviousDistance = Vector3.Distance(rightHandPos,
            new Vector3(leftHandPos.x, rightHandPos.y, rightHandPos.z));
        yPreviousDistance = Vector3.Distance(rightHandPos,
            new Vector3(rightHandPos.x, leftHandPos.y, rightHandPos.z));
        zPreviousDistance = Vector3.Distance(rightHandPos,
            new Vector3(rightHandPos.x, rightHandPos.y, leftHandPos.z));
        initialScale = zoomObject.transform.localScale;
        xTotalScaleChange = 0;
        yTotalScaleChange = 0;
        zTotalScaleChange = 0;
        initialize = false;
    }

    private void determineAxis()
    {
        if (xTotalScaleChange >= yTotalScaleChange && xTotalScaleChange >= zTotalScaleChange)
        {
            currentAxis = "x";
        }
        else if (yTotalScaleChange >= xTotalScaleChange && yTotalScaleChange >= zTotalScaleChange)
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

    private void applyScale()
    {
        if (currentAxis == previousAxis)
        {
            switch (currentAxis)
            {
                case "x":
                    scaleChange = new Vector3(xDistanceChange, 0, 0);

                    //setting new scale with calculated values if max/min scale isn't reached yet
                    if (xTotalScaleChange >= 1 / maxScale && xTotalScaleChange <= maxScale)
                    {
                        zoomObject.transform.localScale += scaleChange * 0.7f;
                    }
                    break;
                case "y":
                    scaleChange = new Vector3(0, yDistanceChange, 0);
                    //setting new scale with calculated values if max/min scale isn't reached yet
                    if (yTotalScaleChange >= 1 / maxScale && yTotalScaleChange <= maxScale)
                    {
                        zoomObject.transform.localScale += scaleChange * 0.7f;
                    }
                    break;
                case "z":
                    scaleChange = new Vector3(0, 0, zDistanceChange);
                    //setting new scale with calculated values if max/min scale isn't reached yet
                    if (zTotalScaleChange >= 1 / maxScale && zTotalScaleChange <= maxScale)
                    {
                        zoomObject.transform.localScale += scaleChange * 0.7f;
                    }
                    break;
            }
        }
        else
        {
            zoomObject.transform.localScale = initialScale;
        }

        //resetting Vector
        xPreviousDistance = xCurrentDistance;
        yPreviousDistance = yCurrentDistance;
        zPreviousDistance = zCurrentDistance;
    }

    private void resetParentScale()
    {
        foreach (Transform child in zoomObject.transform)
        {
            objectScales.Add(child.localScale);
        }

        zoomObject.transform.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < zoomObject.transform.childCount; i++)
        {
            zoomObject.transform.GetChild(i).transform.localScale = objectScales[i];
        }
    }
}