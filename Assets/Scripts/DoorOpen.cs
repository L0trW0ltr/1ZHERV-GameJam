using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string requiredItem = "Key"; // The item needed to affect the door
    [SerializeField] private float newMass = 0.5f; // New mass if the player has the item

    private Rigidbody2D doorRigidbody;
    private bool massChanged = false;

    private void Start()
    {
        doorRigidbody = GetComponent<Rigidbody2D>();
        if (doorRigidbody == null)
        {
            Debug.LogError("No Rigidbody2D found on the door!");
        }
    }

    private void Update()
    {
        // Check the inventory and change the mass if the item is present
        if (!massChanged && PlayerInventory.Instance.HasItem(requiredItem))
        {
            ChangeDoorMass(newMass);
        }
    }

    private void ChangeDoorMass(float mass)
    {
        if (doorRigidbody != null)
        {
            doorRigidbody.mass = mass;
            massChanged = true;
        }
    }
}
