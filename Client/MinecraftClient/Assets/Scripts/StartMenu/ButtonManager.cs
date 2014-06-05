using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    private GameObject[]    _buttons;
    private MinecraftClient _client;

    void Start()
    {
        _buttons = GameObject.FindGameObjectsWithTag("MenuButton");
        _client = GameObject.FindGameObjectWithTag("ClientManager").GetComponent<MinecraftClient>();
    }

    public void DirectConnection()
    {
        foreach (GameObject go in _buttons)
            go.SetActive(!go.active);

        _client.connection = !_client.connection;
    }
}
