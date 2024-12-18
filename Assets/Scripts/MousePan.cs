using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MousePan : MonoBehaviour
{
    [Required]
    public Transform player;
    [Required]
    public Transform playerCamera;

    [Range(0.0f, 1.0f)]
    public float sensitivity = 1.0f;

    [SerializeField]
    [Range(15f, 180f)]
    private float maxRotation = 90f;

    [ReadOnly]
    [SerializeField]
    private float mouseX;

    [ReadOnly]
    [SerializeField]
    private float mouseY;

    [ReadOnly]
    [SerializeField]
    float verticalRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks and hides cursor to the window
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * (sensitivity * 1000) * Time.deltaTime; // Get mouse X axis based on current sensitivity
        mouseY = Input.GetAxis("Mouse Y") * (sensitivity * 1000) * Time.deltaTime; // Get mouse Y axis based on current sensitivity

        verticalRotation -= mouseY; // Reduce the vertical rotation by mouseY
        verticalRotation = Mathf.Clamp(verticalRotation, maxRotation * -1, maxRotation); // Prevent the vertical rotation from being less/greater than the max rotation

        player.Rotate(Vector3.up * mouseX); // Rotates the player on the Y axis by mouseX
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f); // Rotates the camera on the Z axis based on verticalRotation
    }
}
