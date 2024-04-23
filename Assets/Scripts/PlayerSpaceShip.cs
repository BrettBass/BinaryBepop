using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceShip : MonoBehaviour
{
    //Player Spaceship rigidbody
    Rigidbody rb;

    //Input Values
    float verticalMove;
    float horizontalMove;
    float mouseInputX;
    float mouseInputY;
    float rollAxis;

    //Speed and acceleration multipliers
    [SerializeField]
    float speedMult = 1.0f;
    [SerializeField]
    float speedMultAngle = 0.5f;
    [SerializeField]
    float speedRollMultAngle = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalMove = Input.GetAxis("Vertical");
        Debug.Log("Hello?");
        horizontalMove = Input.GetAxis("Horizontal");
        rollAxis = Input.GetAxis("Roll");

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");
    }
    void FixedUpdate()
    {
        rb.AddForce(rb.transform.TransformDirection(Vector3.forward) * verticalMove * speedMult, ForceMode.VelocityChange);
        
        rb.AddForce(rb.transform.TransformDirection(Vector3.right) * horizontalMove * speedMult, ForceMode.VelocityChange);
        rb.AddTorque(rb.transform.right * speedMultAngle * mouseInputY * -1, ForceMode.VelocityChange);
        rb.AddTorque(rb.transform.up * speedMultAngle * mouseInputX, ForceMode.VelocityChange);
        rb.AddTorque(rb.transform.forward * speedRollMultAngle * rollAxis, ForceMode.VelocityChange);
    }
}
