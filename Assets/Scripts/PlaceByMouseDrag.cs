using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceByMouseDrag : MonoBehaviour
{

    public int ID;
    public bool clicked;
    private EditorManager editor;
    private GameObject scaleSlider;
    private List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();

        scaleSlider = editor.scaleSlider;
    }

    private void OnMouseDrag()
    {

        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Input.GetMouseButtonDown(1))
        {
            GameObject go = Instantiate(editor.itemImage[ID], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            go.GetComponent<Renderer>().sortingOrder = 1000000000;
            go.transform.localScale = new Vector3(200 * scaleSlider.GetComponent<Slider>().value, 200 * scaleSlider.GetComponent<Slider>().value, 0);
            editor.currentButtonPressed = ID;
            placedObjects.Add(go);
            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
    }

    public void clearInstantiatedExtras() {
      foreach(GameObject go in placedObjects) {
        Destroy(go);
      }
    }
}
