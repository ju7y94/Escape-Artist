using UnityEngine;

public class CameraControlNet : MonoBehaviour
{
    [SerializeField] private Transform target; // The object the camera will follow
    [SerializeField] private float height = 10f; // The height of the camera above the target
    [SerializeField] private float distance = 10f; // The distance of the camera from the target
    [SerializeField] private float smoothSpeed = 0.125f; // The speed at which the camera will follow the target

    private Vector3 offset; // The offset between the target and the camera

    private void Start()
    {
        // Calculate the offset between the target and the camera
        offset = new Vector3(0f, height, -distance);
        Camera.main.enabled = false;
    }

    private void LateUpdate()
    {
        // Move the camera to the target position with a smooth follow
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Keep the camera looking down at the target
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}