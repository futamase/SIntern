using UnityEngine;
using System.Collections;

public class LiftScript : MonoBehaviour {

	private Vector3 m_firstPosition;

	private bool m_IsCollision;
	private bool m_IsMoving;
	private bool m_IsDown;
	[SerializeField]
	private bool m_FirstDown = true;

	// Use this for initialization
	void Start () {
		this.m_firstPosition = this.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move(bool isDown, int distance){
		m_IsCollision = false;
		if (this.m_IsMoving) {
			return;
		}
		SoundManager.I.PlaySE("anchor_chain");
		StartCoroutine (MoveCoroutine (distance, isDown));
	}

	// コルーチン  
	private IEnumerator MoveCoroutine(float distance, bool isDown) {
		m_IsMoving = true;
		m_IsDown = isDown;
		float limit = 0;
		if (m_FirstDown) {
			limit = isDown ? m_firstPosition.y - distance : m_firstPosition.y;
		} else {
			limit = isDown ? m_firstPosition.y : m_firstPosition.y + distance;
		}
		float diff = isDown ? -0.1f : 0.1f;
		for (int i = 0; i < distance*10; i++) {
			if (m_IsCollision) {
				break;
			}
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + diff, 0);
			yield return new WaitForSeconds (0.1f);  
			if (isDown && this.transform.position.y <= limit) {
				break;
			} else if (!isDown && this.transform.position.y >= limit) {
				break;
			}
		}
		m_IsMoving = false;
		m_IsDown = false;
		yield return null;
	}

	void OnTriggerEnter(Collider other){
		m_IsCollision = true;
		//Debug.Log (other.transform.tag);
	}


	void OnCollisionEnter(Collision collision){
		m_IsCollision = true;
		//Debug.Log (collision.transform.tag);
	}

	public void Reset(){
		this.transform.position = this.m_firstPosition;
	}

	public bool IsLiftDown(){
		return this.m_IsDown;
	}
}
