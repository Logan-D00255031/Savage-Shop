using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    [Required]
    public CharacterController controller;

    public float speed = 10f;

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
        x = Input.GetAxis("Horizontal");    // store horizontal input
        z = Input.GetAxis("Vertical");      // store vertical input

        Vector3 move = controller.transform.right * x + controller.transform.forward * z; // store x and z vector movement

        controller.Move(move * speed * Time.deltaTime); // Move the player by move vector
    }
}
