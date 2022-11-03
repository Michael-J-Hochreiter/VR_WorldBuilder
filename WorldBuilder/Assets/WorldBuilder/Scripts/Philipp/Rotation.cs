using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private  GameObject zoomObject;
    private  GameObject leftHand;
    private  GameObject rightHand;

    private StateMachine stateMachine;

    private bool initialize = true;

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHand = GameObject.FindWithTag("LeftController");
        rightHand = GameObject.FindWithTag("RightController");
        
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
        throw new NotImplementedException();
    }
}
