using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : SingletonMonoBehaviour<GameManagerScript>
{
    private GameObject m_Princess;
    private GameObject m_Robot;
    private bool m_IsUsingPrincess = false;

    private int m_Row = 9;

    private int m_Col = 16;


    private float m_OffsetX, m_OffsetY;

    public Sprite m_S;

    private List<GameObject> m_blockList = new List<GameObject>();
    private bool[,] m_blockExistList;

    [SerializeField]
    private GameObject m_Prefab;

    // ステージカウント
    private int m_StageCount = 1;

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
        //        m_Princess = GameObject.Find("Princess");
        //        m_Robot = GameObject.Find("Robot");
        //        SetUseCharacter();

        SoundManager.I.PlayBGM("ingame");

        this.hoge();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Change"))
        {
            SetUseCharacter();
        }
    }

    private void SetUseCharacter()
    {
        m_Princess = m_Princess ?? GameObject.Find("Princess");
        m_Robot = m_Robot ?? GameObject.Find("Robot");
        m_IsUsingPrincess = !m_IsUsingPrincess;
        m_Princess.GetComponent<PrincessScript>().ChangeCharacter(m_IsUsingPrincess);
        m_Robot.GetComponent<RobotScript>().ChangeCharacter(!m_IsUsingPrincess);
    }
    private Camera _mainCamera;

    void hoge()
    {
        var reader = new CSVReader();
        if(!reader.LoadFile("stage3"))
        {
            return;
        }
        var data = reader.GetData();

        var stageData = Resources.Load<CommonDataSet>("CommonDataSet");
        this.m_Row = (int)stageData.m_StageSize[m_StageCount-1].y;
        this.m_Col = (int)stageData.m_StageSize[m_StageCount-1].x;
        Debug.Log(m_Row + " " + m_Col);

        m_blockExistList = new bool[this.m_Row, this.m_Col];
        for (int i = 0; i < this.m_Row; i++)
        {
            for (int j = 0; j < this.m_Col; j++)
            {
                m_blockExistList[i, j] = false;
            }
        }

        // 座標値を出力
        Debug.Log(getScreenTopLeft().x + ", " + getScreenTopLeft().y);
        Debug.Log(getScreenBottomRight().x + ", " + getScreenBottomRight().y);

        var topL = getScreenTopLeft();
        var botR = getScreenBottomRight();

        float width = botR.x - topL.x;
        float height = topL.y - botR.y;

        m_OffsetX = width / this.m_Col;
        m_OffsetY = height / this.m_Row;

        float startX = topL.x + (m_OffsetX / 2f);
        float startY = topL.y - (m_OffsetY / 2f);

        float curX = startX;
        float curY = startY;


        GameObject blocks = new GameObject("blocks");

        var scale = m_S.textureRect.size.x;
        Debug.Log(scale);

        //TODO
        for(int i = 0; i < this.m_Row - 1; i++)
        {
            for(int j = 0; j < this.m_Col; j++)
            {
                if (data[i][j] != "")
                {
                    GameObject obj = new GameObject(i.ToString() + j.ToString());
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = Resources.Load<Sprite>("Texture/" + data[i][j]);
                    obj.transform.position = new Vector3(curX, curY, 0);
                    obj.transform.localScale = new Vector3(0.435f, 0.435f, 1);

                    obj.transform.parent = blocks.transform;
                    m_blockList.Add(obj);
                    m_blockExistList[i, j] = true;
                }
                curX += m_OffsetX;
            }
            curX = startX;
            curY -= m_OffsetY; 
        }
    }

    Vector2 GetPos(Vector3 pos)
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
                if(curX < pos.x && pos.x < curX + m_OffsetX)
                {
                    if(curY - m_OffsetY < pos.y && pos.y < curY)
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

    public void Action(Transform tr, bool isPri)
    {
        var pos = this.GetPos(tr.position);
        Debug.Log(pos);

        if(isPri)
        {
            Debug.Log(tr.localScale);

            // 隣にブロックがあれば
            if(m_blockExistList[(int)pos.y, (int)pos.x + 1 * (int)tr.localScale.x])
            {
                Debug.Log("ya");
                return;
            }
            
            var topL = getScreenTopLeft();

//            var obj = Instantiate(m_Prefab, new Vector3(topL.x + m_OffsetX * (pos.x + 1 /** tr.right*/), topL.y - m_OffsetY * pos.y), Quaternion.identity) as GameObject;
            var obj = new GameObject(pos.y.ToString() + pos.x.ToString());
            var r = obj.AddComponent<SpriteRenderer>();
            r.sprite = m_S;

            obj.transform.localScale = new Vector3(0.435f, 0.435f,1);
            obj.AddComponent<BoxCollider>();
            obj.transform.position = new Vector3(topL.x + m_OffsetX * (pos.x + 1f * (int)tr.localScale.x), topL.y - m_OffsetY * pos.y);

            obj.transform.parent = GameObject.Find("blocks").transform;
            
            m_blockExistList[(int)pos.y, (int)pos.x + 1 * (int)tr.localScale.x] = true;
        }
        else
        {
            if(!m_blockExistList[(int)pos.y, (int)pos.x + 1 * (int)tr.localScale.x])
            {
                Debug.Log("yo");
                return;
            }

//            var topL = getScreenTopLeft();


            
            m_blockExistList[(int)pos.y, (int)pos.x + 1 * (int)tr.localScale.x] = false;
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
