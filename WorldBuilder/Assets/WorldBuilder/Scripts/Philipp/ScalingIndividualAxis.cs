//@Author Philipp Thayer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingIndividualAxis : MonoBehaviour
{
    private  GameObject zoomObject;
    private  GameObject leftHand;
    private  GameObject rightHand;

    private StateMachine stateMachine;

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
        if (stateMachine.state == StateMachine.State.EditingScaleIndividualAxis)
        {
            //Handle scaling on individual axis
        }
    }
}
