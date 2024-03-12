using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            hide();
        });
        startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            hide();
        });
    }
    private void hide()
    {
        gameObject.SetActive(false);
    }
}
