using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TestLoadLabel : MonoBehaviour
{
    public RawImage img;
    public AssetLabelReference textueLabelRef;
    // Start is called before the first frame update

    
    void Start()
    {
        int i = 0;
        Addressables.LoadAssetsAsync<Texture2D>(textueLabelRef, (Texture2D texture) => {
            Debug.LogError("加载了一个资源： " + texture.name);

            i++;
            GameObject itemGO = Instantiate(img.gameObject);
            itemGO.transform.SetParent(img.gameObject.transform.parent);
            RawImage itemImg = itemGO.GetComponent<RawImage>();
            itemImg.texture = texture;
            itemImg.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            itemImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + i * (100 + 10), -10);


        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
