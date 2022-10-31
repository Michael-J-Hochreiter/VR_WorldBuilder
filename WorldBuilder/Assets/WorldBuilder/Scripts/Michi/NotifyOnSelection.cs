// AUTHOR: MICHAEL HOCHREITER

using UnityEngine;

public class NotifyOnSelection : MonoBehaviour
{
    public SelectionUI.Segment segment;
    public SelectionUI selectionUI;
    
    private void Start()
    {
        selectionUI = transform.root.GetComponent<SelectionUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Segment Highlighted");
        selectionUI.HighlightUISegment(segment);
    }

    private void OnTriggerExit(Collider other)
    {
        selectionUI.RemoveUIHighlight();
    }
}
