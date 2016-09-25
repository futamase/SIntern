using UnityEngine;
using System.Collections;

public class PlayerBaseScript : MonoBehaviour {

	//public Vector2 m_SPEED = new Vector2(0.05f, 0.05f);
	private CharacterController m_Controller;
	public GameManagerScript m_GameManagerScript;
	private Vector3	m_Move = Vector3.zero;
	private float m_Speed = 5.0f;
	private const float	GRAVITY = 20f;			// 重力
	private bool m_IsUsing = true;
	private bool m_IsAlive = true;
	private Vector3 m_firstPosition;


	// Use this for initialization
	public void Start () {
		m_Controller = GetComponent<CharacterController>();
		m_GameManagerScript = GameObject.Find ("GameManager").GetComponent<GameManagerScript> ();
		m_firstPosition = this.transform.position;

	}
	
	// Update is called once per frame
	public void Update () {
		Move ();
		if (this.m_IsUsing && Input.GetButtonDown ("Action")) {
			Action ();
		}
		if (Input.GetButtonDown ("Change")) {
			ChangeCharacter (!m_IsUsing);
		}
	}

	void Move(){
		float x = 0.0f;
		if (this.m_IsUsing) {
			x = Input.GetAxis ("Horizontal") * m_Speed;
		}
		Vector3 scale = this.transform.localScale;
		if (x < 0) {
			this.transform.localScale = new Vector3(-1*Mathf.Abs(scale.x), scale.y, 1); //左向く
		} else if(x > 0) {
			this.transform.localScale = new Vector3(1*Mathf.Abs(scale.x), scale.y, 1); //右向く
		}
		m_Move = new Vector3(x, m_Move.y , 0.0f);		// 左右上下のキー入力を取得し、移動量に代入.
		m_Move.y -=  GRAVITY * Time.deltaTime;	// 重力を代入.
		m_Controller.Move(m_Move * Time.deltaTime);	// キャラ移動.
	}

	public void ChangeCharacter(bool IsUsing){
		this.m_IsUsing = IsUsing;
	}

	public virtual void Action(){
		;
	}

	public void OnTriggerEnter(Collider other){
		if (other.transform.tag == "Lift") {
			this.Dead ();
		}
	}

	public void Dead(){
		Debug.Log ("Dead");
		this.m_IsAlive = false;
		Destroy (this.transform.gameObject);
	}

	public void Reset(){
		this.transform.position = m_firstPosition;
		this.m_IsAlive = true;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		string tag = hit.transform.tag;
		if (tag == "Block" || tag == "FixedBlock") {
			if (this.transform.position.y+0.3f > hit.transform.position.y) {
				if (m_Controller.stepOffset == 0) {
					this.m_Controller.stepOffset = 0.7f;
					this.m_Controller.slopeLimit = 90f;
				}
			} else {
				this.m_Controller.stepOffset = 0.0f;
				this.m_Controller.slopeLimit = 0;
			}
		}
	}
		
}
