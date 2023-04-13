using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedLock : MonoBehaviour
{
  private Button btn;
  public bool seedLocked = false;

  void Start()
  {
      btn = this.GetComponent<Button>();
      btn.onClick.AddListener(OnObjectsClick);
          btn.GetComponent<Image>().color = Color.gray; 
  }


    private void OnObjectsClick()
    {
        seedLocked = !seedLocked;
        if (seedLocked)
        {
            btn.GetComponent<Image>().color = new Color(255,215,0);
        } else
        {
            btn.GetComponent<Image>().color = Color.gray; 
        }
    }
}
