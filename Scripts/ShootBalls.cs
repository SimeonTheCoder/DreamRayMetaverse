using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBalls : MonoBehaviour
{
    public GameObject sphere;
    public GameObject saveContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            GameObject copy =  Instantiate(sphere);
            copy.transform.position = transform.position + transform.forward * 5;

            copy.transform.SetParent(saveContainer.transform);

            //copy.GetComponent<Rigidbody>().AddForce(transform.forward * 3000);

            copy.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f) * 100000f);
        }
    }
}
