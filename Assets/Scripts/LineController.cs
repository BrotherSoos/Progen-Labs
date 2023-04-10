using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private List<Transform> points = new List<Transform>();
    private bool drawing;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        drawing = false;
    }

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (drawing && Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(new GameObject(), new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            points.Add(go.transform);
            SetUpLines();
        }
        if(points.Count >= 1)
        {
            for (int i = 0; i < points.Count; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
        
    }

    private void SetUpLines()
    {
        lr.positionCount = points.Count;

    }

    public void drawEnabled()
    {
        drawing = true;
    }
}
