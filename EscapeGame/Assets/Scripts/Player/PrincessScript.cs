using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PrincessScript : PlayerBaseScript {

	//public GameObject m_Block;
	private bool m_HasKey;
	public GameObject m_Key;

	// Use this for initialization
	new void Start () {
		base.Start ();
		m_HasKey = false;
		ChangeCharacter (true);
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	public override void Action(){
		if (IsGetKey ()) {
			return;
		}
		
		if (!m_HasKey) {
			GenerateBlock ();
		} else {
			PutKey ();
		}
	}

	private bool IsGetKey(){
		GameObject Key = GameObject.FindGameObjectWithTag ("Key");
		if (!Key) {
			return false;
		}
		//GetKey
		if (Mathf.Abs (Key.transform.position.x - this.transform.position.x) < 2) {
			Destroy (Key);
			m_HasKey = true;
			return true;
		}
		return false;
	}

	private void PutKey(){
		Vector3 pos = this.transform.position;
		Instantiate(m_Key, new Vector3(pos.x+ 1, pos.y-0.5f, 0), Quaternion.identity);
		m_HasKey = false;
	}

	private void GenerateBlock(){		
		Vector3 spriteSize = this.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().bounds.size;
		GameManagerScript.I.Action(this.transform, true, spriteSize);
	}

}
