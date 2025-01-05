using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For handling UI Image and color

public class Ally : MonoBehaviour
{
    [SerializeField] private GameObject imagePrefab; // The image to show above the ally (default)
    [SerializeField] private GameObject imagePrefabAlt; // The image to show when the player already has the item
    private bool isPlayerNearby = false; // To check if the player is near
    private bool isBubbleGone = true; // Bool to check if the bubble is gone
    [SerializeField] public string itemNeeded = "DefaultItem";

    [SerializeField] private string itemToGive = "NewItem"; // The name of the item the ally will give
    [SerializeField] private Sprite itemSprite; // The sprite for the new item
    [SerializeField] private string slotName = "Slot0"; // The name of the UI slot to put the new item into
    [SerializeField] private Image playerUIImage; // The UI image the player will receive the new item sprite on

    private void Update()
    {
        // Only check for input if the player is nearby and bubble is gone
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isBubbleGone)
        {
            ExchangeItem();
        }
    }

    private void DisplayImage()
    {
        // Instantiate the correct image prefab
        if (PlayerInventory.Instance.HasItem(itemToGive))
        {
            // Player already has the item
            if (imagePrefabAlt != null)
            {
                GameObject image = Instantiate(imagePrefabAlt, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                StartCoroutine(FloatBubble(image));
                Destroy(image, 5f);
            }
        }
        else
        {
            // Player does not have the item
            if (imagePrefab != null)
            {
                GameObject image = Instantiate(imagePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                StartCoroutine(FloatBubble(image));
                Destroy(image, 5f);
            }
        }

        isBubbleGone = false;
        StartCoroutine(BubbleCooldown());
    }

    private IEnumerator FloatBubble(GameObject image)
    {
        Vector3 initialPosition = image.transform.position;
        float time = 0;
        while (time < 4f) // Float for 4 seconds
        {
            time += Time.deltaTime;
            image.transform.position = initialPosition + new Vector3(0, Mathf.Sin(time * Mathf.PI * 2f) * 0.1f, 0); // Sine wave
            yield return null;
        }
    }

    private IEnumerator BubbleCooldown()
    {
        yield return new WaitForSeconds(5f);
        isBubbleGone = true; // Set the bubble state back to true after 5 seconds
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    // The function for exchanging the item with the ally
    private void ExchangeItem()
    {
        // Check if the player has the required item
        if (PlayerInventory.Instance.HasItem(itemNeeded))
        {
            PlayerInventory.Instance.RemoveItem(itemNeeded);
            PlayerInventory.Instance.AddItem(itemToGive);
            UpdateSlotImage(slotName, itemSprite);
        }
        else
        {
            Debug.Log("Player does not have the required item to exchange.");
        }

        DisplayImage();
    }

    private void UpdateSlotImage(string targetSlotName, Sprite newSprite)
    {
        // Find the slot GameObject with the name matching the slotName
        GameObject slotObject = GameObject.Find(targetSlotName);

        if (slotObject != null)
        {
            Image slotImage = slotObject.GetComponent<Image>();
            if (slotImage != null)
            {
                slotImage.sprite = newSprite;
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
}
