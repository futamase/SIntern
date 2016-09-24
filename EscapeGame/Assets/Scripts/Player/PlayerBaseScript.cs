using UnityEngine;
using System.Collections;

public class PlayerBaseScript : MonoBehaviour {

	//public Vector2 m_SPEED = new Vector2(0.05f, 0.05f);
	private CharacterController m_Controller;
	private Vector3	m_Move = Vector3.zero;
	private float m_Speed = 5.0f;
	private const float	GRAVITY = 20f;			// 重力
	private bool m_IsUsing = true;


	// Use this for initialization
	public void Start () {
		m_Controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	public void Update () {
		Move ();
	}

	void Move(){
		float x = 0.0f;
		if (this.m_IsUsing) {
			x = Input.GetAxis ("Horizontal") * m_Speed;
		}
		m_Move = new Vector3(x, m_Move.y , 0.0f);		// 左右上下のキー入力を取得し、移動量に代入.
		m_Move.y -=  GRAVITY * Time.deltaTime;	// 重力を代入.
		m_Controller.Move(m_Move * Time.deltaTime);	// キャラ移動.
	}

	public void ChangeCharacter(bool IsUsing){
		this.m_IsUsing = IsUsing;
	}
}
