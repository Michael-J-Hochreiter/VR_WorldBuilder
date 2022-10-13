using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class HandGrabZoom : MonoBehaviour
{
    public InputActionProperty lGrabAction;
    public InputActionProperty rGrabAction;
    public InputActionProperty lReleaseAction;
    public InputActionProperty rReleaseAction;
    //private float previousDistance;
    private Vector3 previousMiddle;
    //private float currentDistance;
    private Vector3 currentMiddle;
    //private float distanceChange;
    //private Vector3 positionChange;
    private float angleChange;
    private Vector2 previousRotationVector;
    private Vector2 currentRotationVector;
    //private Vector3 totalTranslation = Vector3.zero;
    //private Vector3 scaleChange;
    //private float totalScale = 1;
    //[SerializeField] private float maxTranslation = 3;
    //[SerializeField] private float maxScale = 3;
    //[SerializeField] private Slider slider;
    //[SerializeField] private GameObject canvas;
    

    [SerializeField] private  GameObject zoomObject;
    [SerializeField] private  GameObject leftHand;
    [SerializeField] private  GameObject rightHand;
    
    private bool lGrabbed = false;
    private bool rGrabbed = false;
    private bool initialize = true;
    

    public void OnEnable()
    {
        if (lGrabAction.action != null) lGrabAction.action.Enable();
        if (lGrabAction.action != null) lGrabAction.action.performed += lGrab;
        if (rGrabAction.action != null) rGrabAction.action.Enable();
        if (rGrabAction.action != null) rGrabAction.action.performed += rGrab;
        if (rReleaseAction.action != null) rReleaseAction.action.Enable();
        if (rReleaseAction.action != null) rReleaseAction.action.performed += rRelease;
        if (lReleaseAction.action != null) lReleaseAction.action.Enable();
        if (lReleaseAction.action != null) lReleaseAction.action.performed += lRelease;
    }

    private void lGrab(InputAction.CallbackContext grab){
        lGrabbed = true;
    }
    private void rGrab(InputAction.CallbackContext grab){
        rGrabbed = true;
    }
        
    private void rRelease(InputAction.CallbackContext grab){
        rGrabbed = false;
    }
    
    private void lRelease(InputAction.CallbackContext grab){
        lGrabbed = false;
    }

    private float distance(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Sqrt((vec1.x - vec2.x) *
                   (vec1.x - vec2.x) + 
                   (vec1.y - vec2.y) * 
                   (vec1.y - vec2.y)
                   + (vec1.z - vec2.z) * 
                   (vec1.z - vec2.z));
    }

    private void calculate()
    {
        //Setting Vectors when first grabbing to later compare them to current values
        if (initialize)
        {
            previousRotationVector = new Vector2(rightHand.transform.position.x, rightHand.transform.position.z)  - new Vector2(leftHand.transform.position.x, leftHand.transform.position.z);
            /*
            previousDistance = distance(rightHand.transform.position, leftHand.transform.position);
            previousMiddle = new Vector3((leftHand.transform.position.x + rightHand.transform.position.x) / 2,
                (leftHand.transform.position.y + rightHand.transform.position.y) / 2,
                (leftHand.transform.position.z + rightHand.transform.position.z) / 2);
                */
            disableRigidbodies();
            //canvas.SetActive(true);
            initialize = false;
        }
        
        //setting current values needed for calculation
        /*
        currentDistance = distance(rightHand.transform.position, leftHand.transform.position);
        currentMiddle = new Vector3((leftHand.transform.position.x + rightHand.transform.position.x) / 2,
            (leftHand.transform.position.y + rightHand.transform.position.y) / 2,
            (leftHand.transform.position.z + rightHand.transform.position.z) / 2);
        */
        currentRotationVector = new Vector2(rightHand.transform.position.x, rightHand.transform.position.z)  - new Vector2(leftHand.transform.position.x, leftHand.transform.position.z);
        
        //calculating change to initial vectors and total value changed
        /*
        distanceChange = currentDistance - previousDistance;
        scaleChange = new Vector3(distanceChange, distanceChange, distanceChange);
        if ((totalScale >= 1/maxScale  && distanceChange < 0) || (totalScale <= maxScale && distanceChange > 0))
        {
            totalScale += distanceChange;
        }
        positionChange = new Vector3(currentMiddle.x - previousMiddle.x, currentMiddle.y - previousMiddle.y,
            currentMiddle.z - previousMiddle.z);
        if (((totalTranslation.x <= maxTranslation && positionChange.x > 0) || (totalTranslation.x >= -maxTranslation && positionChange.x < 0)) &&
            ((totalTranslation.z <= maxTranslation && positionChange.z > 0) || (totalTranslation.z >= -maxTranslation && positionChange.z < 0)))
        {
            totalTranslation += positionChange;
        }
        */
        angleChange = Vector2.SignedAngle(previousRotationVector, currentRotationVector);

        //setting new rotation, translation and scale with calculated values if max/min scale isn't reached yet
        /*
        if (totalScale >= 1/maxScale && totalScale <= maxScale)
        {
            zoomObject.transform.localScale += scaleChange * 0.7f;
        }
        if ((totalTranslation.x <= maxTranslation || totalTranslation.x >= -maxTranslation) &&
            (totalTranslation.z <= maxTranslation || totalTranslation.z >= -maxTranslation))
        {
            zoomObject.transform.position += positionChange * 5;
        }
        */
        zoomObject.transform.Rotate(new Vector3(0, -angleChange, 0) * 1.2f);
        
        //Resetting previous Vectors
        /*
        previousDistance = currentDistance;
        previousMiddle = currentMiddle;
        */
        previousRotationVector = currentRotationVector;

    }

    private void disableRigidbodies()
    {
        //Disables Rigidbodies of all children of zoomObject
        int children = zoomObject.transform.childCount;
        for (int i = 0; i < children; ++i){
            if(zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>() != null)
            {
                zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    
    private void enableRigidbodies()
    {
        //Enables Rigidbodies of all children of zoomObject
        int children = zoomObject.transform.childCount;
        for (int i = 0; i < children; ++i){
            if(zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>() != null)
            {
                zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().detectCollisions = true;
                zoomObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    /*
    private void changeSliderValue()
    {
        slider.value = totalScale;
    }
    */
    
    void Update()
    {
        if (lGrabbed && rGrabbed)
        {
            calculate();
            //changeSliderValue();
        }
        else if (initialize == false)
        {
            initialize = true;
            enableRigidbodies();
            //canvas.SetActive(false);
        }
    }
}
