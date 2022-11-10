//@Author Philipp Thayer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAllAxis : MonoBehaviour
{
    private GameObject zoomObject;
    private GameObject leftHand;
    private GameObject rightHand;

    private StateMachine stateMachine;

    private float previousDistance;
    private float currentDistance;
    private float distanceChange;
    private Vector3 scaleChange;
    private float totalScale = 1;
    [SerializeField] private float maxScale = 20;

    private bool initialize = true;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHand = GameObject.FindWithTag("LeftController");
        rightHand = GameObject.FindWithTag("RightController");

        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.Idle && stateMachine.leftGrabPressed && stateMachine.rightGrabPressed && zoomObject.transform.childCount > 0)
        {
            //Handle scaling on all axis
            calculateScale();
        }
        else if (initialize == false)
        {
            initialize = true;
        }
    }

    private void calculateScale()
    {
        if (initialize)
        {
            //Initializes distance
            //previousDistance = distance(rightHand.transform.position, leftHand.transform.position);
            previousDistance = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            initialize = false;
        }
            
        //setting current distance, needed for calculation
        currentDistance = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            
        //calculating change to initial vectors and total value changed
        distanceChange = currentDistance - previousDistance;
        scaleChange = new Vector3(distanceChange, distanceChange, distanceChange);
        if ((totalScale >= 1 / maxScale && distanceChange < 0) || (totalScale <= maxScale && distanceChange > 0))
        {
            totalScale += distanceChange;
        }
            
        //setting new scale with calculated values if max/min scale isn't reached yet
        if (totalScale >= 1 / maxScale && totalScale <= maxScale)
        {
            zoomObject.transform.localScale += scaleChange * 0.7f;
        }
            
        //resetting Vector
        previousDistance = currentDistance;
    }
}