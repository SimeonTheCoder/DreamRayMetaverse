using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoordManager : MonoBehaviour
{
    public GameObject x;
    public GameObject y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Randomize()
    {
        x.GetComponent<TMP_InputField>().text = "" + Random.Range(0, 1000000);
        y.GetComponent<TMP_InputField>().text = "" + Random.Range(0, 1000000);
    }

    public void SetCoords()
    {
        PlayerPrefs.SetInt("XPlot", int.Parse(x.GetComponent<TMP_InputField>().text));
        PlayerPrefs.SetInt("YPlot", int.Parse(y.GetComponent<TMP_InputField>().text));
    }
}
