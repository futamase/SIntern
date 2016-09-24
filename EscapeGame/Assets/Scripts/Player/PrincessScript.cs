﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PrincessScript : PlayerBaseScript {

	//public GameObject m_Block;
	public bool m_HasKey;
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
		if (IsPushSwitch ()) {
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
		if (this.IsTouch(Key.transform.position)) {
			Destroy (Key);
			m_HasKey = true;
			return true;
		}
		return false;
	}

	private bool IsPushSwitch(){
		GameObject[] Switches = GameObject.FindGameObjectsWithTag ("Switch");
		if (Switches.Length <= 0) {
			return false;
		}
		foreach (GameObject o in Switches) {
			if (this.IsTouch(o.transform.position)) {
				o.SendMessage ("Push");
				return true;
			}
		}
		return false;
	}

	private bool IsTouch(Vector3 targetPos){
		float distance = this.transform.position.x - targetPos.x;
		if (this.transform.localScale.x > 0) {
			return -2 < distance && distance < 0;
		} else {
			return 0 < distance && distance < 2;
		}
	}

	private void PutKey(){
		Vector3 pos = this.transform.position;
		Instantiate(m_Key, new Vector3(pos.x+ 1, pos.y-0.5f, 0), Quaternion.identity);
		m_HasKey = false;
	}

	private void GenerateBlock(){
        //		Vector3 spriteSize = this.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().bounds.size;
        //		GameManagerScript.I.Action(this.transform, true, spriteSize);
        GameManager.I.ActionHime(this.transform);
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log (collision);
		switch (collision.transform.tag) {
		case "Enemy":
			Dead ();
			break;
		case "Lift":
			Dead ();
			break;
		case "Door":
			if (this.m_HasKey) {
				collision.transform.gameObject.SendMessage ("Goal");
			}
			break;
		default:
			break;
		}

	}

	void OnTriggerEnter(Collider other){
		base.OnTriggerEnter(other);
		if(other.transform.tag == "Door"){
			if (this.m_HasKey) {
				other.transform.gameObject.SendMessage ("Goal");
			}
		}
	}
	new public void Reset(){
		base.Reset ();
		m_HasKey = false;
		ChangeCharacter (true);
	}

}
