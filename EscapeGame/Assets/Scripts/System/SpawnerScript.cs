using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	public const int ENEMY_MAX = 3;
	public const float GENERATE_TIME = 5.0f;
	private float m_LeftTime = 0.0f;
	public GameObject m_Enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_LeftTime += Time.deltaTime;
		if (m_LeftTime > GENERATE_TIME) {
			if (GameObject.FindGameObjectsWithTag ("Enemy").Length < ENEMY_MAX) {
				var go = Instantiate(m_Enemy, this.transform.position, Quaternion.identity) as GameObject;

                GameManager.I.AddGameObject(go, GameManager.Type.Dynamic);
			}
			m_LeftTime = 0;
		}
	}

	public void Reset(){
		m_LeftTime = 0.0f;
	}
}
