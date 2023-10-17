using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateRenderer : MonoBehaviour
{
    public CellularAutomata automata;

    public GameObject cube;
    public Vector3 scales;

    private int updateRate;
    private int currFrame;

    public int displaySlice;

    // Start is called before the first frame update
    void Start()
    {
        this.updateRate = automata.updateRate;
        this.currFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        this.currFrame ++;

        if(this.currFrame % updateRate != 0) return;

        for(int i = 0; i < automata.grid.GetLength(0); i ++)
        {
            for(int j = 0; j < automata.grid.GetLength(1); j ++)
            {
                for(int k = 0; k < automata.grid.GetLength(2); k ++)
                {
                    if(automata.grid[i,j,k] == displaySlice)
                    {
                        GameObject copy = Instantiate(cube);

                        cube.transform.position = new Vector3(
                            j * scales.x + transform.position.x, k * scales.z + transform.position.y, i * scales.y + transform.position.z
                        );
                    }
                }
            }
        }
    }
}
