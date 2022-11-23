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
        if (stateMachine.state == StateMachine.State.EditingScaleAllAxis && stateMachine.leftGrabPressed && stateMachine.rightGrabPressed && zoomObject.transform.childCount > 0)
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
            previousDistance = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            initialize = false;
        }
            
        //setting current distance, needed for calculation
        currentDistance = Vector3.Distance(rightHand.transform.position, leftHand.transform.position);
            
        //calculating change to initial vectors and total value changed
        distanceChange = currentDistance - previousDistance;
        scaleChange = new Vector3(distanceChange, distanceChange, distanceChange);
        //setting new scale with calculated values 
        zoomObject.transform.localScale += scaleChange * 0.7f;
        storeScale(scaleChange * 0.7f);

        //resetting Vector
        previousDistance = currentDistance;
    }
    private void storeScale(Vector3 scale)
    {
        foreach (Transform child in zoomObject.transform)
        {
            child.gameObject.GetComponent<ObjectTransforms>().scale += scale;
        }
    }
}