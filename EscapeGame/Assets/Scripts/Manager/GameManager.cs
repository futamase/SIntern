using UnityEngine;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // ステージを生成する矩形の左上の点の座標
    public Vector3 m_GeneratePoint;

    private int m_Row = 0;
    private int m_Col = 0;

    private float m_OffsetX, m_OffsetY;

    // 今何ステージ目か
    private int m_StageCount = 1;

    // ステージ毎のコンボ数
    private int[] m_ComboList = new int[7];


    private GameObject m_StaticObjectsParent;
    private GameObject m_DynamicObjectsParent;

    private GameObject m_Prelude;

    // UI テキスト
    private Text m_FloorCountText, m_ComboText;

    public enum Type
    {
        Static, Dynamic
    }

    // コンボ公開用
    public int m_Combo
    {
        get
        {
            return m_ComboList[m_StageCount - 1];
        }
    }

    void Awake()
    {
        if (this != I)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < 7; i++)
            m_ComboList[i] = 0;

        m_Prelude = transform.Find("Prelude").gameObject;
        m_Prelude.SetActive(false);
    }

    // オブジェクトを配置
    private void SetBlock()
    {
        CSVReader reader = new CSVReader();
        if (!reader.LoadFile("Stages/stage" + m_StageCount.ToString()))
            return;

        var data = reader.GetData();

        var commonData = Resources.Load<CommonDataSet>("CommonDataSet");

        m_GeneratePoint = commonData.m_GeneratePoints[m_StageCount - 1];//m_GeneratePoint;//this.getScreenTopLeft();
        var topLeft = m_GeneratePoint;

        m_Row = (int)commonData.m_StageSize[m_StageCount - 1].y;
        m_Col = (int)commonData.m_StageSize[m_StageCount - 1].x;

        m_StaticObjectsParent = m_StaticObjectsParent ?? new GameObject("StaticParent");
        m_DynamicObjectsParent = m_DynamicObjectsParent ?? new GameObject("DynamicParent");

        for (int i = 0; i < m_Row; i++)
        {
            for (int j = 0; j < m_Col; j++)
            {
                if (data[i][j] != "")
                {
                    GameObject obj = Resources.Load("Prefabs/" + data[i][j]) as GameObject;
                    var go = Instantiate(obj,
                         new Vector3(topLeft.x + j + 0.5f, topLeft.y - (i + 0.5f)),
                         Quaternion.identity) as GameObject;
                    this.AddGameObject(go, Type.Static);
                }
            }
        }
        GenerateWallAndCelling(topLeft, m_Row, m_Col);

    }

    void GenerateWallAndCelling(Vector3 topLeft, int m_Row, int m_Col)
    {
        //Celling
        for (int i = 0; i < m_Col; i++)
        {
            GenerateWall(topLeft.x + i + 0.5f, topLeft.y + 2 - (-1 + 0.5f));
        }
        //Wall
        for (int i = -1; i < m_Row + 3; i++)
        {
            GenerateWall(topLeft.x - 1 + 0.5f, topLeft.y  + 2 - (i + 0.5f));
            GenerateWall(topLeft.x + m_Col + 0.5f, topLeft.y + 2 - (i + 0.5f));
        }

    }

    void GenerateWall(float x, float y)
    {
        GameObject _obj = Resources.Load("Prefabs/Wall") as GameObject;
        var _go = Instantiate(_obj,
            new Vector3(x, y),
            Quaternion.identity) as GameObject;
        this.AddGameObject(_go, Type.Static);

    }

    private void UpdateUI()
    {

    }

    public void CallReset()
    {
//        StartCoroutine(ResetStage());
        
        m_ComboList[m_StageCount - 1] = 0; // コンボ数をリセット
        m_StaticObjectsParent = null;
        m_DynamicObjectsParent = null;

        FadeManager.I.Fade(0.5f, () =>
         {
             SceneManager.LoadScene("Scene" + m_StageCount.ToString());
         });
    }

    // ステージをリセットする
    System.Collections.IEnumerator ResetStage()
    {
        if (m_StaticObjectsParent == null || m_DynamicObjectsParent == null)
            yield return null;

        m_ComboList[m_StageCount - 1] = 0; // コンボ数をリセット

        Destroy(m_StaticObjectsParent);
        m_StaticObjectsParent = null;

        Destroy(m_DynamicObjectsParent);
        m_DynamicObjectsParent = null;

        yield return new WaitForSeconds(0.5f);

        this.SetBlock();
        GameObject.FindObjectOfType<PrincessScript>().Reset();
        GameObject.FindObjectOfType<RobotScript>().Reset();
    }

    // Use this for initialization
    void Start()
    {
        // FadeManagerの設計ミスによるダメな実装
        FadeManager.I.Reset();
        this.SetBlock();

        SceneManager.sceneLoaded += this.CreateStage;

        var prefab = Resources.Load("Prefabs/StageInfoCanvas") as GameObject;
        var go = Instantiate(prefab);
//        m_FloorCountText = go.transform.FindChild("FloorCount").GetComponent<Text>();
        m_ComboText = go.transform.FindChild("ActCount").GetComponent<Text>();

//        m_FloorCountText.text = "Floor " + (8-m_StageCount).ToString();

        m_Prelude.SetActive(true);
        Time.timeScale = 0f;
        m_Prelude.transform.FindChild("Text").GetComponent<Text>().text = "Floor " + (8 - m_StageCount).ToString();
        var image = m_Prelude.transform.FindChild("RawImage").GetComponent<RawImage>();
        image.DOColor(new Color(0, 0, 0, 0), 2f)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                Time.timeScale = 1f;
                m_Prelude.SetActive(false);
                image.color = new Color(0, 0, 0, 0.5f);
            });
    }

    // ロードシーンのフロア名を管理しつつ、アニメーションが終わり次第次のステージへ
    void SetLoadScene()
    {
        GameObject.Find("Canvas").transform.FindChild("Floor").GetComponent<Text>().text =
            "Floor " + (7 - (m_StageCount)).ToString();
        float dummy = 0;
        DOTween.To(() => dummy, (x) => dummy = x, 1, 3f)
            .OnComplete(() =>
            {
                FadeManager.I.Fade(1f, this.GotoNextStage);
            });
    }

    // ステージシーンでステージを生成する(ステージ間移動シーン等はこれを呼んではいけない)
    void CreateStage(Scene scene, LoadSceneMode mode)
    {
        // FadeManagerの設計ミスによるダメな実装
        FadeManager.I.Reset();

        // TODO : シーンによってはこれを呼んではいけないので条件分岐する
        if (scene.name == "LoadScene")
        {
            this.SetLoadScene();
            return;
        }
        else if (scene.name == "Ending")
            return;

        this.SetBlock();

        var prefab = Resources.Load("Prefabs/StageInfoCanvas") as GameObject;
        var go = Instantiate(prefab);
//        m_FloorCountText = go.transform.FindChild("FloorCount").GetComponent<Text>();
        m_ComboText = go.transform.FindChild("ActCount").GetComponent<Text>();

//        m_FloorCountText.text = "Floor " + (8-m_StageCount).ToString();

        SoundManager.I.PlayBGM("title");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(this.ResetStage());
        }*/

        #region ステージショートカット
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_StageCount = 1;
            SceneManager.LoadScene("LoadScene");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            m_StageCount = 2;
            SceneManager.LoadScene("LoadScene");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            m_StageCount = 3;
            SceneManager.LoadScene("LoadScene");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            m_StageCount = 4;
            SceneManager.LoadScene("LoadScene");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            m_StageCount = 5;
            SceneManager.LoadScene("LoadScene");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            m_StageCount = 6;
            SceneManager.LoadScene("LoadScene");
        }
        #endregion

        if (m_ComboText != null)
            m_ComboText.text = "Act " + m_ComboList[m_StageCount - 1].ToString();
    }

    // ブロックの中から鍵とかブロックの中からスイッチとかした時にこれで登録したい
    public void AddGameObject(GameObject go, Type type)
    {
        if (m_StaticObjectsParent == null)
            return;

        if (type == Type.Static)
        {
            go.transform.parent = m_StaticObjectsParent.transform;
            return;
        }

        if (m_DynamicObjectsParent == null)
            return;

        if (type == Type.Dynamic)
        {
            go.transform.parent = m_DynamicObjectsParent.transform;
        }
    }

    public void GotoEnding()
    {
        FadeManager.I.color = Color.white;
        FadeManager.I.Fade(4f, () =>
        {
            SceneManager.LoadScene("Ending");
        });
    }

    // 次のステージへ
    private void GotoNextStage()
    {
        m_StageCount++;
        m_StaticObjectsParent = null;
        m_DynamicObjectsParent = null;
        m_ComboText = null;
//        m_FloorCountText = null;
        SceneManager.LoadScene("Scene" + m_StageCount.ToString());

        m_Prelude.SetActive(true);
        Time.timeScale = 0f;
        m_Prelude.transform.FindChild("Text").GetComponent<Text>().text = "Floor " + (8 - m_StageCount).ToString();
        var image = m_Prelude.transform.FindChild("RawImage").GetComponent<RawImage>();
        image.DOColor(new Color(0, 0, 0, 0), 2f)
            .SetEase(Ease.InExpo)
            .OnComplete(() =>
            {
                Time.timeScale = 1f;
                m_Prelude.SetActive(false);
                image.color = new Color(0, 0, 0, 0.5f);
            });
    }

    // 左端点を可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(m_GeneratePoint, 0.1f);
    }

    // 対象のタイル的位置を返す
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

    // for put key by Princess
    public Vector3 GetGeneratePos(Vector3 pos)
    {
        Int2 p = GetPlayerPos(pos);
        return new Vector3(m_GeneratePoint.x + p.x + 0.5f, m_GeneratePoint.y - p.y - 0.5f);

    }

    // 姫さまアクション
    public bool ActionHime(Transform tr)
    {

        RaycastHit hit;
        bool isHit = Physics.Raycast(tr.position, Vector3.right * (int)tr.localScale.x, out hit, 0.7f);
        Debug.DrawRay(tr.position, Vector3.right * (int)tr.localScale.x, Color.blue, 0.7f);
		if (isHit)
            return false;


        var p = this.GetPlayerPos(tr.position);
        // 範囲外チェック
        if (p.x == -1 || p.y == -1)
            return false;

        // 生成する
        var x = p.x + (int)tr.localScale.x;

        // 生成する位置が範囲外だったらダメ
        if (x == -1 || x == m_Col)
            return false;

        Vector3 wannaBePos = new Vector3(
            m_GeneratePoint.x + p.x + 0.5f,
            tr.position.y);
        // m_GeneratePoint.y - p.y - 0.5f);

        tr.DOLocalMove(wannaBePos, 0.5f)
            .OnComplete(() =>
            {
				SoundManager.I.PlaySE("creating_block");
                Vector3 genePos = new Vector3(m_GeneratePoint.x + x + 0.5f, m_GeneratePoint.y - p.y - 0.5f);
                GameObject obj = Resources.Load("Prefabs/Block") as GameObject;
                var instance = Instantiate(obj, genePos, Quaternion.identity) as GameObject;
                this.AddGameObject(instance, Type.Dynamic);
            });
        m_ComboList[m_StageCount - 1]++;

        return true;
    }

    // ロボアクション
    public void ActionRobo(Transform tr, bool isPressedShift)
    {
        // transform.positionが、２マスの下側のマスにあることを仮定
        var p = this.GetPlayerPos(tr.position);

        RaycastHit hit;
        bool isHit = Physics.Raycast(
            isPressedShift ? tr.position + new Vector3(0, 0.5f) : tr.position - new Vector3(0, 0.5f), // Shift押してたら上、else 下 
            Vector3.right * (int)tr.localScale.x,
            out hit,
            0.7f);
        Debug.DrawRay(
            isPressedShift ? tr.position + new Vector3(0, 0.5f) : tr.position - new Vector3(0, 0.5f), // Shift押してたら上、else 下 
            Vector3.right * (int)tr.localScale.x, Color.blue, 0.7f);

        if (isHit)
        {
            // ブロックのときのみ壊す
            if (hit.transform.tag == "Block")
            {
                float alpha = 1f;

                hit.transform.GetComponent<SpriteRenderer>().enabled = false;
                hit.transform.GetComponent<BoxCollider>().enabled = false;
                var particle = hit.transform.GetComponentInChildren<ParticleSystem>();

                var shine = hit.transform.FindChild("shine_effect");
                if (shine != null)
                {
                    shine.gameObject.SetActive(false);
                }
                SoundManager.I.PlaySE("brick_crash");
                particle.Play();
                DOTween.To(() => alpha, (x) => alpha = x, 0, 1f)
                    .OnComplete(() =>
                    {
                        Destroy(hit.transform.gameObject);
                    });
            }
            else if(hit.transform.tag == "Enemy")
            {
                hit.transform.DOScale(0f, 1.5f)
                    .SetEase(Ease.InExpo)
                    .OnComplete(() =>
                    {
                        Destroy(hit.transform.gameObject);
                    });
            }
            else if(hit.transform.tag == "Boss")
            {
                hit.transform.GetComponent<BossScript>().Damage();
            }

            m_ComboList[m_StageCount - 1]++;
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
