using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class worldGenerator : MonoBehaviour
{
    // Deklaracja czterowymiarowej tablicy
    int[,,] chunkData = new int[16, 16, 256];
    Mesh mesh = new Mesh();
    Vector3[] vertices = new Vector3[256];
    Vector2[] uv = new Vector2[256];
    int[] triangles = new int[384];

    // Przyk³adowe u¿ycie tablicy - ustawienie wartoœci dla konkretnego bloku
    private int xPos, yPos, zPos, idBlock, localHeight;
    public GameObject blockPrefab;
    private void Start()
    {

        for (int x = 0; x <= 16; x++)
        {
            for (int z = 0; z <= 16; z++)
            {
                localHeight = Random.Range(4, 8);
                for (int y = 0; y < localHeight; y++)
                {
                    xPos = x;
                    zPos = z;
                    yPos = y;
                    chunkData[xPos, yPos, zPos] = 1;
                    Vector3 spawnPosition = transform.position + new Vector3(xPos - 8, yPos, zPos - 8);
                    
                }

            }
        }
        Debug.Log(chunkData);
    }
}
