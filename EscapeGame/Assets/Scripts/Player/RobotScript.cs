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

//	private GameObject ChooseDestroyBlock(){
//		GameObject[] AllBlocks = GameObject.FindGameObjectsWithTag ("Block");
//		List<GameObject> CandidateBlocks = new List<GameObject> ();
//		bool IsRight = this.transform.localScale.x > 0;
//		float PosX = this.transform.position.x;
//		foreach (GameObject o in AllBlocks){
//			Vector3 BlockPos = o.transform.position;
//			if (IsRight && BlockPos.x < PosX) {
//				continue;
//			} else if(!IsRight && BlockPos.x > PosX){
//				continue;
//			}
//			if (Mathf.Abs (PosX - BlockPos.x) < 2) {
//				CandidateBlocks.Add (o);
//			}
//		}
//		if (CandidateBlocks.Count > 0){
//			return CandidateBlocks [0];
//		}else{
//			return null;
//		}
//		
//	}

}
