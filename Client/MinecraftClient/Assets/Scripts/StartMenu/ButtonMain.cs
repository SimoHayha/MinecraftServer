using UnityEngine;
using System.Collections;

public class ButtonMain : MonoBehaviour
{
    public int              index;
    public Texture          defaultTex, selectedTex;

    private GUITexture      _guiTex;
    private Texture         _textureToAdd;

    void Start()
    {
        _guiTex = GetComponent<GUITexture>();
        _textureToAdd = defaultTex;
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
            case 1: Debug.Log("MultiplayerClicked");
                Application.LoadLevel("MultiplayerMenu");
                break;
            case 2: Debug.Log("LeavingGame");
                Application.Quit();
                break;
        }
    }

    void Update()
    {
        _guiTex.texture = _textureToAdd;
    }
}
