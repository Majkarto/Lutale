using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class colonistLogic : NetworkBehaviour
{
    private enum State
    {
        Idle,
        Working,
        Sleeping,
        Fighting,
        InComat,
        Crafting,
        NeedSMG,
        Walking,
        Strolling,
    }
    private State currentState;
    [Header("works")]
    [SerializeField]
    private bool isFarmer;
    [SerializeField] private bool isResearcher, isBuilder, isCrafter, isHunter, isMiner, isWarehouseman;
    [Header("needs")]
    [SerializeField] private float maxHunger;
    [SerializeField]    private float maxHealth, maxIlness, maxFun, maxEnergy;
    private bool isill = false;
    private bool noFood = false;
    private bool noWork = false;
    private float Hunger, Health, Illnes, Fun, Energy;
    private string whatIsNedded;

    [Header("searching system")]
    private GameObject[] AllObjects;
    private GameObject nerestObject, objectToStore;
    float distance;
    float maxNerestDistance = 10000;
    float nerestDistance = 10000;
    [Header("worksystem")]
    private GameObject workingplace;
    private bool isWorking=false;
    [Header("other")]
    [SerializeField] private Animator aninate;
    private State lastState;
    private float fixedUpdateTimer = 0f;
    private float fixedUpdateInterval = 3f;
    private NavMeshAgent navMeshAgent;
    //public override void OnNetworkSpawn()
    private void Start()
    {
        currentState = State.Idle;
        navMeshAgent = GetComponent<NavMeshAgent>();
        Hunger = maxHunger;
        Health = maxHealth;
        Illnes = maxIlness;
        Fun = maxFun;
        Energy = maxEnergy;
    }
    void FixedUpdate()
    {
        //        if(!IsOwner)
        //        {
        //            return;
        //        }
        if(currentState!=State.Working && isWorking)
        {
            isWorking = false;
            nerestDistance = maxNerestDistance;
            nerestObject = null;
        }
        needsupdate();
        if (currentState == State.Idle)
        {

            Idle();
        }
        if (currentState == State.Working)
            Working();
        if (currentState == State.Sleeping)
            Sleeping();
        if (currentState == State.Fighting)
            Fighting();
        if (currentState == State.InComat)
            InComat();
        if (currentState == State.Crafting)
            Crafting();
        if (currentState == State.NeedSMG)
            NeedSMG(whatIsNedded);
        if (currentState == State.Walking)
            Walking();
        if (currentState == State.Strolling)
            strolling();
    }
    private void needsupdate()
    {
        if (Hunger>0)
        {
            Hunger -= Time.deltaTime * 0.3f;
        }
        if ((Hunger<30) && !noFood)
        {
            whatIsNedded = "Hunger";
            currentState = State.NeedSMG;
        }
        if(currentState == State.NeedSMG && whatIsNedded== "Hunger" && noFood)
        {
            currentState = State.Idle;
        }
        if(Hunger<=0)
        {
            Health -= Time.deltaTime * 3.1f;
        }
        if(Health<0)
        {
            Destroy(gameObject);
        }
    }
    private void Idle()
    {
        fixedUpdateTimer += Time.deltaTime;
        if (fixedUpdateTimer >= fixedUpdateInterval)
        {
            fixedUpdateTimer = 0f;
            nerestDistance = maxNerestDistance;
            if (GameObject.FindGameObjectsWithTag("workPlace").Length > 0)
            {
                currentState = State.Working;
            }
            else
            {
                aninate.SetBool("isWalking", false);
                aninate.SetBool("isMining", false);
                aninate.SetBool("isPickingUp", false);
            }
        }

    }
    private void Working()
    {

        if (isFarmer)
        {
            findObjets("readyToHarvest");
            if (AllObjects.Length != 0)
            {
                if (nerestDistance < 0.5 && nerestObject.tag == "readyToHarvest")
                {
                    aninate.SetBool("isWalking", false);
                    aninate.SetBool("isMining", false);
                    aninate.SetBool("isPickingUp", true);
                    workingplace = nerestObject;
                    isWorking = true;
                    Debug.Log("workfarm");
                    lastState = currentState;
                    workingplace.SendMessage("work", SendMessageOptions.DontRequireReceiver);
                    currentState = State.Idle;
                }
                else
                {
                    navMeshAgent.destination = nerestObject.transform.position;
                    lastState = currentState;
                    currentState = State.Walking;
                }
            }
        }
        if(isResearcher)
        {
            findObjets("workPlace");
            if(AllObjects.Length != 0)
            {
                if (nerestDistance < 2 && nerestObject.tag == "workPlace")
                {
                    aninate.SetBool("isWalking", false);
                    aninate.SetBool("isMining", true);
                    aninate.SetBool("isPickingUp", false);
                    workingplace = nerestObject;
                    isWorking = true;
                    Debug.Log("worklab");
                    lastState = currentState;
                    //workingplace.SendMessage("work", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    navMeshAgent.destination = nerestObject.transform.position;
                    lastState = currentState;
                    currentState = State.Walking;
                }
            }
        }
        if(isWarehouseman)
        {
            objectToStore = null;
            findObjets("food");
            if (AllObjects.Length != 0)
            {
                if(nerestObject.tag == "food")
                {
                    itemLogic itemScrpit = nerestObject.GetComponent<itemLogic>();
                    if(itemScrpit.isStored)
                    {
                        if (nerestDistance < 2 && nerestObject.tag == "food ")
                        {
                            aninate.SetBool("isWalking", false);
                            aninate.SetBool("isMining", false);
                            aninate.SetBool("isPickingUp", true);
                        }
                        else
                        {
                            navMeshAgent.destination = nerestObject.transform.position;
                            lastState = currentState;
                            currentState = State.Walking;
                        }
                    }
                }
            }
            findObjets("weapon");
            if (AllObjects.Length != 0)
            {
                if (nerestObject.tag == "food")
                {
                    itemLogic itemScrpit = nerestObject.GetComponent<itemLogic>();
                    if (itemScrpit.isStored)
                    {
                        if (nerestDistance < 2 && nerestObject.tag == "food ")
                        {

                        }
                        else
                        {
                            navMeshAgent.destination = nerestObject.transform.position;
                            lastState = currentState;
                            currentState = State.Walking;
                        }
                    }
                }
            }
            findObjets("item");
            if (AllObjects.Length != 0)
            {
                if (nerestObject.tag == "food")
                {
                    itemLogic itemScrpit = nerestObject.GetComponent<itemLogic>();
                    if (itemScrpit.isStored)
                    {
                        if (nerestDistance < 2 && nerestObject.tag == "food ")
                        {

                        }
                        else
                        {
                            navMeshAgent.destination = nerestObject.transform.position;
                            lastState = currentState;
                            currentState = State.Walking;
                        }
                    }
                }
            }


        }
        if (AllObjects.Length == 0)
        {
            noWork = true;
            currentState = State.Strolling;
        }




    }
    private void Sleeping()
    {
       Energy += Time.deltaTime * 3f;
    }
    private void Fighting()
    {

    }
    private void InComat()
    {

    }
    private void Crafting()
    {

    }
    private void NeedSMG(string need)
    {
        if (need == "Hunger")
        {

            AllObjects = GameObject.FindGameObjectsWithTag("food");
            for (int i = 0; i < AllObjects.Length; i++)
            {
                if(AllObjects.Length!=0)
                {
                    if (AllObjects[i].tag != "food")
                    {
                        System.Array.Clear(AllObjects, i, 1);
                    }
                    else
                    {
                        distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);
                        if (distance < nerestDistance)
                        {
                            nerestObject = AllObjects[i];
                            nerestDistance = distance;
                        }
                    }
                }
            }
            if (AllObjects.Length != 0)
            {
                if (nerestDistance < 2 && nerestObject.tag == "food")
                {
                    aninate.SetBool("isWalking", false);
                    aninate.SetBool("isMining", false);
                    aninate.SetBool("isPickingUp", true);
                    Destroy(nerestObject);
                    nerestObject = null;
                    Hunger += 50;
                    currentState = State.Idle;
                    nerestDistance = maxNerestDistance;
                }
                else
                {
                    navMeshAgent.destination = nerestObject.transform.position;
                    lastState = currentState;
                    currentState = State.Walking;
                }
            }
            else
            {
                noFood = true;
            }

        }
    }
    private void strolling()
    {
        if(!noWork)
        {
            lastState = currentState;
            currentState = State.Working;
            return;
        }
        navMeshAgent.destination = new Vector3(gameObject.transform.position.x + Random.Range(-20, 20), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-20, 20));
        lastState = currentState;
        currentState = State.Walking;
    }
    private void Walking()
    {
        aninate.SetBool("isWalking", navMeshAgent.velocity.magnitude>0.01f);
        aninate.SetBool("isMining", false);
        aninate.SetBool("isPickingUp", false);
        if (navMeshAgent.remainingDistance < 0.5)
        {

            fixedUpdateTimer += Time.deltaTime;
            if (fixedUpdateTimer >= fixedUpdateInterval)
            {
                fixedUpdateTimer = 0f;
                currentState = lastState;
                lastState = State.Walking;
                if(noFood || noWork)
                {
                    noFood = false;
                    noWork = false;
                }
            }
        }
    }
    private void findObjets(string tag)
    {
        AllObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < AllObjects.Length; i++)
        {
            if (AllObjects.Length != 0)
            {
                if (AllObjects[i].tag != tag)
                {
                    System.Array.Clear(AllObjects, i, 1);
                }
                else
                {
                    distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);
                    if (distance < nerestDistance)
                    {
                        nerestObject = AllObjects[i];
                        nerestDistance = distance;
                    }
                }
            }
        }
    }


}
