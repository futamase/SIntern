using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotScript : PlayerBaseScript {

	Animator animator;

	Vector3 m_beforePosition;

	// Use this for initialization
	new void Start () {
		base.Start ();
		ChangeCharacter (false);
		animator = GetComponent<Animator> ();

	}

	// Update is called once per frame
	new void Update () {
		base.Update ();
		float x = Input.GetAxis ("Horizontal");
		animator.SetBool ("IsWalking", Mathf.Abs(x)> 0 );
		m_beforePosition = this.transform.position;
	}

	public override void Action(){
		DestroyBlock ();
	}

	private void DestroyBlock(){
		animator.SetTrigger ("Crush");
        GameManager.I.ActionRobo(transform, Input.GetKey(KeyCode.LeftShift));
	}

//	void OnCollisionEnter(Collision collision) {
//		base.OnCollisionEnter (collision);
//		Debug.Log (collision);
//		switch (collision.transform.tag) {
//		case "Lift":
//			Dead ();
//			break;
//		default:
//			break;
//		}
//
//	}



	new public void Reset(){
		base.Reset ();
		ChangeCharacter (false);
	}

}
