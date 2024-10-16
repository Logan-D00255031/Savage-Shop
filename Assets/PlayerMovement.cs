using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;

    [SerializeField]
    private float x;
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

        Vector3 move = transform.right * x + transform.forward * z; // store x and z vector movement

        controller.Move(move * speed * Time.deltaTime); // Move the player by move vector
    }
}
