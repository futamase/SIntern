using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {


	[SerializeField]
	private float m_Speed = 10f;
	private int m_Direction = 1;

	private float timeLeft = 0;
	private bool m_IsCollision = false;

	// Use this for initialization
	void Start () {
		m_Direction = -1;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft += Time.deltaTime;
		if (timeLeft > 0.5f) {
			m_IsCollision = false;
		}
		this.transform.position += new Vector3 (m_Direction * m_Speed, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log (collision.transform.tag);
		switch (collision.transform.tag) {
		case "Block":
			ChangeDirection ();
			break;
		case "FixedBlock":
			ChangeDirection ();
			break;
		case "Wall":
			ChangeDirection ();
			break;
		default:
			break;
		}
	}

	void ChangeDirection(){
		if(m_IsCollision){
			return;
		}
		m_Direction *= -1;
		Vector3 pos = this.transform.position;
		this.transform.position = new Vector3 (pos.x + m_Direction * m_Speed, pos.y, 0);
		this.transform.localScale = new Vector3 (this.transform.localScale.x * -1, 1, 1);
		m_IsCollision = true;
	}

	public void Reset(){
		Destroy (this.transform.gameObject);
	}
}
