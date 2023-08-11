using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float moveSpeed;
    private Camera cam;

    [SerializeField] private float limiterLeft;
    [SerializeField] private float limiterRight;

    public float minOrthoSize = 0.5f;
    public float maxOrthoSize = 2.8f;

    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        CalculateXYAxisLimits(minOrthoSize);

        cam = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        CameraLimiter();
        CameraInput();
        CameraZoom();
    }
    // Update is called once per frame
    void Update()
    {
        // Get the current orthographic size of the camera.
        float currentOrthoSize = GetComponent<Camera>().orthographicSize;

        // Calculate the new X and Y-axis limits based on the current orthographic size.
        CalculateXYAxisLimits(currentOrthoSize);

        // Limit the camera's position to the calculated X and Y-axis limits.
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    private void CalculateXYAxisLimits(float orthoSize)
    {
        float orthoSizeFactor = (orthoSize - minOrthoSize) / (maxOrthoSize - minOrthoSize);

        minX = Mathf.Lerp(-5f, -1f, orthoSizeFactor);
        maxX = Mathf.Lerp(5f, 1f, orthoSizeFactor);

        minY = Mathf.Lerp(-2.8f, -0.3f, orthoSizeFactor);
        maxY = Mathf.Lerp(2.8f, 0.3f, orthoSizeFactor);
    }
    private void CameraInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 8f;
        }
        else
        {
            moveSpeed = 4f;
        }

        float h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(h, v, 0);
    }

    private void CameraZoom()
    {
        if (Input.GetKey(KeyCode.E))
        {
            cam.orthographicSize += 1f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cam.orthographicSize -= 1f * Time.deltaTime;
        }
    }

    private void CameraLimiter()
    {
        if (cam.orthographicSize >= 2.8f)
        {
            cam.orthographicSize = 2.8f;
        }
        if (cam.orthographicSize <= 0.5f)
        {
            cam.orthographicSize = 0.5f;
        }
    }
}
