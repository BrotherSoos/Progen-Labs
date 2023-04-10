using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButtonScript : MonoBehaviour
{

    [SerializeField]
    private GameObject popUp;
    private Button btn;
    private bool popUpActive = false;

    // Start is called before the first frame update
    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnObjectsClick);
    }


    private void OnObjectsClick()
    {
        if (!popUpActive)
        {
            popUpActive = !popUpActive;
            popUp.SetActive(true);
        } else
        {
            popUpActive = !popUpActive;
            popUp.SetActive(false);
        }
    }
}
