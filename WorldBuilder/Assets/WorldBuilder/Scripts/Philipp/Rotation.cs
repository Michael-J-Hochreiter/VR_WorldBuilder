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

    private void Awake()
    {
        zoomObject = GameObject.FindWithTag("ModificationParent");
        leftHand = GameObject.FindWithTag("LeftHand");
        rightHand = GameObject.FindWithTag("RightHand");
        
        stateMachine = GameObject.FindWithTag("RightHand").GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine.state == StateMachine.State.EditingRotation)
        {
            //Handle rotation
        }
    }
}
