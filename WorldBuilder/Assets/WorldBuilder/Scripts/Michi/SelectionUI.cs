// AUTHOR: MICHAEL HOCHREITER

using System.Collections.Generic;
using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    public float distanceToPlayer = 2f;
    public float alphaSelectedSegment = 0.3f;
    public float alphaNotSelectedSegment = 1.0f;
    [HideInInspector] public Transform buildingBlock;

    private Transform player;
    private Transform UI;
    private List<CanvasGroup> UISegments;

    public enum Segment
    {
        Rotate,
        Translate,
        ScaleAll,
        ScaleIndividual
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        UI = transform.Find("UI");
        foreach (CanvasGroup UISegment in UI)
        {
            UISegments.Add(UISegment);           
        }
    }
    
    void Update()
    {
        // always rotate towards player
        transform.LookAt(player);
        
        // always set position between player and buildingBlock;
        transform.position = player.position + Vector3.Normalize(buildingBlock.position - player.position) * distanceToPlayer;
    }

    public void HighlightUISegment(Segment segment)
    {
        RemoveUIHighlight();
        
        switch (segment)
        {
            case Segment.Rotate:
                UI.Find("UISegment_rotate").GetComponent<CanvasGroup>().alpha = alphaSelectedSegment;
                break;
            case Segment.Translate:
                UI.Find("UISegment_translate").GetComponent<CanvasGroup>().alpha = alphaSelectedSegment;
                break;
            
            case Segment.ScaleAll:
                UI.Find("UISegment_scaleAll").GetComponent<CanvasGroup>().alpha = alphaSelectedSegment;
                break;
            case Segment.ScaleIndividual:
                UI.Find("UISegment_scaleIndividual").GetComponent<CanvasGroup>().alpha = alphaSelectedSegment;
                break;
        }
    }
    
    public void RemoveUIHighlight()
    {
        foreach (CanvasGroup UISegment in UISegments)
        {
            UISegment.alpha = alphaNotSelectedSegment;
        }
    }
}
