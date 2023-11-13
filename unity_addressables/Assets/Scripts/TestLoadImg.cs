using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class TestLoadImg : MonoBehaviour
{
    public RawImage img;
    public Button freeResBtn;
    public Texture2D texture2D;

    // Start is called before the first frame update
    void Start()
    {
        var handle = Addressables.LoadAssetAsync<Texture2D>("Assets/Textures/fengjing01.jpg");
        handle.Completed += (handle)=> {
            Texture2D texture2D = handle.Result;
            img.texture = texture2D;
            this.texture2D = texture2D;
            //img.GetComponent<RectTransform>().sizeDelta = new Vector2(texture2D.width, texture2D.height);
        };

        freeResBtn.onClick.AddListener(() =>
        {
            //if (null != img)
            //{
            //    img.texture = null;
            //    //Destroy(img);
            //}

            //if (null != texture2D) { 
            //    Addressables.Release(texture2D );
            //}

            // ÊÍ·Å×ÊÔ´
            Addressables.Release(handle);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
