using UnityEngine;
using System.Collections;

public class ButtonMulti : MonoBehaviour
{
    public int              index;
    public Texture          defaultTex, selectedTex;

    private GUITexture      _guiTex;
    private Texture         _textureToAdd;
    private ButtonManager   _buttonManager;

    void Start()
    {
        _guiTex = GetComponent<GUITexture>();
        _textureToAdd = defaultTex;
        _buttonManager = GameObject.FindGameObjectWithTag("ButtonManager").GetComponent<ButtonManager>();
    }

    void OnMouseEnter()
    {
        _textureToAdd = selectedTex;
    }

    void OnMouseExit()
    {
        _textureToAdd = defaultTex;
    }

    void OnMouseDown()
    {
        switch (index)
        {
            case 1: Debug.Log("AddServer");
                break;
            case 2: Debug.Log("Direct Connect");
                _buttonManager.DirectConnection();
                break;
            case 3: Debug.Log("Exit");
                Application.LoadLevel("MainMenu");
                break;
        }
    }

    void Update()
    {
        _guiTex.texture = _textureToAdd;
    }
}
