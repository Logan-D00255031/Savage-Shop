using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    [Required]
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    [ReadOnly]
    [SerializeField]
    private float x;
    [ReadOnly]
    [SerializeField] 
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset velocity if on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        x = Input.GetAxis("Horizontal");    // store horizontal input
        z = Input.GetAxis("Vertical");      // store vertical input

        Vector3 move = controller.transform.right * x + controller.transform.forward * z; // store x and z vector movement

        controller.Move(move * speed * Time.deltaTime); // Move the player by move vector

        // Calculate gravity
        velocity.y += gravity * Time.deltaTime; 

        controller.Move(velocity * Time.deltaTime);
    }
}
