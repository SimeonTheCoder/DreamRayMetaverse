using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materialize : MonoBehaviour
{
    public Material material;
    public Material material2;

    public bool transform;
    public bool reset;

    public float strength;

    public ParticleSystem destruction;

    public int gridX;
    public int gridY;
    public int gridZ;

    public bool immune;

    public CellularAutomata automata;
    
    private bool done;
    private int time;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = material;
        material.SetFloat("_Factor", 1f);
        material.SetFloat("_Emission_strength", 100000);

        transform = true;
        done = false;

        time = 0;
    }

    void Update()
    {
        if(transform && !done)
        {
            material.SetFloat("_Factor", material.GetFloat("_Factor") - Time.deltaTime * 2.5f);
            //material.SetFloat("_Emission_strength", Mathf.Max(0, material.GetFloat("_Emission_strength") - Time.deltaTime * 150000f));
            material.SetFloat("_Emission_strength", Mathf.Max(0, material.GetFloat("_Emission_strength")));

            //if(material.GetFloat("_Emission_strength") == 0f)
            //{
            //    GetComponent<Renderer>().material = material2;
            //    transform = false;
            //    done = true;
            //}
            transform = false;
            done = true;
        }

        if(time % 100 == 0)
        {
            material.SetColor("_Color", new Vector4(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
        }

        time ++;

        if(!immune && GameObject.Find("WeatherController").GetComponent<WeatherController>().windStrength * (automata.grid[gridX, gridY, gridZ] == 2 ? 1 : 0.5f) > strength)
        {
            //Debug.Log("I'm dead lmao");
                
            //destruction.Play();
            this.gameObject.AddComponent<Rigidbody>();
            this.gameObject.GetComponent<Rigidbody>().AddForce(
                0, 15, GameObject.Find("WeatherController").GetComponent<WeatherController>().windStrength * (automata.grid[gridX, gridY, gridZ] == 2 ? 1 : 0.5f)
            );
        }
    }
}
