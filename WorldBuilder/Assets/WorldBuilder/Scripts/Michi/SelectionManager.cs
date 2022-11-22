// AUTHOR: MICHAEL HOCHREITER

using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public bool rightHanded = true;
    public InputActionProperty lTriggerDown;
    public InputActionProperty lTriggerUp;

    public InputActionProperty rTriggerDown;
    public InputActionProperty rTriggerUp;

    public Transform modificiationParent;

    public Material selectedMaterial;
    public Material notSelectedMaterial;

    private StateMachine stateMachine;
    private Transform lHand;
    private Transform rHand;
    private bool doRaycast = false;
    private String currentSelectionHand = "";

    public List<GameObject> selectedBuildingBlocks = new List<GameObject>();
    private SelectionUI LatestSelectedBlockUI;
    private OutlineManager outlineManager;

    private bool lTriggerPressed = false;
    private bool rTriggerPressed = false;
    private bool shifting = false;

    private void Awake()
    {
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
        lHand = GameObject.FindWithTag("LeftController").transform;
        rHand = GameObject.FindWithTag("RightController").transform;
        outlineManager = gameObject.GetComponent<OutlineManager>();
        modificiationParent = GameObject.FindWithTag("ModificationParent").transform;
    }

    public void OnEnable()
    {
        if (lTriggerDown.action != null) lTriggerDown.action.Enable();
        if (lTriggerDown.action != null) lTriggerDown.action.performed += LTriggerDown;
        if (lTriggerUp.action != null) lTriggerUp.action.Enable();
        if (lTriggerUp.action != null) lTriggerUp.action.performed += LTriggerUp;

        if (rTriggerDown.action != null) rTriggerDown.action.Enable();
        if (rTriggerDown.action != null) rTriggerDown.action.performed += RTriggerDown;
        if (rTriggerUp.action != null) rTriggerUp.action.Enable();
        if (rTriggerUp.action != null) rTriggerUp.action.performed += RTriggerUp;
    }

    void LTriggerDown(InputAction.CallbackContext trigger)
    {
        lTriggerPressed = true;
        if (rightHanded)
        {
            shifting = true;
        }


        TriggerDown("left");
    }

    void RTriggerDown(InputAction.CallbackContext trigger)
    {
        rTriggerPressed = true;
        if (!rightHanded)
        {
            shifting = true;
        }

        TriggerDown("right");
    }

    void LTriggerUp(InputAction.CallbackContext trigger)
    {
        lTriggerPressed = false;
        if (rightHanded)
        {
            shifting = false;
        }

        TriggerUp("left");
    }

    void RTriggerUp(InputAction.CallbackContext trigger)
    {
        rTriggerPressed = false;
        if (!rightHanded)
        {
            shifting = false;
        }

        TriggerUp("right");
    }

    private void TriggerDown(string hand)
    {
        if ((hand == "right" && rightHanded) || (hand == "left" && !rightHanded))
        {
            doRaycast = true;
            currentSelectionHand = hand;

            RaycastHit hit;
            Ray ray = new Ray(
                rightHanded ? rHand.position : lHand.position,
                rightHanded ? rHand.forward : lHand.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("BuildingBlock"))
                {
                    if (!shifting)
                    {
                        RemoveSelectionMaterial();
                        selectedBuildingBlocks.Clear();
                    }

                    var latestHit = hit.transform.gameObject;

                    selectedBuildingBlocks.Add(latestHit);
                    latestHit.GetComponent<MeshRenderer>().material = selectedMaterial;

                    foreach (var block in selectedBuildingBlocks)
                    {
                        block.GetComponent<BuildingBlock>().DisableSelectionUI();
                    }

                    latestHit.GetComponent<BuildingBlock>().EnableSelectionUI();
                    LatestSelectedBlockUI = latestHit.GetComponent<BuildingBlock>()
                        .selectionUI.GetComponent<SelectionUI>();
                }
            }
        }
    }

    private void TriggerUp(string hand)
    {
        if ((hand == "right" && rightHanded) || (hand == "left" && !rightHanded))
        {
            doRaycast = false;
            currentSelectionHand = "";

            RaycastHit hit;
            Ray ray = new Ray(
                hand == "left" ? lHand.position : rHand.position,
                hand == "left" ? lHand.forward : rHand.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                switch (hit.transform.tag)
                {
                    case "rotate":
                        ParentBlocksAndDisableUI();
                        stateMachine.state = StateMachine.State.EditingRotation;
                        outlineManager.SetOutlineColor("rotate");
                        break;
                    case "translate":
                        ParentBlocksAndDisableUI();
                        stateMachine.state = StateMachine.State.EditingTranslation;
                        outlineManager.SetOutlineColor("translate");
                        break;
                    case "scaleAll":
                        ParentBlocksAndDisableUI();
                        stateMachine.state = StateMachine.State.EditingScaleAllAxis;
                        outlineManager.SetOutlineColor("scaleAll");
                        break;
                    case "scaleIndividual":
                        ParentBlocksAndDisableUI();
                        stateMachine.state = StateMachine.State.EditingScaleIndividualAxis;
                        outlineManager.SetOutlineColor("scaleIndividual");
                        break;
                    default:
                        if (shifting)
                        {
                            LatestSelectedBlockUI.GetComponent<BuildingBlock>().DisableSelectionUI();
                            return;
                        }

                        ParentBlocksAndDisableUI();

                        break;
                }
            }
            else
            {
                if (shifting)
                {
                    LatestSelectedBlockUI.GetComponent<BuildingBlock>().DisableSelectionUI();
                    return;
                }

                ParentBlocksAndDisableUI();
            }
        }

        outlineManager.UpdateOutlines();
    }

    private void ParentBlocksAndDisableUI()
    {
        foreach (var block in selectedBuildingBlocks)
        {
            block.transform.parent = modificiationParent;
        }

        LatestSelectedBlockUI.GetComponent<BuildingBlock>().DisableSelectionUI();
        RemoveSelectionMaterial();
        selectedBuildingBlocks.Clear();
    }

    private void RemoveSelectionMaterial()
    {
        foreach (var block in selectedBuildingBlocks)
        {
            block.GetComponent<MeshRenderer>().material = notSelectedMaterial;
        }
    }

    private void Update()
    {
        print("shifting = " + shifting);
        
        if (doRaycast)
        {
            Ray ray = new Ray(
                currentSelectionHand == "left" ? lHand.position : rHand.position,
                currentSelectionHand == "left" ? lHand.forward : rHand.forward);

            RaycastHit hit;
            if (LatestSelectedBlockUI != null)
            {
                LatestSelectedBlockUI.RemoveUISegmentHighlight();
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    switch (hit.collider.tag)
                    {
                        case "translate":
                            LatestSelectedBlockUI.HighlightUISegment(SelectionUI.Segment.Translate);
                            break;
                        case "rotate":
                            LatestSelectedBlockUI.HighlightUISegment(SelectionUI.Segment.Rotate);
                            break;
                        case "scaleIndividual":
                            LatestSelectedBlockUI.HighlightUISegment(SelectionUI.Segment.ScaleIndividual);
                            break;
                        case "scaleAll":
                            LatestSelectedBlockUI.HighlightUISegment(SelectionUI.Segment.ScaleAll);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}