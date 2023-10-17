using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveContainer : MonoBehaviour
{
    public GameObject player;

    public GameObject sound;
    public GameObject soundb;

    public int[,,] states;

    public float damping = 0.95f;

    public float[,,] current;
    public float[,,] previous;

    public int sizeX, sizeY, sizeZ;

    public int x, y, z;
    public int x2, y2, z2;

    public float vol;
    public float volb;

    private int frame = 0;

    private bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<VoxelizeScene>().InitScene();

        this.states = this.gameObject.GetComponent<VoxelizeScene>().states;

        sizeX = this.states.GetLength(0);
        sizeY = this.states.GetLength(1);
        sizeZ = this.states.GetLength(2);

        current = new float[sizeX, sizeY, sizeZ];
        previous = new float[sizeX, sizeY, sizeZ];

        x = (int) ((sound.transform.position.x + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.x);
        y = (int) (sound.transform.position.y / this.gameObject.GetComponent<VoxelizeScene>().resolution.z);
        z = (int) ((sound.transform.position.z + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.y);

        previous[x,z,y] = 5;

        // for(int i = 0; i < 30; i ++) {
            // Iteration();
        // }

        // this.gameObject.GetComponent<VoxelizeScene>().Visualize();
    }

    public void Iteration() {
        for(int l = 0; l < 5; l ++)
        {
            for(int i = 1; i < sizeX - 1; i ++)
            {
                for(int j = 1; j < sizeY - 1; j ++)
                {
                    for(int k = 1; k < sizeZ - 1; k ++)
                    {
                        if(states[i,j,k] == 0)
                        {
                            current[i,j,k] = ((
                                previous[i-1,j,k] +
                                previous[i+1,j,k] +
                                previous[i,j-1,k] +
                                previous[i,j+1,k] +
                                previous[i,j,k-1] +
                                previous[i,j,k+1]
                            ) / 3f - current[i,j,k]) * damping;
                        }
                    }
                }
            }

            for(int i = 1; i < sizeX - 1; i ++)
            {
                for(int j = 1; j < sizeY - 1; j ++)
                {
                    for(int k = 1; k < sizeZ - 1; k ++)
                    {
                        float temp = current[i,j,k];
                        current[i,j,k] = previous[i,j,k];
                        previous[i,j,k] = temp;

                        vol += current[x,z,y];
                        volb += current[x2,z2,y2];
                    }
                }
            }
        }

        vol /= 5;
        volb /= 5;
    }

    // Update is called once per frame
    void Update()
    {
        frame ++;

        if(Input.GetKeyDown("o") || Input.GetMouseButtonDown(0))
        {
            x = (int) ((sound.transform.position.x + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.x);
            y = (int) (sound.transform.position.y / this.gameObject.GetComponent<VoxelizeScene>().resolution.z);
            z = (int) ((sound.transform.position.z + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.y);

            previous[x,z,y] = 5;

            sound.GetComponent<AudioSource>().Play();
            soundb.GetComponent<AudioSource>().Play();

            played = false;

            this.frame = 1;
        }

        x = (int) ((player.transform.position.x + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.x);
        y = (int) (player.transform.position.y / this.gameObject.GetComponent<VoxelizeScene>().resolution.z);
        z = (int) ((player.transform.position.z + 5) / this.gameObject.GetComponent<VoxelizeScene>().resolution.y);

        x2 = (int) (x + player.transform.right.x);
        y2 = (int) (y + player.transform.right.y);
        z2 = (int) (z + player.transform.right.z);

        x = (int) (x - player.transform.right.x);
        y = (int) (y - player.transform.right.y);
        z = (int) (z - player.transform.right.z);

        Iteration();

        vol /= 1500;
        volb /= 1500;

        vol -= 0.3f;
        volb -= 0.3f;

        //Debug.Log(x + " " + y + " " + z);
        ///Debug.Log(x2 + " " + y2 + " " + z2);
        
        if((vol > 0 || volb > 0) && !played)
        {
            played = true;
            sound.GetComponent<AudioSource>().Stop();
            soundb.GetComponent<AudioSource>().Stop();

            sound.GetComponent<AudioSource>().Play();
            soundb.GetComponent<AudioSource>().Play();
        }

        sound.GetComponent<AudioSource>().volume = vol;
        soundb.GetComponent<AudioSource>().volume = volb;
        // this.gameObject.GetComponent<VoxelizeScene>().Visualize();
    }
}
