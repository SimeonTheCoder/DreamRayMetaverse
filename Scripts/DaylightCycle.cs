using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DaylightCycle : MonoBehaviour
{
    public Volume volume;
    public GameObject lightGO;

    public float rotationSpeed;
    public bool control;

    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!control)
        {
            transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            if(Input.GetKey("]"))
            {
                transform.rotation *= Quaternion.Euler(rotationSpeed / 50f * 20f, 0, 0);
            }
            else if(Input.GetKey("["))
            {
                transform.rotation *= Quaternion.Euler(-rotationSpeed / 50f * 20f, 0, 0);
            }

            if(Input.GetKey("p"))
            {
                paused = !paused;
            }

            if(paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        Fog fog;
        volume.profile.TryGet(out fog);

        fog.meanFreePath.value = Mathf.Lerp(8, 800, Mathf.Abs(transform.eulerAngles.x - 270f) / 90f);
    }
}
