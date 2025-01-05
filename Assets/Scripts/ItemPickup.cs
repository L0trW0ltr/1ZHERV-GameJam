using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "DefaultItem"; // Name or ID of the item
    [SerializeField] private Sprite newItemSprite; // The sprite to set in the UI upon pickup
    [SerializeField] private string slotName = "DefaultSlot"; // The name of the UI slot to update
    [SerializeField] private Color pickupColor = Color.green; // The color to set in the slot
    [SerializeField] private Color defaultColor = Color.white; // Default color for clearing the slot

    private bool playerNearby = false; // Tracks if the player is nearby

    private void Update()
    {
        // Check for the E key press when the player is nearby
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }

        // If the player died or entered a scene he was before where he recently took an item, and now has it in his inventory
        if(((PlayerInventory.Instance.HasItem("Pepsi") || PlayerInventory.Instance.HasItem("Black_key")) && itemName == "Pepsi") || 
           ((PlayerInventory.Instance.HasItem("Beer") || PlayerInventory.Instance.HasItem("Yellow_key")) && itemName == "Beer") ||
           ((PlayerInventory.Instance.HasItem("Coke") || PlayerInventory.Instance.HasItem("silver_key")) && itemName == "Coke")){
            Destroy(gameObject);
        }
    }

    private void PickupItem()
    {
        // Update the specified slot's sprite and color
        UpdateSlotImage(slotName, newItemSprite, pickupColor);
        PlayerInventory.Instance.AddItem(itemName);

        // Destroy the item after pickup
        Destroy(gameObject);
    }

    private void UpdateSlotImage(string targetSlotName, Sprite newSprite, Color color)
    {
        GameObject slotObject = GameObject.Find(targetSlotName);

        if (slotObject != null)
        {
            Image slotImage = slotObject.GetComponent<Image>();

            if (slotImage != null)
            {
                slotImage.sprite = newSprite;
                slotImage.color = color;
            }
            else
            {
                Debug.LogError($"Slot '{targetSlotName}' does not have an Image component!");
            }
        }
        else
        {
            Debug.LogError($"No slot found with the name '{targetSlotName}'!");
        }
    }

    // Clears the specified slot by resetting its sprite and color
    public static void ClearSlot(string targetSlotName, Sprite defaultSprite, Color defaultColor)
    {
        GameObject slotObject = GameObject.Find(targetSlotName);

        if (slotObject != null)
        {
            Image slotImage = slotObject.GetComponent<Image>();

            if (slotImage != null)
            {
                slotImage.sprite = defaultSprite;
                slotImage.color = defaultColor;
                Debug.Log($"Slot '{targetSlotName}' cleared.");
            }
            else
            {
                Debug.LogError($"Slot '{targetSlotName}' does not have an Image component!");
            }
        }
        else
        {
            Debug.LogError($"No slot found with the name '{targetSlotName}'!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
