using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public int ID;
    public bool clicked;
    private EditorManager editor;
    private GameObject scaleSlider;

    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();

        scaleSlider = editor.scaleSlider;
    }

    public void ButtonClicked()
    {
        clicked = true;
        

        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        GameObject go = Instantiate(editor.itemImage[ID], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
        go.GetComponent<Renderer>().sortingOrder = 5;
        go.transform.localScale = new Vector3(25 * scaleSlider.GetComponent<Slider>().value, 25 * scaleSlider.GetComponent<Slider>().value, 0);
        editor.currentButtonPressed = ID;
    }
}
