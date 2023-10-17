using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelizeScene : MonoBehaviour
{
    public WaveContainer container;

    public Vector3 dimensions;
    public Vector3 resolution;

    public GameObject cube;

    public GameObject[,,] cubeMatrix;

    public int[,,] states;

    public void InitScene()
    {
        states = new int[(int) dimensions.x, (int) dimensions.y, (int) dimensions.z];

        // cubeMatrix = new GameObject[(int) dimensions.x, (int) dimensions.y, (int) dimensions.z];

        for(int i = 0; i < dimensions.x; i ++)
        {
            for(int j = 0; j < dimensions.y; j ++)
            {
                for(int k = 0; k < dimensions.z; k ++)
                {
                    Vector3 pos = new Vector3(i * resolution.x, k * resolution.z, j * resolution.y) + transform.position;

                    RaycastHit hit;

                    if(Physics.Raycast(pos, new Vector3(1,0,0), out hit, resolution.x) ||
                        Physics.Raycast(pos + new Vector3(resolution.x,0,0), new Vector3(-1,0,0), out hit, resolution.x) ||
                        Physics.Raycast(pos, new Vector3(0,1,0), out hit, resolution.y) ||
                        Physics.Raycast(pos + new Vector3(0,resolution.y,0), new Vector3(0,-1,0), out hit, resolution.y) ||
                        Physics.Raycast(pos, new Vector3(0,0,1), out hit, resolution.z) ||
                        Physics.Raycast(pos + new Vector3(0,0,resolution.z), new Vector3(0,0,-1), out hit, resolution.z)) {
                        if(hit.collider.name != "self")
                        {
                            states[i,j,k] = 1;
                        }
                    }
                }
            }
        }

        // Visualize();
    }

    public void Visualize()
    {
        for(int i = 0; i < dimensions.x; i ++)
        {
            for(int j = 0; j < dimensions.y; j ++)
            {
                for(int k = 0; k < dimensions.z; k ++)
                {
                    if(states[i,j,k] == 0) continue;

                    Vector3 pos = new Vector3(i * resolution.x, k * resolution.z, j * resolution.y) + transform.position;

                    GameObject copy = Instantiate(cube);
                    copy.transform.position = pos;
                    copy.transform.localScale = new Vector3(resolution.x, resolution.z, resolution.y);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Visualize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
