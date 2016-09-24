using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{

    [SerializeField]
    private GameObject m_prefab;

    private List<List<GameObject>> m_stage = new List<List<GameObject>>();

    // Use this for initialization
    void Start()
    {
        var reader = new CSVReader();

        if (reader.LoadFile("hoge"))
        {
            var data = reader.GetData();

            var texture = new Texture2D(1280, 720, TextureFormat.ARGB32, false);

            for (int i = 0; i < data.Count; i++)
            {
                m_stage.Add(new List<GameObject>());
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] == "")
                        continue;

                    var filePath = "Assets/Captures" + data[i][j] + ".png";

                    var tileSprite = Resources.Load<Sprite>("Captures/" + data[i][j]);
//                    GameObject instance = new GameObject(data[i][j]);
//                    var renderer = instance.AddComponent<SpriteRenderer>();
//                    renderer.sprite = Resources.Load<Sprite>("Captures/" + data[i][j]);
//                    instance.transform.position = new Vector3(i, j * 2f, 0);
//                    Debug.Log(renderer.sprite.rect.size.x);

                    // 3.Textureのピクセル情報
                    
                    Color[] colors = tileSprite.texture.GetPixels(
                        (int)tileSprite.textureRect.x,
                        (int)tileSprite.textureRect.y,
                        (int)tileSprite.textureRect.width,
                        (int)tileSprite.textureRect.height
                    );

                    // 4.空のテクスチャへ指定の位置にセット
                    texture.SetPixels(
                        i * (int)tileSprite.textureRect.width, // タイルのx座標位置
                        j * (int)tileSprite.textureRect.height, // タイルのy座標位置
                        (int)tileSprite.textureRect.width,
                        (int)tileSprite.textureRect.height,
                        colors  // 抽出したタイルのピクセル情報
                    );
                }
            }

            texture.Apply();

            //            GameObject instance = new GameObject("hoge");
            //            var renderer = instance.AddComponent<SpriteRenderer>();
            //            renderer.sprite = Sprite.Create(texture, new Rect(0,0,1280,720), Vector2.zero);
            GameObject.Find("Canvas").transform.FindChild("RawImage").GetComponent<RawImage>().texture = texture;
        }
        else
        {
            Debug.Log("false");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}