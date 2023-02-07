using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public ItemController[] itemButtons;
    public GameObject[] itemPrefabs;
    public int currentButtonPressed;
    public GameObject scaleSlider;
    public GameObject[] itemImage;

    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (Input.GetMouseButtonDown(0) && itemButtons[currentButtonPressed].clicked)
        {
            itemButtons[currentButtonPressed].clicked = false;
            GameObject go= Instantiate(itemPrefabs[currentButtonPressed], new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
            go.transform.localScale = new Vector3(2000 * scaleSlider.GetComponent<Slider>().value, 2000 * scaleSlider.GetComponent<Slider>().value, 0);
            go.GetComponent<Renderer>().sortingOrder = 5;

            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
    }
}
