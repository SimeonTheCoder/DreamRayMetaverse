using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;

    public float friction;

    private float turningSpeed;
    private float brakesFactor;

    private float velocity;

    public bool control;

    public int vehicleId;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {            
        turningSpeed = 65f * Time.deltaTime * 5f;

        if ((Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) && control)
        {
            transform.rotation *= Quaternion.Euler(0, -turningSpeed, 0);
        }
        else if ((Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) && control)
        {
            transform.rotation *= Quaternion.Euler(0, turningSpeed, 0);
        }

        if ((Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) && velocity < maxSpeed && control)
        {
            Debug.Log("SPEEED!");

            GetComponent<Rigidbody>().AddForce(transform.forward * -acceleration * Time.deltaTime);

            velocity += acceleration * Time.deltaTime / 100f;
        }
        else if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && maxSpeed > 0 && control)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * acceleration / 2f * Time.deltaTime);

            velocity -= acceleration * Time.deltaTime / 100f;
        }

        velocity -= friction / 1000f;

        if (velocity < 0) velocity = 0;
        if (velocity > maxSpeed / 100f) velocity = maxSpeed / 100f;
    }

    public void LeftInput()
    {
        transform.rotation *= Quaternion.Euler(0, 0, turningSpeed);
    }

    public void RightInput()
    {
        transform.rotation *= Quaternion.Euler(0, 0, -turningSpeed);
    }

    public void UpInput()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * acceleration * Time.deltaTime);

        velocity += acceleration * Time.deltaTime / 100f;
    }

    public void DownInput()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * -acceleration * brakesFactor * Time.deltaTime);

        velocity -= acceleration * Time.deltaTime / 100f;
    }
}
