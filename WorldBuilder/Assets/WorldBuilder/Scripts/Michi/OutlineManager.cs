// AUTHOR: MICHAEL HOCHREITER

using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public Transform modificationParent;
    public Transform staticBlocksParent;
    
    public void UpdateOutlines()
    {
        for (int i = 0; i < modificationParent.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Outline>().enabled = true;
        }
        
        for (int i = 0; i < staticBlocksParent.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Outline>().enabled = false;
        }
    }

    private void ApplyOutlineColor(Color c)
    {
        for (int i = 0; i < modificationParent.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Outline>().OutlineColor = c;
        }
    }

    public void SetOutlineColor(string mode)
    {
        Color c;
        switch (mode)
        {
            case "translate":
                c = new Color(187, 179, 216);
                break;
            case "rotate":
                c = new Color(183, 212, 51);
                break;
            case "scaleIndividual":
                c = new Color(77, 182, 163);
                break;
            case "scaleAll":
                c = new Color(77, 172, 188);
                break;
            default:
                c = Color.white;
                break;
        }
        ApplyOutlineColor(c);
    }
}
