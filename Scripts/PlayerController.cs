using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject self;

    public float walk_speed;
    public float run_speed;

    public GameObject flashlight;

    private bool cursor;
    private bool flying;
    private bool flash;

    float movement_speed;

    // Start is called before the first frame update
    void Start()
    {
        movement_speed = walk_speed;
        Cursor.lockState = CursorLockMode.Confined;
        cursor = false;

        flashlight.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("l"))
        {
            cursor = !cursor;

            if(cursor)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movement_speed = run_speed;
        }
        else
        {
            movement_speed = walk_speed;
        }

        if(Input.GetKey("w"))
        {
            if(!flying)
            {
                self.transform.position += new Vector3(transform.forward.x, transform.forward.y * 0.3f, transform.forward.z) * movement_speed * Time.deltaTime;
            }
            else
            {
                self.transform.position += new Vector3(transform.forward.x, transform.forward.y, transform.forward.z) * movement_speed * 5 * Time.deltaTime;
            }
        }

        if (Input.GetKey("s"))
        {
            if(!flying)
            {
                self.transform.position -= new Vector3(transform.forward.x, transform.forward.y * 0.3f, transform.forward.z) * movement_speed * Time.deltaTime;
            }
            else
            {
                self.transform.position -= new Vector3(transform.forward.x, transform.forward.y, transform.forward.z) * movement_speed * 5 * Time.deltaTime;
            }
        }

        if (Input.GetKey("a"))
        {
            self.transform.position -= transform.right * movement_speed * Time.deltaTime * (flying ? 5 : 1);
        }

        if (Input.GetKey("d"))
        {
            self.transform.position += transform.right * movement_speed * Time.deltaTime * (flying ? 5 : 1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            self.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * 300);
        }

        if (Input.GetKeyDown("`"))
        {
            flying = !flying;
            self.GetComponent<Rigidbody>().useGravity = !flying;
        }

        if (Input.GetKeyDown("f"))
        {
            flash = !flash;
            flashlight.SetActive(flash);
        }

        transform.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * 2, Input.GetAxis("Mouse X"), 0);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
