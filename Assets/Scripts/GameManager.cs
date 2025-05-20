using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform cursorTexture;

    void Awake()
    {
        //Cursor.SetCursor(cursorTexture, Vector2.left, CursorMode.ForceSoftware);
        Cursor.visible = false;
    }

    void Update()
    {
        cursorTexture.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }


}
