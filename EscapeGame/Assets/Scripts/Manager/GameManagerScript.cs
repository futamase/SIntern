using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : SingletonMonoBehaviour<GameManagerScript>
{
    private GameObject m_Princess;
    private GameObject m_Robot;
    private bool m_IsUsingPrincess = false;

    [SerializeField]
    private int m_Row = 9;

    [SerializeField]
    private int m_Col = 16;


    private float m_OffsetX, m_OffsetY;

    public Sprite m_S;

    private List<GameObject> m_blockList = new List<GameObject>();
    private bool[,] m_blockExistList;

	private float m_LeftEnd;
	private float m_RightEnd;


    [SerializeField]
    private GameObject m_Block;

    void Awake()
    {
        if (this != I)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        SoundManager.I.PlayBGM("ingame");

        this.hoge();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private Camera _mainCamera;

    void hoge()
    {
		// 座標値を出力
		Debug.Log(getScreenTopLeft().x + ", " + getScreenTopLeft().y);
		Debug.Log(getScreenBottomRight().x + ", " + getScreenBottomRight().y);

		var topL = getScreenTopLeft();
		var botR = getScreenBottomRight();

		float width = botR.x - topL.x;
		float height = topL.y - botR.y;

		m_OffsetX = GameObject.FindGameObjectWithTag ("Block").GetComponent<SpriteRenderer> ().bounds.size.x;
		m_OffsetY = GameObject.FindGameObjectWithTag ("Block").GetComponent<SpriteRenderer> ().bounds.size.y;

		m_Col = (int)Mathf.Floor(width / m_OffsetX);
		m_Row = (int)Mathf.Floor(height / m_OffsetY);

		m_LeftEnd = topL.x + ((float)m_Col - (width / m_OffsetX)) / 2;
		m_RightEnd = botR.x - ((float)m_Col - (width / m_OffsetX)) / 2;

		m_blockExistList = new bool[this.m_Row, this.m_Col];
        for (int i = 0; i < this.m_Row; i++)
        {
            for (int j = 0; j < this.m_Col; j++)
            {
                m_blockExistList[i, j] = false;
            }
        }

//        m_OffsetX = width / this.m_Col;
//        m_OffsetY = height / this.m_Row;

        GameObject blocks = new GameObject("blocks");

//		float startX = topL.x + (m_OffsetX / 2f);
//		float startY = topL.y - (m_OffsetY / 2f);
//      var scale = m_S.textureRect.size.x;
//      Debug.Log(scale);
//		float curX = startX;
//		float curY = startY;
//        //TODO
//        for(int i = 0; i < this.m_Row - 1; i++)
//        {
//            for(int j = 0; j < this.m_Col; j++)
//            {
//                GameObject obj = new GameObject(i.ToString() + j.ToString());
//                SpriteRenderer renderer= obj.AddComponent<SpriteRenderer>();
//                renderer.sprite = m_S;
//                obj.transform.position = new Vector3(curX, curY, 0);
//                obj.transform.localScale = new Vector3(0.435f, 0.435f, 1);
//
//                obj.transform.parent = blocks.transform;
//                m_blockList.Add(obj);
//
//                curX += m_OffsetX;
//
//                m_blockExistList[i, j] = true;
//            }
//            curX = startX;
//            curY -= m_OffsetY; 
//        }
//		Debug.Log(m_OffsetX);
//		Debug.Log (m_OffsetY);
    }

	Vector2 GetPos(Vector3 pos, float PosOffsetX = 0, float PosOffsetY = 0)
    {
        var topL = this.getScreenTopLeft();

        float startX = topL.x;
        float startY = topL.y;

        float curX = startX;
        float curY = startY;

        Vector2 ind = new Vector2();
        
        for(int i = 0; i < this.m_Row; i++)
        {
            for(int j = 0; j < this.m_Col; j++)
            {
				if(curX < pos.x + PosOffsetX && pos.x + PosOffsetX < curX + m_OffsetX)
                {
					if(curY - m_OffsetY < pos.y - PosOffsetY && pos.y- PosOffsetY < curY)
                    {
                        // プレイヤーの場所
                        ind.x = j;
                        ind.y = i;
                        return ind;
                    }
                }
                curX += m_OffsetX;

            }
            curX = startX;
            curY -= m_OffsetY; 
        }

        return ind;
    }

	public void Action(Transform tr, bool isPri, Vector3 size)
    {
		
		var pos = this.GetPos(tr.position, size.x/2 * tr.localScale.x, size.y/4);
        Debug.Log(pos);

        if(isPri)
        {
            Debug.Log(tr.localScale);

			int x = (int)pos.x + (1 * (int)tr.localScale.x);
			Debug.Log (pos.x);
			Debug.Log (x);
			int y = (int)pos.y;
            // 隣にブロックがあれば
			if(m_blockExistList[y, x])
            {
                Debug.Log("ya");
                return;
            }
            
            var topL = getScreenTopLeft();

			var obj = Instantiate(m_Block, new Vector3(m_LeftEnd + m_OffsetX * x, topL.y - m_OffsetY * y), Quaternion.identity) as GameObject;
//            var obj = new GameObject(pos.y.ToString() + pos.x.ToString());
//            var r = obj.AddComponent<SpriteRenderer>();
//            r.sprite = m_S;
//
//            obj.transform.localScale = new Vector3(0.435f, 0.435f,1);
//            obj.AddComponent<BoxCollider>();
//            obj.transform.position = new Vector3(topL.x + m_OffsetX * (pos.x + 1f * (int)tr.localScale.x), topL.y - m_OffsetY * pos.y);

            obj.transform.parent = GameObject.Find("blocks").transform;
			Debug.Log(obj.transform.FindChild ("Sprite").GetComponent<SpriteRenderer> ().bounds.size.x);
            m_blockExistList[y, x] = true;
        }
        else
        {

        }
    }

    private Vector3 getScreenTopLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft;
    }

    private Vector3 getScreenBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight;
    }
}
