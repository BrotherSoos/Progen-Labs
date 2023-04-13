using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    private bool hover;

    private void Start()
    {
        hover = false;
    }

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Input.GetMouseButton(0) && hover)
        {
            this.transform.position = worldPos;
        }
    }

    private void OnMouseOver()
    {
        hover = true;
    }

    private void OnMouseExit()
    {
        hover = false;
    }
}
