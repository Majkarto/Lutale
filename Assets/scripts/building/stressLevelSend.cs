using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stressLevelSend : MonoBehaviour
{
    [SerializeField] public float objectMass = 4f;
    private bool isdestoyed = false;


    void Start()
    {
        PerformRaycast();
    }
    private void FixedUpdate()
    {
        if (isdestoyed)
        {
            return;
        }
        isFlying();
    }

    void PerformRaycast()
    {

        RaycastHit hit1, hit2;
        float raycastLength = 0.12f;
        if (Physics.Raycast(transform.position + new Vector3(-0.3f, -0.4f, -0.3f), -Vector3.up, out hit1, raycastLength))
        {
            if (Physics.Raycast(transform.position + new Vector3(0.5f, -0.4f, 0.5f), -Vector3.up, out hit2, raycastLength))
            {
                hit1.transform.gameObject.SendMessage("ReceiveMass", objectMass * 0.5f, SendMessageOptions.DontRequireReceiver);
                hit2.transform.gameObject.SendMessage("ReceiveMass", objectMass * 0.5f, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                hit1.transform.gameObject.SendMessage("ReceiveMass", objectMass, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (Physics.Raycast(transform.position + new Vector3(0.3f, -0.4f, 0.3f), -Vector3.up, out hit2, raycastLength))
        {
            hit2.transform.gameObject.SendMessage("ReceiveMass", objectMass, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            isdestoyed = true;
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            }
            die(20f);

        }
    }


    public void ReceiveMass(float receivedMass)
    {
        RaycastHit hit1, hit2;
        float raycastLength = 0.12f;
        if (Physics.Raycast(transform.position + new Vector3(-0.3f, -0.4f, -0.3f), -Vector3.up, out hit1, raycastLength))
        {
            if (Physics.Raycast(transform.position + new Vector3(0.3f, -0.4f, 0.3f), -Vector3.up, out hit2, raycastLength))
            {
                hit1.transform.gameObject.SendMessage("ReceiveMass", receivedMass * 0.5f, SendMessageOptions.DontRequireReceiver);
                hit2.transform.gameObject.SendMessage("ReceiveMass", receivedMass * 0.5f, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                hit1.transform.gameObject.SendMessage("ReceiveMass", receivedMass, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (Physics.Raycast(transform.position + new Vector3(0.3f, -0.4f, 0.3f), -Vector3.up, out hit2, raycastLength))
        {
            hit2.transform.gameObject.SendMessage("ReceiveMass", receivedMass, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            isdestoyed = true;
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            }
            die(20f);

        }

        objectMass += receivedMass;
        if (objectMass > 22)
        {
            isdestoyed = true;
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            }
            die(20f);
        }

    }
    public void die(float timeToDie)
    {
        Destroy(gameObject, timeToDie);
    }


    public void isFlying()
    {
        RaycastHit hit1, hit2;
        float raycastLength = 0.12f;
        if (Physics.Raycast(transform.position + new Vector3(-0.3f, -0.4f, -0.3f), -Vector3.up, out hit1, raycastLength))
        {
        }
        else if (Physics.Raycast(transform.position + new Vector3(0.3f, -0.4f, 0.3f), -Vector3.up, out hit2, raycastLength))
        {
        }
        else
        {
            isdestoyed = true;
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            }
            die(20f);
        }

    }
}

