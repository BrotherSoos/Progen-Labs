using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegenerateMap : MonoBehaviour
{
    [SerializeField] 
    private TMP_InputField inputField;
    private Button btn;
    private GameObject map = null;
    private GameObject dragger = null;
    private GameObject seedLock = null;
    private string seed;
    
    // Start is called before the first frame update
    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnObjectsClick);
        map = GameObject.Find("Map");
        seedLock = GameObject.Find("SeedLock");
        dragger = GameObject.Find("EditorManager");
    }


    private void OnObjectsClick()
    {
      PerlinNoiseTileSet generationScript = (PerlinNoiseTileSet) map.GetComponent(typeof(PerlinNoiseTileSet));
      PlaceByMouseDrag objectScript = (PlaceByMouseDrag) dragger.GetComponent(typeof(PlaceByMouseDrag));
      objectScript.clearInstantiatedExtras();
      SeedLock lockScript = (SeedLock) seedLock.GetComponent(typeof(SeedLock));
      Debug.Log("Lock set to " + lockScript.seedLocked);
      if(lockScript.seedLocked) {
        SeedGenerator.setSeed(inputField.text);
      }
      else {
        SeedGenerator.generateNew();
        inputField.text = SeedGenerator.computeSeed();
      }
      generationScript.RegenerateMap();
    }
}
