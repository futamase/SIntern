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
		float x = 0;
		if(this.m_IsUsing){
			x = Input.GetAxis ("Horizontal");
		}
		animator.SetBool ("IsWalking", Mathf.Abs(x)> 0 );
		m_beforePosition = this.transform.position;
	}

	public override void Action(){
		DestroyBlock ();
	}

	private void DestroyBlock(){
		StartCoroutine (DestroyBlockCoroutine (Input.GetKey(KeyCode.LeftShift)));
	}

	private IEnumerator DestroyBlockCoroutine(bool isPushShift) {
		animator.SetTrigger ("Crush");
		if (isPushShift) {
			yield return new WaitForSeconds (0.5f);
		} else {
			yield return new WaitForSeconds (0.4f);
		}
		GameManager.I.ActionRobo(transform, isPushShift);
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
