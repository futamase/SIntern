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
        //		Debug.Log(this.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().bounds.size.y);
//		m_GameManagerScript.Action (this.transform, false);
//		GameObject TargetBlock = ChooseDestroyBlock ();
//		if (TargetBlock) {
//			Destroy (TargetBlock);
//		}
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
