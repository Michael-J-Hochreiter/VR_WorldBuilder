// AUTHOR: MICHAEL HOCHREITER

using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    public GameObject selectionUIPrefab;
    [HideInInspector] public GameObject selectionUI;

    public void EnableSelectionUI()
    {
        selectionUI = Instantiate(selectionUIPrefab);
        selectionUI.GetComponent<SelectionUI>().buildingBlock = transform;
    }

    public void DisableSelectionUI()
    {
        Destroy(selectionUI);
    }

}
