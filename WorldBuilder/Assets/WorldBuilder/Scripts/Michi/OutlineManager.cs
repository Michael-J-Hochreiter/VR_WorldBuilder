// AUTHOR: MICHAEL HOCHREITER

using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public Transform modificationParent;
    public Transform staticBlocksParent;

    private Color currentOutlineColor;
    
    public void UpdateOutlines()
    {
        ApplyOutlineColor();
        for (int i = 0; i < modificationParent.childCount; i++)
        {
            modificationParent.GetChild(i).GetComponent<Outline>().enabled = true;
        }
        
        for (int i = 0; i < staticBlocksParent.childCount; i++)
        {
            staticBlocksParent.GetChild(i).GetComponent<Outline>().enabled = false;
        }
    }

    private void ApplyOutlineColor()
    {
        for (int i = 0; i < modificationParent.childCount; i++)
        {
            modificationParent.GetChild(i).GetComponent<Outline>().OutlineColor = currentOutlineColor;
        }
    }

    public void SetOutlineColor(string mode)
    {
        Color c;

        switch (mode)
        {
            case "translate":
                c = new Color(187/255f, 179/255f, 216/255f);
                break;
            case "rotate":
                c = new Color(183/255f, 212/255f, 51/255f);
                break;
            case "scaleIndividual":
                c = new Color(77/255f, 182/255f, 163/255f);
                break;
            case "scaleAll":
                c = new Color(77/255f, 172/255f, 188/255f);
                break;
            default:
                c = Color.white;
                break;
        }

        currentOutlineColor = c;
    }
}
