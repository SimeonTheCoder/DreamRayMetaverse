using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public GameObject saveContainer;
    public TerrainGen terrain;
    
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/" + terrain.xOffset + "_" + terrain.yOffset + ".lvl";

        if(File.Exists(path))
        {
            foreach(Transform child in saveContainer.transform)
            {
                Destroy(child.gameObject);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataHolder data = (DataHolder) formatter.Deserialize(stream);

            for(int i = 0; i < data.position.Length; i += 3)
            {
                GameObject newObj = Instantiate(GameObject.Find(data.objType[i / 3]));
                
                Vector3 pos = new Vector3(
                    data.position[0 + i],
                    data.position[1 + i],
                    data.position[2 + i]
                );

                Vector3 rotation = new Vector3(
                    data.rotation[0 + i],
                    data.rotation[1 + i],
                    data.rotation[2 + i]
                );

                Vector3 scale = new Vector3(
                    data.scale[0 + i],
                    data.scale[1 + i],
                    data.scale[2 + i]
                );

                newObj.transform.position = pos;
                newObj.transform.eulerAngles = rotation;
                newObj.transform.localScale = scale;
            }

            stream.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("o"))
        {
            Save();
        }

        if(Input.GetKeyDown("l"))
        {
            Load();
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + terrain.xOffset + "_" + terrain.yOffset + ".lvl";

        FileStream stream = new FileStream(path, FileMode.Create);

        DataHolder data = new DataHolder(saveContainer.GetComponentsInChildren<Transform>());

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Save succesfull at path: " + path);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/" + terrain.xOffset + "_" + terrain.yOffset + ".lvl";

        if(File.Exists(path))
        {
            foreach(Transform child in saveContainer.transform)
            {
                Destroy(child.gameObject);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataHolder data = (DataHolder) formatter.Deserialize(stream);

            for(int i = 0; i < data.position.Length; i += 3)
            {
                GameObject newObj = Instantiate(GameObject.Find(data.objType[i / 3]));
                
                Vector3 pos = new Vector3(
                    data.position[0 + i],
                    data.position[1 + i],
                    data.position[2 + i]
                );

                Vector3 rotation = new Vector3(
                    data.rotation[0 + i],
                    data.rotation[1 + i],
                    data.rotation[2 + i]
                );

                Vector3 scale = new Vector3(
                    data.scale[0 + i],
                    data.scale[1 + i],
                    data.scale[2 + i]
                );

                newObj.transform.position = pos;
                newObj.transform.eulerAngles = rotation;
                newObj.transform.localScale = scale;
            }

            stream.Close();
        }
    }
}
