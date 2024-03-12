using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isBuildable : MonoBehaviour
{
    public Material goodMaterial;
    public Material badMaterial;
    public Vector3 boxSize = new Vector3(2f, 1f, 1f);
    private Vector3 boxCenter;


    private void FixedUpdate()
    {
        boxCenter = transform.position + new Vector3(0,0.5f,0); // Lokalizacja boxa raycasta
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxSize / 2, Vector3.down);
        GetComponent<Renderer>().material = goodMaterial;
        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                break;
            }
            Debug.DrawLine(boxCenter, hit.point, Color.green);
            Debug.Log("Collision");
            GetComponent<Renderer>().material = badMaterial;
        }
    }

}

