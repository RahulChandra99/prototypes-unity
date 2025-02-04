using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    [Header("Backpack UI Elements")]
    public GameObject backpackUIPanel; 
    public Transform weaponBackpackArea; 
    public Transform materialBackpackArea; 
    public Transform armorBackpackArea;  
    public GameObject backpackSlotPrefab; 
    

    [Header("Backpack Settings")]
    public Transform weaponPhysicalSlots;
    public Transform materialPhysicalSlots;
    public Transform armorPhysicalSlots; 
    private Dictionary<GameObject, Transform> backpackSlots = new Dictionary<GameObject, Transform>();

    private GameObject draggedItem;
    private Vector3 originalItemPosition;
    private Vector3 draggedItemOffset = new Vector3(0, 0, 2f);
    
    public ServerManager serverManager;

    private bool isHoveringOverBackpack = false;
    private float backpackHoldTime = 0f;
    private const float holdDuration = 0.3f; 
    private bool isHoldingItem;
    [SerializeField] private GameObject hoverGO;
    
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private Crosshair crosshair;
    
    public UnityEvent<string> onItemAdded;
    public UnityEvent<string> onItemRemoved;
    
    

    void Start()
    {
        backpackUIPanel.SetActive(false);
    }

    void Update()
    {
        DragAndDrop();
        BackpackHover();
    }

    private void DragAndDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Item item = hit.collider.GetComponent<Item>();
                
                if (item != null)
                {
                    draggedItem = hit.collider.gameObject;
                    originalItemPosition = item.itemOriginalPosition;

                    Vector3 cameraPosition = Camera.main.transform.position;
                    Vector3 cameraForward = Camera.main.transform.forward;
                    draggedItem.transform.position = cameraPosition + cameraForward * draggedItemOffset.z;

                    isHoldingItem = true;
                }
            }
        }

        if (draggedItem != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 2f; 
            draggedItem.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && draggedItem != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Backpack"))
                {
                    AddItemToBackpack(draggedItem);
                }
                else
                {
                    draggedItem.transform.position = originalItemPosition; 
                }
            }
            isHoldingItem = false;
            draggedItem = null;
        }
    }

    private void BackpackHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Backpack") && !isHoldingItem)
        {
            isHoveringOverBackpack = true;
            hoverGO.SetActive(true);
            
            if (Input.GetMouseButton(0))
            {
                backpackHoldTime += Time.deltaTime;
                if (backpackHoldTime >= holdDuration)
                {
                    ShowBackpackUI();
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                CloseBackPack();
                backpackHoldTime = 0f;
            }
        }
        else
        {
            hoverGO.SetActive(false);
            isHoveringOverBackpack = false;
            backpackHoldTime = 0f;
        }
    }

    private void ShowBackpackUI()
    {
        if (!backpackUIPanel.activeSelf)
        {
            backpackUIPanel.SetActive(true);
            
            if(crosshair != null)
                crosshair.crosshairGO.SetActive(false);

            // Pause the FPS controller and unlock the cursor
            firstPersonController.TogglePause(true);
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    private void CloseBackPack()
    {
        backpackUIPanel.SetActive(false);
        crosshair.crosshairGO.SetActive(true);

        firstPersonController.TogglePause(false);
        // Resume the FPS controller and lock the cursor
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    private void AddItemToBackpack(GameObject item)
    {
        if (backpackSlots.ContainsKey(item)) return;

        Item itemComponent = item.GetComponent<Item>();
        if (itemComponent == null) return;

        Transform targetSlots;
        Transform uiParent;
        
        itemComponent.itemNameTXT.gameObject.SetActive(false);

        switch (itemComponent.type)
        {
            case "Weapon":
                targetSlots = weaponPhysicalSlots;
                uiParent = weaponBackpackArea;
                break;
            case "Armor":
                targetSlots = armorPhysicalSlots;
                uiParent = armorBackpackArea;
                break;
            case "Material":
                targetSlots = materialPhysicalSlots;
                uiParent = materialBackpackArea;
                break;
            default:
                return;
        }
        
        foreach (Transform slot in targetSlots)
        {
            if (slot.childCount == 0)
            {
                AttachItemToSlot(item, slot); 
                CreateUISlot(itemComponent.itemName, uiParent,itemComponent.type);
                backpackSlots[item] = slot;
                
                Debug.Log("Adding item: " + itemComponent.itemName + " of type: " + itemComponent.type);
                
                // Trigger UnityEvent and send request
                onItemAdded?.Invoke(itemComponent.identifier);
                break;
            }
        }
    }

    private void AttachItemToSlot(GameObject item, Transform slot)
    {
        item.transform.SetParent(slot);
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.SetActive(true);
        
        StartCoroutine(MoveItem(item.transform, slot.position, 0.5f));
    }

    private IEnumerator MoveItem(Transform item, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = item.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            item.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        item.position = targetPosition;
        
        
    }

    private void CreateUISlot(string itemName, Transform uiParent, string itemType)
    {
        GameObject uiSlot = Instantiate(backpackSlotPrefab, uiParent);
        uiSlot.GetComponentInChildren<TextMeshProUGUI>().text = itemName;
        uiSlot.GetComponent<InventoryUIItem>().itemName = itemName;
        uiSlot.GetComponent<InventoryUIItem>().itemType = itemType;
        uiSlot.GetComponent<InventoryUIItem>().backpackManager = this;
    }

    
    public void RemoveItemFromBackpack(string itemName, string itemType)
    {
        foreach (var entry in backpackSlots)
        {
            GameObject item = entry.Key;
            Item itemComponent = item.GetComponent<Item>();

            if (itemComponent != null && itemComponent.itemName == itemName && itemComponent.type == itemType)
            {
                itemComponent.itemNameTXT.gameObject.SetActive(true);
                
                StartCoroutine(MoveItem(item.transform, itemComponent.itemOriginalPosition, 0.5f));
                StartCoroutine(RemoveAfterDelay(item, 0.5f, GetBackpackContentArea(itemType)));

                Debug.Log("Removing item: " + itemName + " of type: " + itemType);

                return; 
            }
        }
        
    }
    
    private Transform GetBackpackContentArea(string itemType)
    {
        switch (itemType)
        {
            case "Weapon":
                return weaponBackpackArea;
            case "Armor":
                return armorBackpackArea;
            case "Material":
                return materialBackpackArea;
            default:
                return null;
        }
    }

    private IEnumerator RemoveAfterDelay(GameObject item, float delay, Transform backpackContentArea)
    {
        yield return new WaitForSeconds(delay);

        item.transform.SetParent(null);
        item.GetComponent<Rigidbody>().isKinematic = false;
       
        
        backpackSlots.Remove(item);
        
        foreach (Transform uiSlot in backpackContentArea)
        {
            if (uiSlot.GetComponentInChildren<TextMeshProUGUI>().text == item.GetComponent<Item>().itemName)
            {
                Destroy(uiSlot.gameObject);
                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        CloseBackPack();

        // Trigger UnityEvent and send request
        onItemRemoved?.Invoke(item.GetComponent<Item>().identifier);
        
    }
}
