using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotScript : PlayerBaseScript {

	// Use this for initialization
	new void Start () {
		base.Start ();
		ChangeCharacter (false);

	}

	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	public override void Action(){
		DestroyBlock ();
	}

	private void DestroyBlock(){
        GameManager.I.ActionRobo(transform, Input.GetKey(KeyCode.LeftShift));
	}

	void OnCollisionEnter(Collision collision) {
		base.OnCollisionEnter (collision);
		Debug.Log (collision);
		switch (collision.transform.tag) {
		case "Lift":
			Dead ();
			break;
		default:
			break;
		}

	}


	new public void Reset(){
		base.Reset ();
		ChangeCharacter (false);
	}

}
