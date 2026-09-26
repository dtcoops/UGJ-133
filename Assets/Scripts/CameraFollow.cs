using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Framing")]
    public float distance = 8f;
    public float height = 5f;
    public float lookAtHeightOffset = 1f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.15f;

    [Header("Rotation")]
    public float cameraYaw = 0f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        UpdateCameraPosition();
        UpdateLookDirection();
    }

    Vector3 CalculateDesiredPosition()
    {
        Quaternion rotation = Quaternion.Euler(0f, cameraYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        return target.position + offset;
    }

    void UpdateCameraPosition()
    {
        Vector3 desiredPosition = CalculateDesiredPosition();
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionSmoothTime);
    }
    void UpdateLookDirection()
    {
        Vector3 lookTarget = target.position + Vector3.up * lookAtHeightOffset;
        transform.LookAt(lookTarget);
    }
}
