using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	private float m_timeLeft = 0.0f;
	const float ATTACK_TIME = 3.0f;

	// Use this for initialization
	void Start () {
	
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
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
		foreach (GameObject o in blocks) {
			Destroy (o);
		}
	}
}
