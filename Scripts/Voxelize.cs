using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxelize : MonoBehaviour
{
    public TerrainGen gen;

    public bool debugVoxels;

    public int resolutionX;
    public int resolutionY;
    public int resolutionZ;

    public GameObject cube;

    public int[,,] grid;

    public int[,,] voxelizeTerrain()
    {
        gen.GenerateArr();

        grid = new int[resolutionX, resolutionY, resolutionZ];

        for(int i = 0; i < resolutionX; i ++)
        {
            for(int k = 0; k < resolutionY; k ++)
            {
                float height = gen.GetHeight((int) ((gen.sizeX + 0f) / resolutionX * i),
                (int) ((gen.sizeY + 0f) / resolutionY * k));

                for(int j = 0; j < resolutionZ; j ++)
                {
                    // && height <= 320f / resolutionZ * (j + 3)

                    if(height >= 320f / resolutionZ * j && height <= 320f / resolutionZ * (j + 3))
                    {
                        grid[i,k,j] = 1;

                        if(height * 0.45 + -108.5317 <= -104.8)
                        {
                            grid[i,k,j] = 3;
                        }

                        if(debugVoxels)
                        {
                            GameObject copy = Instantiate(cube);

                            copy.transform.position = new Vector3(400f / resolutionX * k + transform.position.x,
                            140f / resolutionZ * j + transform.position.y, 400f/ resolutionY * i + transform.position.z);
                        }
                    }
                    else
                    {
                        grid[i,k,j] = 0;
                    }
                }   
            }
        }

        return grid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
