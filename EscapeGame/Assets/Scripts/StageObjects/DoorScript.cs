using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	private bool m_IsOpen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
		

	void Goal(){
		this.m_IsOpen = true;
		Debug.Log ("GOOOOOOOOOOOOOOOOOOAL");
	}

	public void Reset(){
		this.m_IsOpen = false;
	}
}
