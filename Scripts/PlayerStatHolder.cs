using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHolder : MonoBehaviour
{
    public PlayerStats[] ships;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerStats GetStats(int shipId)
    {
        return ships[shipId];
    }
}
