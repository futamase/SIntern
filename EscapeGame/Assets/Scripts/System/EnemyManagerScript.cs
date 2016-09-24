using UnityEngine;
using System.Collections;

public class EnemyManagerScript : MonoBehaviour {

	public const int ENEMY_MAX = 3;
	public const float GENERATE_TIME = 3.0f;
	private float m_LeftTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_LeftTime += Time.deltaTime;
		if (m_LeftTime > GENERATE_TIME) {

		}
	}
}
