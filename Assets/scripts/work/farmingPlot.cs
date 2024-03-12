using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmingPlot : MonoBehaviour
{
    [Header("Plantobject")]
    [SerializeField] private GameObject firstStep, secoundStep, fruit;
    [Header("growingLogic")]
    [SerializeField] public float timeToGrow;
    public float growingProgres;
    [SerializeField] public int foodOutpoot;
    public bool isPlanted = true;
    private void FixedUpdate()
    {
        if (isPlanted)
        {
            if (growingProgres < timeToGrow)
                growingProgres += Time.deltaTime;
        }
        if (growingProgres >= timeToGrow/3)
        {
            firstStep.SetActive(true);
        }
        if (growingProgres>=timeToGrow)
        {
            firstStep.SetActive(false);
            secoundStep.SetActive(true);
            gameObject.tag = "readyToHarvest";
        }
    }
    public void work()
    {
        if (growingProgres >= timeToGrow)
        {
            for (int i = 0; i < foodOutpoot; i++)
            {

                secoundStep.SetActive(false);
                Instantiate(fruit, new Vector3(gameObject.transform.position.x+ Random.Range(-2,2), gameObject.transform.position.y+2, gameObject.transform.position.z + Random.Range(-2, 2)), Quaternion.identity);
                growingProgres = 0;
                isPlanted = false;
            }
        }
        else
        {
            isPlanted = true;
            gameObject.tag = "noWorkPlace";
        }
    }
}
