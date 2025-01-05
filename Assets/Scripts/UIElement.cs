using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    private Image uiImage;

    private void Awake()
    {
        // Get the Image component
        uiImage = GetComponent<Image>();
        if (uiImage == null)
        {
            Debug.LogError("No Image component found on the UI element!");
        }
    }

    // Change the color of the UI element
    public void ChangeColor(Color newColor)
    {
        if (uiImage != null)
        {
            uiImage.color = newColor;
        }
    }
}
