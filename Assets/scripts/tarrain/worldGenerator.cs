using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.AI.Navigation;


public class worldGenerator : MonoBehaviour
{
    // Deklaracja czterowymiarowej tablicy
    int[,,] chunkData = new int[16, 16, 256];

    // Przyk³adowe u¿ycie tablicy - ustawienie wartoœci dla konkretnego bloku
    private int xPos, yPos, zPos, idBlock, localHeight;
    public GameObject blockPrefab;
    [SerializeField] private NavMeshSurface firstsurface;
    private void Start()
    {
        int loopNumber = 0;
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[256];
        Vector2[] uv = new Vector2[256];
        int[] triangles = new int[15*15*6];
        for (int x = 0; x <= 15; x++)
        {
            for (int z = 0; z <= 15; z++)
            {
                localHeight = Random.Range(6, 8);
                for (int y = 0; y < localHeight; y++)
                {
                    xPos = x;
                    zPos = z;
                    yPos = y;
                    chunkData[xPos, yPos, zPos] = 1;
                    //Vector3 spawnPosition = transform.position + new Vector3(xPos - 8, yPos, zPos - 8);
                    
                }
                Debug.Log(chunkData[5,10,5]);
                vertices[loopNumber] = new Vector3(xPos*5, yPos, zPos*5);
                loopNumber++;
            }
        }
        for (int i = 0; i < 15*14; i+=1)
        {
            triangles[i*6]   = i+16;
            triangles[i*6+1] = i;
            triangles[i*6+2] = i+1;

            triangles[i*6+3] = i+16;
            triangles[i*6+4] = i+1;
            triangles[i*6+5] = i+17;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;

    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            firstsurface.BuildNavMesh();
        }
    }
}
