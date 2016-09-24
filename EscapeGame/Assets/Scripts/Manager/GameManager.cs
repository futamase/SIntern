using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // ステージを生成する矩形の左上の点の座標
    public Vector3 m_GeneratePoint;

    private GameObject m_Princess;
    private GameObject m_Robot;
    private bool m_IsUsingPrincess = false;

    private int m_Row = 0;
    private int m_Col = 0;

    private float m_OffsetX, m_OffsetY;

    private List<GameObject> m_blockList = new List<GameObject>();
    private bool[,] m_blockExistList;

    private float m_LeftEnd;
    private float m_TopEnd;

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

    // オブジェクトを配置
    private void PutOn()
    {
        CSVReader reader = new CSVReader();
        if (!reader.LoadFile("Stages/stage" + m_StageCount.ToString()))
            return;

        var topLeft = m_GeneratePoint;//this.getScreenTopLeft();

        var data = reader.GetData();

        m_Row = data.Count;
        m_Col = data[0].Count;

        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Count; j++)
            {
                if (data[i][j] != "")
                {
                    GameObject obj = Resources.Load("Prefabs/" + data[i][j]) as GameObject;
                    Instantiate(obj, new Vector3(topLeft.x + j + 0.5f, topLeft.y - (i + 0.5f)), Quaternion.identity);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        this.PutOn();
        m_pri = GameObject.Find("Princess");
        var objs = GameObject.FindObjectsOfType<PlayerBaseScript>();
        for (int i = 0; i < objs.Length; i++)
            Debug.Log(objs[i]);
    }
    GameObject m_pri;

    // Update is called once per frame
    void Update()
    {


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(m_GeneratePoint, 0.1f);
    }

    Int2 GetPlayerPos(Vector3 pos)
    {
        for (int i = 0; i < m_Row; i++)
        {
            // y判定
            if (!(m_GeneratePoint.y - i - 1f < pos.y && pos.y < m_GeneratePoint.y - i))
                continue;

            for (int j = 0; j < m_Col; j++)
            {
                // x判定
                if ((m_GeneratePoint.x + j < pos.x && pos.x < m_GeneratePoint.x + j + 1f))
                    return new Int2(j, i);
            }
        }
        return new Int2(-1, -1);
    }

    // 姫さまアクション
    public void ActionHime(Transform tr)
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(tr.position, Vector3.right * (int)tr.localScale.x, out hit, 1);
        Debug.DrawRay(tr.position, Vector3.right * (int)tr.localScale.x, Color.blue, 1f);

        if (isHit)
            return;

        var p = this.GetPlayerPos(tr.position);

        // 範囲外チェック
        if (p.x == -1 || p.y == -1)
            return;

        // 生成する
        var x = p.x + (int)tr.localScale.x;

        Vector3 wannaBePos = new Vector3(m_GeneratePoint.x + p.x + 0.5f, m_GeneratePoint.y - p.y - 0.5f);

        tr.DOLocalMove(wannaBePos, 0.5f)
            .OnComplete(() =>
            {
                Vector3 genePos = new Vector3(m_GeneratePoint.x + x + 0.5f, m_GeneratePoint.y - p.y - 0.5f);
                GameObject obj = Resources.Load("Prefabs/Block") as GameObject;
                var instance = Instantiate(obj, genePos, Quaternion.identity);
            });
    }

    // ロボアクション
    public void ActionRobo(Transform tr, bool isPressedShift)
    {
        // transform.positionが、２マスの下側のマスにあることを仮定
        var p = this.GetPlayerPos(tr.position);

        RaycastHit hit;
        bool isHit = Physics.Raycast(
            isPressedShift ? tr.position + new Vector3(0, 1f) : tr.position, // Shift押してたら上、else 下 
            Vector3.right * (int)tr.localScale.x,
            out hit,
            1f);
        Debug.DrawRay( 
            isPressedShift ? tr.position + new Vector3(0, 1f) : tr.position, // Shift押してたら上、else 下 
            Vector3.right * (int)tr.localScale.x, Color.blue, 1f);

        if (isHit)
        {
            Debug.Log("当たり");
            // ブロックのときのみ壊す
            if (hit.transform.tag == "Block")
            {
                hit.transform.GetComponent<Animation>().Play();

                float alpha = 1f;
                var zero = hit.transform.FindChild("0");
                var one = hit.transform.FindChild("1");
                var two = hit.transform.FindChild("2");
                var three = hit.transform.FindChild("3");

                DOTween.To(() => alpha, (x) => alpha = x, 0f, 0.5f)
                    .OnUpdate(() => 
                    {
                        var color = new Color(1, 1, 1, alpha);
                        zero.GetComponent<SpriteRenderer>().color = color;
                        one.GetComponent<SpriteRenderer>().color = color;
                        two.GetComponent<SpriteRenderer>().color = color;
                        three.GetComponent<SpriteRenderer>().color = color;
                    })
                    .OnComplete(() =>
                    {
                        Destroy(hit.transform.gameObject);
                    });
            }
        }
    }

    // カメラの左上のワールド座標を求める
    private Vector3 getScreenTopLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft;
    }

    // カメラの右下のワールド座標を求める
    private Vector3 getScreenBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight;
    }
}
