using UnityEngine;
using Cinemachine;

public class CameraTiltWithDutch : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;
    public Vector2 levelBoundsMin; // Bottom-left corner of the level
    public Vector2 levelBoundsMax; // Top-right corner of the level
    public float maxDutchAngle = 15f; // Maximum tilt angle

    private CinemachineComposer composer;

    private void Awake()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera is not assigned!");
            return;
        }

        var existingCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var camera in existingCameras)
        {
            if (camera != virtualCamera)
            {
                Destroy(camera.gameObject); // Destroy duplicates
            }
        }
        DontDestroyOnLoad(virtualCamera.gameObject);
    }

    void Update()
    {
        if (virtualCamera == null || player == null)
            return;

        // Get player's normalized position in the level
        float normalizedX = Mathf.InverseLerp(levelBoundsMin.x, levelBoundsMax.x, player.position.x);
        float normalizedY = Mathf.InverseLerp(levelBoundsMin.y, levelBoundsMax.y, player.position.y);

        // Calculate Dutch angle
        float dutchX = Mathf.Lerp(-maxDutchAngle, maxDutchAngle, normalizedX); // Horizontal tilt
        float dutchY = Mathf.Lerp(maxDutchAngle, -maxDutchAngle, normalizedY); // Vertical tilt

        // Average the angles for smoother tilt effect
        float finalDutch = (dutchX + dutchY) / 2;

        // Apply Dutch tilt to the camera
        virtualCamera.m_Lens.Dutch = finalDutch;
    }
}