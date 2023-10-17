using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class TerrainGen : MonoBehaviour
{
    public GlobalTerrain globalSettings;

    public bool update;
    public bool done;
    public bool gizmos;

    public int sizeX;
    public int sizeY;

    public int xOffset;
    public int yOffset;

    public float worldScale;
    public float noiseOctaves;

    public float[] amplitudes;
    public float[] scales;
    
    public float[] maskBiases;
    public float[] maskAmplitudes;
    public float[] maskScales;

    public int[] maskMapping;
    public int uvMask;

    private int[,] maskMappingsArr;

    public float steepnessBias;

    public bool scatterObjects;

    public GameObject[] objectsToScatter;
    public float[] probabilities;

    public bool[] invertedMasks;

    private System.Random random;

    Mesh mesh;

    Vector3[] verts;
    Vector2[] uvs;

    int[] tris;

    // Start is called before the first frame update
    void Start()
    {
        this.random = new System.Random(this.xOffset + this.yOffset);

        this.xOffset = PlayerPrefs.GetInt("XPlot");
        this.yOffset = PlayerPrefs.GetInt("YPlot");

        if(!done)
        {
            done = false;

            mesh = new Mesh();

            GetComponent<MeshFilter>().mesh = mesh;

            GenerateArr();

            GenerateTerrain();
            MeshUpdate();

            done = true;
        }

        Destroy(this.gameObject.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }

    private float Noise(float x, float y, float scale)
    {
        float result = 0f;

        int multiplier = 1;

        for(int i = 0; i < noiseOctaves; i ++)
        {
            result += (Mathf.PerlinNoise(x * scale * multiplier, y * scale * multiplier) / (multiplier + 0f));

            multiplier *= 2;
        }

        return result / 2f; 
    }

    public float GetHeight(int i, int j)
    {
        float height = 0f;

        for(int k = 0; k < amplitudes.Length; k ++) {
            float mask = 1f;

            for(int l = 0; l < maskMappingsArr.GetLength(0); l ++)
            {
                if(maskMappingsArr[k,l] == -1) continue;

                float currMask = (Noise((j * worldScale + xOffset + globalSettings.xOffset), (i * worldScale + yOffset + globalSettings.yOffset), maskScales[maskMappingsArr[k,l]])
                - maskBiases[maskMappingsArr[k,l]]) * maskAmplitudes[maskMappingsArr[k,l]];
            
                currMask = Mathf.Max(0, Mathf.Min(1, currMask));

                mask *= currMask;
            }

            mask = Mathf.Max(0, Mathf.Min(1, mask));

            height += Noise((j * worldScale + xOffset + globalSettings.xOffset), (i * worldScale + yOffset + globalSettings.yOffset), scales[k]) * amplitudes[k] * mask;
        }

        return height;
    }

    public void GenerateArr()
    {
        maskMappingsArr = new int[maskMapping.Length,6];

        for (int i = 0; i < maskMapping.Length; i ++)
        {
            int currNum = maskMapping[i];

            for (int j = 0; j < 5; j ++)
            {
                int currDigit = currNum % 10;

                maskMappingsArr[i,j] = currDigit - 1;

                currNum /= 10;
            }
        }
    }

    void GenerateTerrain()
    {
        mesh.Clear();

        verts = new Vector3[(sizeX + 1) * (sizeY + 1)];
        uvs = new Vector2[verts.Length];

        int index = 0;

        for (int i = 0; i <= sizeY; i ++)
        {
            for(int j = 0; j <= sizeX; j ++)
            {
                float currUvMask = (Noise((j * worldScale + xOffset + globalSettings.xOffset), (i * worldScale + yOffset + globalSettings.yOffset), maskScales[uvMask]) - maskBiases[uvMask]) * maskAmplitudes[uvMask];
                currUvMask = Mathf.Max(0, Mathf.Min(1, currUvMask));

                float height = GetHeight(i, j);

                //uvs[index] = new Vector2(currUvMask, 0);
                if(i < sizeY && j < sizeX)
                {
                    uvs[index] = new Vector2(currUvMask, Mathf.Sqrt(Mathf.Pow(GetHeight(i, j) - GetHeight(i + 1, j), 2) + Mathf.Pow(GetHeight(i, j) - GetHeight(i, j + 1), 2)) / 5f + steepnessBias);
                    uvs[index] = new Vector2(uvs[index].x / 10f, Mathf.Max(0, Mathf.Min(1, uvs[index].y / 10f)));

                    if(Mathf.Repeat(uvs[index].y, 1f) < 0.2f)
                    {
                        uvs[index].y = 0f;
                    }
                    else
                    {
                        uvs[index].y = 1f;
                    }

                    uvs[index].x *= 10f;

                    if(uvs[index].x <= 0.5f)
                    {
                        uvs[index].x = 0f;
                    }
                    else
                    {
                        uvs[index].x = 1f;
                    }

                    if(uvs[index].y < 0.5f && scatterObjects && height * transform.localScale.y + transform.position.y > -107.6 + 3)
                    {
                        float val = (float) this.random.NextDouble();

                        for(int k = 0; k < probabilities.Length; k ++)
                        {                   
                            float currMask = uvs[index].x;

                            if(invertedMasks[k])
                            {
                                currMask = 1 - currMask;
                            }

                            if(val < probabilities[k] && Noise(j * worldScale + xOffset + globalSettings.xOffset, i * worldScale + yOffset + globalSettings.yOffset, .02f) > 0.4f && currMask < 0.5f)
                            {
                                GameObject instance = Instantiate(objectsToScatter[k]);
                                instance.transform.position = new Vector3(
                                    j * transform.localScale.x + transform.position.x,
                                    height * transform.localScale.y + transform.position.y,
                                    i * transform.localScale.z + transform.position.z
                                );

                                instance.transform.eulerAngles = new Vector3(
                                    instance.transform.eulerAngles.x, UnityEngine.Random.Range(0f, 360f), instance.transform.eulerAngles.z
                                );

                                float randomScale = UnityEngine.Random.Range(.5f, 2f);

                                instance.transform.localScale = new Vector3(
                                    randomScale * instance.transform.localScale.x,
                                    randomScale * instance.transform.localScale.y,
                                    randomScale * instance.transform.localScale.z
                                );

                                break;
                            }
                            else
                            {
                                val -= probabilities[k];
                            }
                        }
                    }

                    if(Math.Abs(height * transform.localScale.y + transform.position.y + 107.6) < 2)
                    {
                        uvs[index].x = 1f;
                    }

                    uvs[index].x /= 2f;
                    uvs[index].y /= 2f;

                    Vector2 uvComponent = new Vector2(j, i);
                    uvComponent.x = Mathf.Repeat(uvComponent.x / 5f, 1f);
                    uvComponent.y = Mathf.Repeat(uvComponent.y / 5f, 1f);

                    uvComponent.x /= 2f;
                    uvComponent.y /= 2f;

                    uvs[index].x += uvComponent.x;
                    uvs[index].y += uvComponent.y;

                    //uvs[index].x = uvComponent.x * 2;
                    //uvs[index].y = uvComponent.y * 2;
                }
                else
                {
                    uvs[index] = new Vector2(0, 0);
                }

                verts[index] = new Vector3(j, height, i);
                index ++;
            }
        }

        tris = new int[sizeY * sizeX * 6];

        int vertIndex = 0;
        int triIndex = 0;

        for(int i = 0; i < sizeY; i ++)
        {
            for(int j = 0; j < sizeX; j ++)
            {
                tris[triIndex + 0] = vertIndex;
                tris[triIndex + 1] = vertIndex + sizeX + 1;
                tris[triIndex + 2] = vertIndex + 1;
                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = vertIndex + sizeX + 1;
                tris[triIndex + 5] = vertIndex + sizeX + 2;

                vertIndex ++;
                triIndex += 6;
            }

            vertIndex ++;
        }
    }

    public void MeshUpdate()
    {
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (verts == null || verts.Length == 0 || !gizmos)
            return;

        Gizmos.color = Color.red;

        foreach (Vector3 vertex in verts)
        {
            Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.1f);
        }
    }

    void Update() {
        if(update) {
            GenerateArr();

            mesh = new Mesh();

            GetComponent<MeshFilter>().mesh = mesh;

            GenerateTerrain();
            MeshUpdate();
        }
    }
}
