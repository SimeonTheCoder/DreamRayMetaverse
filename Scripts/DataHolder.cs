using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataHolder
{
    public string[] objType;
    public float[] position;
    public float[] rotation;
    public float[] scale;

    public DataHolder(Transform[] objs)
    {
        objType = new string[objs.Length];

        position = new float[objs.Length * 3];
        rotation = new float[objs.Length * 3];
        scale = new float[objs.Length * 3];

        for(int i = 0; i < objs.Length; i ++)
        {
            position[0 + i * 3] = objs[i].position.x;
            position[1 + i * 3] = objs[i].position.y;
            position[2 + i * 3] = objs[i].position.z;

            rotation[0 + i * 3] = objs[i].eulerAngles.x;
            rotation[1 + i * 3] = objs[i].eulerAngles.y;
            rotation[2 + i * 3] = objs[i].eulerAngles.z;

            scale[0 + i * 3] = objs[i].localScale.x;
            scale[1 + i * 3] = objs[i].localScale.y;
            scale[2 + i * 3] = objs[i].localScale.z;

            objType[i] = objs[i].gameObject.name.Replace("(Clone)", "");
        }
    }
}
