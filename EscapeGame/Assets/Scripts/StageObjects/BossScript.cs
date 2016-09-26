using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	private float m_timeLeft = 0.0f;
	const float ATTACK_TIME = 25.0f;

	private bool m_IsHidden = true;
	private GameObject m_Obj;
	private bool m_IsQuitting = false;

	private Vector3 m_FirstPosition;
	private GameObject m_Instance;

    private int m_HP = 2;

	// Use this for initialization
	void Start () {
		m_Obj = Resources.Load("Prefabs/Key") as GameObject;
		m_FirstPosition = this.transform.position;

		if (m_IsHidden && !m_IsQuitting) {
			Vector3 pos = this.transform.position;
			m_Instance = Instantiate(m_Obj, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;

			m_Instance.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_timeLeft += Time.deltaTime;
		if (m_timeLeft > ATTACK_TIME) {
			BossAttack ();
			m_timeLeft = 0.0f;
		}
	}

	void BossAttack(){
		StartCoroutine (BossAttackCoroutine ());
	}

	private IEnumerator BossAttackCoroutine() {
		SoundManager.I.PlaySE("boss_laughter");
		yield return new WaitForSeconds (6.0f);
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject o in blocks) {
			Destroy (o);
		}
	}

    public void Damage()
    {
        m_HP--;
        if(m_HP == 0)
        {
            Destroy(this.gameObject);
        }
    }

	void OnApplicationQuit ()
	{
		m_IsQuitting = true;
	}

	void OnDestroy(){
		if (m_IsHidden && !m_IsQuitting) {
			if(m_Instance != null)
				m_Instance.SetActive(true);
		}
	}
}
