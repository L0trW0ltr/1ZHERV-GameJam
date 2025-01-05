using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int sceneBuildIndex;
    public string levelName;
    public Player player;

    private void OnTriggerEnter2D(Collider2D other) {
        print("Trigger Entered");

        // Check if the colliding object is the player
        if(other.GetComponent<Player>()) {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);

            // Find the player after the scene is loaded
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<Player>();

                // Set player's position based on the scene and level conditions
                if (sceneBuildIndex == 1 && levelName == "Level0")
                {
                    player.transform.position = new Vector3(-2f, -2f, 0f);
                }
                else if (sceneBuildIndex == 2 && levelName == "Level1")
                {
                    player.transform.position = new Vector3(1.2f, 14f, 0f);
                }
                else if (sceneBuildIndex == 1 && levelName == "Level2")
                {
                    player.transform.position = new Vector3(13f, 14f, 0f);
                }
                else if (sceneBuildIndex == 3 && levelName == "Level2")
                {
                    player.transform.position = new Vector3(1.2f, 14f, 0f);
                }
                else if (sceneBuildIndex == 2 && levelName == "Level3")
                {
                    player.transform.position = new Vector3(13f, 14f, 0f);
                }
                else if (sceneBuildIndex == 4 && levelName == "Level3")
                {
                    player.transform.position = new Vector3(1.2f, 14f, 0f);
                }
                else if (sceneBuildIndex == 3 && levelName == "Level4")
                {
                    player.transform.position = new Vector3(13f, 14f, 0f);
                }
                else if (sceneBuildIndex == 6 && levelName == "Level1")
                {
                    player.transform.position = new Vector3(11.42f, 6.05f, 0f);
                    PlayerInventory.Instance?.ClearInventory();

                    // Clear UI slots with default sprites
                    ItemPickup.ClearSlot("Slot0", player.defaultSprite, Color.white);
                    ItemPickup.ClearSlot("Slot1", player.defaultSprite, Color.white);
                    ItemPickup.ClearSlot("Slot2", player.defaultSprite, Color.white);
                }
                else if (sceneBuildIndex == 1 && levelName == "win")
                {
                    player.transform.position = new Vector3(-2f, -2f, 0f);
                }
                else if (sceneBuildIndex == 1 && levelName == "death")
                {
                    player.transform.position = new Vector3(-2f, -2f, 0f);
                }
            }
            else
            {
                //Debug.LogWarning("Player object not found after scene load!");
            }
        }
    }
}
