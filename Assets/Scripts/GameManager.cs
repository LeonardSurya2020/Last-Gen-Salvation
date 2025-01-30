using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("IsRangeWeapon");
        PlayerPrefs.DeleteKey("SelectedWeapon");
    }
    //[SerializeField]
    //private Texture2D cursorTexture = null;

    //private void Start()
    //{
    //    SetCursorIcon();
    //}

    //private void SetCursorIcon()
    //{
    //    Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2f, cursorTexture.height/2), CursorMode.Auto);
    //}
}
