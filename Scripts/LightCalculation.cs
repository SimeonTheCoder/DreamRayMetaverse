using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCalculation : MonoBehaviour
{
    public CellularAutomata automata;
    public GameObject terrain;

    public Vector3 sunDirection;

    public bool calculate;

    public int steps;
    public float stepSize;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(calculate)
        {
            Mesh mesh = terrain.GetComponent<MeshFilter>().mesh;

            Vector3[] verts = mesh.vertices;

            for(int i = 0; i < verts.Length; i ++)
            {
                Vector3 vert = verts[i];

                bool finished = true;

                for(int j = 0; j < steps; j ++)
                {
                    vert += sunDirection * stepSize;

                    int cellX = (int) vert.x;
                    int cellY = (int) vert.z;
                    int cellZ = (int) vert.y;

                    GameObject copy = Instantiate(cube);
                    copy.transform.position = vert;

                    if(cellX >= 0 && cellX < automata.grid.GetLength(0) &&
                        cellY >= 0 && cellY < automata.grid.GetLength(1) &&
                        cellZ >= 0 && cellZ < automata.grid.GetLength(2))
                    {
                        if(automata.grid[cellX,cellY,cellZ] != 0)
                        {
                            finished = false;

                            break;
                        }
                    }
                }

                if(!finished)
                {
                    terrain.GetComponent<MeshFilter>().mesh.uv[i] = new Vector2(0,0);
                }
                else
                {
                    terrain.GetComponent<MeshFilter>().mesh.uv[i] = new Vector2(1,0);
                }
            }

            Debug.Log("DONE!!!!!!!!!!");

            calculate = false;
        }
    }
}
