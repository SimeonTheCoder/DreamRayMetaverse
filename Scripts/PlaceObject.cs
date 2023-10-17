using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceObject : MonoBehaviour
{
    public int selectedModule;
    public GameObject[] modules;

    public GameObject saveContainer;

    public GameObject selectorCube;

    public GameObject voxelizer;

    public TextMeshProUGUI text;

    private GameObject prevBuilding;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        this.prevBuilding = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);

            Vector3 placePos = hit.point;

            placePos.x = ((int) placePos.x) / 5 * 5;
            placePos.y = ((int) placePos.y) / 5 * 5;
            placePos.z = ((int) placePos.z) / 5 * 5;

            placePos.x += .9061f;
            placePos.y += .5317f - 1.5f;
            placePos.z += .0041f + 0.793f + 0.4f;

            selectorCube.transform.position = placePos;

            if(Input.GetMouseButtonDown(0))
            {
                int cellX = (int) (placePos.x - voxelizer.transform.position.x) / 5;
                int cellY = (int) (placePos.y - voxelizer.transform.position.y) / 5;
                int cellZ = (int) (placePos.z - voxelizer.transform.position.z) / 5;

                if(cellX >= 0 && cellX < voxelizer.GetComponent<Voxelize>().resolutionX &&
                    cellY >= 0 && cellY < voxelizer.GetComponent<Voxelize>().resolutionZ &&
                    cellZ >= 0 && cellZ < voxelizer.GetComponent<Voxelize>().resolutionY)
                {
                    voxelizer.GetComponent<Voxelize>().grid[cellX, cellZ, cellY] = 2;

                    GameObject copy = Instantiate(modules[selectedModule]);

                    copy.transform.SetParent(saveContainer.transform);

                    copy.transform.position = placePos + new Vector3(0, -1.5f, 0);

                    this.prevBuilding = copy;
                }
            }

            if(Input.GetKeyDown("r"))
            {
                this.prevBuilding.transform.eulerAngles += new Vector3(0, 90f, 0);
            }

            if(Input.GetKeyDown("i"))
            {
                this.selectedModule ++;
                this.selectedModule %= modules.Length;

                text.SetText((this.selectedModule + 1) + ". " + modules[selectedModule].name);
            }
        }
        else
        {
            text.SetText("");
        }

        if(Input.GetKeyDown("t"))
        {
            active = !active;
        }
    }
}
