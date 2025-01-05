using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Camera cam;
    public Sprite defaultSprite;

    Vector2 movement;
    Vector2 mousePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (cam != null)
        {
            Camera[] existingCameras = FindObjectsOfType<Camera>();
            foreach (Camera existingCam in existingCameras)
            {
                if (existingCam != cam)
                {
                    Destroy(existingCam.gameObject);
                }
            }
            DontDestroyOnLoad(cam.gameObject);
        }
        else
        {
            Debug.LogError("Camera is not assigned to the Player script!");
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePos - rb.position;
        rb.rotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Died! Respawning...");

            // Clear the inventory
            PlayerInventory.Instance?.ClearInventory();

            // Clear UI slots with default sprites
            ItemPickup.ClearSlot("Slot0", defaultSprite, Color.white);
            ItemPickup.ClearSlot("Slot1", defaultSprite, Color.white);
            ItemPickup.ClearSlot("Slot2", defaultSprite, Color.white);

            // Load the "death" scene
            SceneManager.LoadScene("death");
            gameObject.transform.position = new Vector3(11.4f, 6f, 0f);
        }
    }
}
