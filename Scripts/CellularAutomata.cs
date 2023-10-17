using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    public int[,,] grid;

    public Voxelize vox;

    public int updateRate;
    private int currFrame;

    // Start is called before the first frame update
    void Start()
    {
        grid = vox.voxelizeTerrain();

        this.currFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.currFrame > 30) return;

        for(int l = 0; l < 30; l ++)
        {
            this.currFrame ++;
            
            for(int i = 0; i < grid.GetLength(0); i ++)
            {
                for(int j = 0; j < grid.GetLength(1); j ++)
                {
                    for(int k = 0; k < grid.GetLength(2); k ++)
                    {
                        // if(grid[i,j,k] == 2)
                        // {
                        //     if(i > 0 && grid[i-1,j,k] == 0)
                        //     {
                        //         //grid[i,j,k] = 0;
                        //         grid[i-1,j,k] = 2;
                        //     }
                        // }

                        if(grid[i,j,k] == currFrame && currFrame >= 3 && currFrame <= 30)
                        {
                            if(i > 0 && i < grid.GetLength(0) - 1 &&
                                j > 0 && j < grid.GetLength(1) - 1 &&
                                k > 0 && k < grid.GetLength(2) - 1)
                            {
                                if(grid[i-1,j,k] == 0)
                                {
                                    grid[i-1,j,k] = currFrame + 1;
                                }
                                if(grid[i+1,j,k] == 0)
                                {
                                    grid[i+1,j,k] = currFrame + 1;
                                }

                                if(grid[i,j-1,k] == 0)
                                {
                                    grid[i,j-1,k] = currFrame + 1;
                                }
                                if(grid[i,j+1,k] == 0)
                                {
                                    grid[i,j+1,k] = currFrame + 1;
                                }

                                if(grid[i,j,k-1] == 0)
                                {
                                    grid[i,j,k-1] = currFrame + 1;
                                }
                                if(grid[i,j,k+1] == 0)
                                {
                                    grid[i,j,k+1] = currFrame + 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
