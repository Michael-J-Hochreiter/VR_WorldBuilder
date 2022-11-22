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
    private Transform camera;

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

    private bool initialize = true;
    private bool initializeAxis = true;

    private List<Vector3> objectScales = new List<Vector3>();
    [SerializeField] private float scaleFactor = 1.3f;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHand = GameObject.FindWithTag("LeftController");
        rightHand = GameObject.FindWithTag("RightController");
        camera = GameObject.FindWithTag("MainCamera").transform;


        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();

        rightHandPos = rightHand.transform.position;
        leftHandPos = leftHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.EditingScaleIndividualAxis && stateMachine.leftGrabPressed &&
            stateMachine.rightGrabPressed && zoomObject.transform.childCount > 0)
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
        zTotalScaleChange += zDistanceChange;
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
        //checks on which axis the user wants to scale the object
        if (Math.Abs(xTotalScaleChange) >= Math.Abs(yTotalScaleChange) &&
            Math.Abs(xTotalScaleChange) >= Math.Abs(zTotalScaleChange))
        {
            currentAxis = "x";
        }
        else if (Math.Abs(yTotalScaleChange) >= Math.Abs(xTotalScaleChange) &&
                 Math.Abs(yTotalScaleChange) >= Math.Abs(zTotalScaleChange))
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
            //scales along the zoomobject's axis 
            {
                switch (currentAxis)
                {
                    case "x":
                        scaleChange = new Vector3(xDistanceChange, 0, 0);
                        zoomObject.transform.localScale += scaleChange * scaleFactor;
                        break;
                    case "y":
                        scaleChange = new Vector3(0, yDistanceChange, 0);
                        zoomObject.transform.localScale += scaleChange * scaleFactor;
                        break;
                    case "z":
                        scaleChange = new Vector3(0, 0, zDistanceChange);
                        zoomObject.transform.localScale += scaleChange * scaleFactor;
                        break;
                }
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
        previousAxis = currentAxis;
    }
}