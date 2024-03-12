using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class objectPlacer : MonoBehaviour
{
    [SerializeField] private NavMeshSurface firstsurface;
    public GameObject objectToPlace;
    public GameObject objectPrePrefab;
    public GameObject objectMassPrePrefab;
    public GameObject objectHolder;
    public float brickSizex = 2f;
    public float brickSizey = 0.5f;
    public float brickSizez = 1f;
    public bool isBuilding = false;
    public LayerMask layerMask;
    private float xStart, yStart, zStart;
    private float xAngle, yAngle, zAngle;
    private float x, y, z;
    private Vector3 spawnPosition;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                yAngle += 90;
                objectHolder.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                var sizeHolder = brickSizex;
                brickSizex = brickSizez;
                brickSizez = sizeHolder;
                if (yAngle == 360)
                {
                    yAngle =0;
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
            {
                if (yAngle == 90 || yAngle == 270)
                {
                    x = (Mathf.Round(hit.point.x / 1) * 1)+0.5f;
                    y = Mathf.Round(hit.point.y / 1 * 1) + 0.6f;
                    z = (Mathf.Round(hit.point.z / 1) * 1)+0.5f;
                }
                else
                {
                    x = Mathf.Round(hit.point.x / 1) * 1;
                    y = Mathf.Round(hit.point.y / 1 * 1) + 0.6f;
                    z = Mathf.Round(hit.point.z / 1) * 1;
                }

                Quaternion rot = Quaternion.Euler(0, yAngle, 0);
                if (objectHolder == null)
                {
                    objectHolder = Instantiate(objectPrePrefab, new Vector3(x, y, z), rot);
                }
                objectHolder.transform.position = new Vector3(x, y, z);
                // SprawdŸ czy lewy przycisk myszy zosta³ wciœniêty
                if (Input.GetMouseButtonDown(0))
                {
                    xStart = x; zStart = z; yStart = y;
                }
                if (Input.GetMouseButton(0))
                {
                    foreach (var obj in GameObject.FindGameObjectsWithTag("massBuildingVisualisation"))
                    {
                        Destroy(obj);
                    }
                    float xVissuaNumber = ((xStart - x) / brickSizex);
                    float zVisualNumber = ((zStart - z) / brickSizez);
                    spawnPosition = new Vector3(xStart, y, zStart);
                    for (int i = 0; i <= Mathf.Abs(xVissuaNumber); i++)
                    {
                        for (int k = 0; k <= Mathf.Abs(zVisualNumber); k++)
                        {
                            Instantiate(objectMassPrePrefab, new Vector3(spawnPosition.x, yStart, spawnPosition.z), rot);
                            if (zVisualNumber > 0)
                            {
                                spawnPosition.z -= brickSizez;
                            }
                            else
                            {
                                spawnPosition.z += brickSizez;
                            }
                        }
                        spawnPosition.z = zStart;
                        if (xVissuaNumber > 0)
                        {
                            spawnPosition.x -= brickSizex;
                        }
                        else
                        {
                            spawnPosition.x += brickSizex;
                        }
                    }
                }
                if(Input.GetMouseButtonUp(0))
                {
                    foreach (var obj in GameObject.FindGameObjectsWithTag("massBuildingVisualisation"))
                    {
                        Destroy(obj);
                    }
                    float xNumber = ((xStart - x)/ brickSizex);
                    float zNumber = ((zStart - z)/ brickSizez);
                    spawnPosition = new Vector3(xStart, y, zStart);
                    for (int i = 0; i <= Mathf.Abs(xNumber); i++)
                    {

                        for (int k = 0; k <= Mathf.Abs(zNumber); k++)
                        {
                            Instantiate(objectToPlace, new Vector3(spawnPosition.x, yStart, spawnPosition.z), rot);
                            if (zNumber > 0)
                            {
                                spawnPosition.z -= brickSizez;
                            }
                            else
                            {
                                spawnPosition.z += brickSizez;
                            }
                        }
                        spawnPosition.z = zStart;
                        if (xNumber >0)
                        {
                            spawnPosition.x -= brickSizex;
                        }
                        else
                        {
                            spawnPosition.x += brickSizex;
                        }
                        firstsurface.BuildNavMesh();
                    }
                }
            }
        }
        else
        {
            if(objectHolder != null)
            {
                Destroy(objectHolder);
            }
            objectHolder = null;
        }    


    }
}
