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

    private StateMachine stateMachine;
    private Transform lHand;
    private Transform rHand;
    private bool doRaycast = false;
    private String currentSelectionHand = "";

    public List<GameObject> selectedBuildingBlocks = new List<GameObject>();
    private SelectionUI LatestSelectedBlockUI;
    private BuildingBlock LatestSelectedBlock;
    private OutlineManager outlineManager;
    private SpawnBlocks spawnBlocks;

    private bool lTriggerPressed = false;
    private bool rTriggerPressed = false;
    private bool shifting = false;

    private AudioSource selectionSound;

    private void Awake()
    {
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateMachine>();
        lHand = GameObject.FindWithTag("LeftController").transform;
        rHand = GameObject.FindWithTag("RightController").transform;
        outlineManager = gameObject.GetComponent<OutlineManager>();
        modificiationParent = GameObject.FindWithTag("ModificationParent").transform;
        spawnBlocks = stateMachine.GetComponent<SpawnBlocks>();
        selectionSound = GetComponent<AudioSource>();
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
            selectedBuildingBlocks.Clear();
            if (stateMachine.state == StateMachine.State.Idle)
            {
                RemoveOutline();
                spawnBlocks.MoveObjectsToStaticBlockParent();
            }
            outlineManager.UpdateOutlines();
        }

        TriggerUp("left");
    }

    void RTriggerUp(InputAction.CallbackContext trigger)
    {
        rTriggerPressed = false;
        if (!rightHanded)
        {
            shifting = false;
            selectedBuildingBlocks.Clear();
            if (stateMachine.state == StateMachine.State.Idle)
            {
                RemoveOutline();
                spawnBlocks.MoveObjectsToStaticBlockParent();
                outlineManager.UpdateOutlines();
            }
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
                        RemoveOutline();
                        selectedBuildingBlocks.Clear();
                    }
                    selectionSound.Play();

                    var latestHit = hit.transform.gameObject;
                    var outline = latestHit.GetComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = Color.white;


                    selectedBuildingBlocks.Add(latestHit);

                    foreach (var block in selectedBuildingBlocks)
                    {
                        block.GetComponent<BuildingBlock>().DisableSelectionUI();
                    }

                    latestHit.GetComponent<BuildingBlock>().EnableSelectionUI();

                    LatestSelectedBlock = latestHit.GetComponent<BuildingBlock>();
                    LatestSelectedBlockUI = LatestSelectedBlock.selectionUI.GetComponent<SelectionUI>();
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
                        ParentBlocks();
                        DisableUI();
                        stateMachine.state = StateMachine.State.EditingRotation;
                        outlineManager.SetOutlineColor("rotate");
                        outlineManager.UpdateOutlines();
                        selectionSound.Play();
                        break;
                    case "translate":
                        ParentBlocks();
                        DisableUI();
                        stateMachine.state = StateMachine.State.EditingTranslation;
                        outlineManager.SetOutlineColor("translate");
                        outlineManager.UpdateOutlines();
                        selectionSound.Play();
                        break;
                    case "scaleAll":
                        ParentBlocks();
                        DisableUI();
                        stateMachine.state = StateMachine.State.EditingScaleAllAxis;
                        outlineManager.SetOutlineColor("scaleAll");
                        outlineManager.UpdateOutlines();
                        selectionSound.Play();
                        break;
                    case "scaleIndividual":
                        ParentBlocks();
                        DisableUI();
                        stateMachine.state = StateMachine.State.EditingScaleIndividualAxis;
                        outlineManager.SetOutlineColor("scaleIndividual");
                        outlineManager.UpdateOutlines();
                        selectionSound.Play();
                        break;
                    default:
                        if (shifting)
                        {
                            if (LatestSelectedBlock != null)
                            {
                                LatestSelectedBlock.DisableSelectionUI();
                            }
                        }
                        else
                        {
                            DisableUI();
                            selectedBuildingBlocks.Clear();
                        }

                        break;
                }
            }
            else
            {
                if (shifting)
                {
                    if (LatestSelectedBlock != null)
                    {
                        LatestSelectedBlock.DisableSelectionUI();
                    }
                }
                else
                {
                    DisableUI();
                    selectedBuildingBlocks.Clear();
                }
            }
        }

        if (!shifting)
        {
            outlineManager.UpdateOutlines();
        }
    }

    private void DisableUI()
    {
        if (LatestSelectedBlock != null)
        {
            LatestSelectedBlock.DisableSelectionUI();
        }

        RemoveOutline();
    }

    private void ParentBlocks()
    {
        spawnBlocks.MoveObjectsToStaticBlockParent();
        
        foreach (var block in selectedBuildingBlocks)
        {
            block.transform.parent = modificiationParent;
        }

        selectedBuildingBlocks.Clear();
    }

    private void RemoveOutline()
    {
        foreach (var block in selectedBuildingBlocks)
        {
            block.GetComponent<Outline>().enabled = false;
        }
    }

    private void Update()
    {
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