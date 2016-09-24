using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {


	[SerializeField]
	private float m_Speed = 10f;
	private int m_Direction = 1;

	// Use this for initialization
	void Start () {
		m_Direction = -1;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += new Vector3 (m_Direction * m_Speed, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		switch (collision.transform.tag) {
		case "Block":
			m_Direction *= -1;
			this.transform.position += new Vector3 (m_Direction * m_Speed, 0, 0);
			//this.transform.localScale = new Vector3 (this.transform.localScale.x * -1, 1, 1);
			break;
		case "FixedBlock":
			m_Direction *= -1;
			this.transform.position += new Vector3 (m_Direction * m_Speed, 0, 0);
			//this.transform.localScale = new Vector3 (this.transform.localScale.x * -1, 1, 1);
			break;
		default:
			break;
		}
	}

	public void Reset(){
		Destroy (this.transform.gameObject);
	}
}
