using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;

    private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = obj.transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - Input.mousePosition;

            //print("origin " + dragOrigin + " new Position " + cam.ScreenToWorldPoint(Input.mousePosition) + " = difference " + difference);

            obj.transform.position += difference;
        }

    }
}
