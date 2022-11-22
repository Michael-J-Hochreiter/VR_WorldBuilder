// AUTHOR: MICHAEL HOCHREITER

using System.Collections.Generic;
using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    public float distanceToPlayer = 3f;
    public float alphaSelectedSegment = 1.0f;
    public float alphaNotSelectedSegment = 0.3f;
    [HideInInspector] public Transform buildingBlock;

    private Transform player;
    private Transform UI;
    private List<CanvasGroup> UISegments = new List<CanvasGroup>();

    public enum Segment
    {
        Rotate,
        Translate,
        ScaleAll,
        ScaleIndividual
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("UITracker").transform;
        UI = transform.Find("UI");

        for (int i = 0; i < UI.transform.childCount; i++)
        {
            UISegments.Add(UI.GetChild(i).GetComponent<CanvasGroup>());
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
        RemoveUISegmentHighlight();

        switch (segment)
        {
            case Segment.Rotate:
                UI.Find("UISegment_rotate").GetComponent<CanvasGroup>().alpha = alphaSelectedSegment;
                print(UI.Find("UISegment_rotate").GetComponent<CanvasGroup>());
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
    
    public void RemoveUISegmentHighlight()
    {
        foreach (CanvasGroup UISegment in UISegments)
        {
            UISegment.alpha = alphaNotSelectedSegment;
        }
    }
}
