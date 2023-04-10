using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleScript : MonoBehaviour
{
    public int ID;
    public EditorManager editor;
    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(this.gameObject);
        }
    }

}
