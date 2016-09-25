using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

	//private bool m_IsOpen = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
		
	void Goal(){
		SoundManager.I.PlaySE("door_squeak");
        //this.m_IsOpen = true;
        SceneManager.LoadScene("LoadScene");
	}

	public void Reset(){
		//this.m_IsOpen = false;
	}
}
