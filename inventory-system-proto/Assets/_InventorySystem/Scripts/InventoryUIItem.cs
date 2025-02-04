using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUIItem : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovering;
    public BackpackManager backpackManager;

    public string itemName, itemType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    void Update()
    {
        if (isHovering && Input.GetMouseButtonUp(0))
        {
            backpackManager.RemoveItemFromBackpack(itemName,itemType);
        }
    }
}