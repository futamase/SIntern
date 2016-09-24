using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	private GameObject m_Princess;
	private GameObject m_Robot;
	private bool m_IsUsingPrincess = false;

	// Use this for initialization
	void Start () {
		m_Princess = GameObject.Find ("Princess");
		m_Robot = GameObject.Find ("Robot");
		SetUseCharacter ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Change")) {
			SetUseCharacter ();
		}
	
	}

	private void SetUseCharacter(){
		m_IsUsingPrincess = !m_IsUsingPrincess;
		m_Princess.GetComponent<PrincessScript> ().ChangeCharacter (m_IsUsingPrincess);
		m_Robot.GetComponent<RobotScript> ().ChangeCharacter (!m_IsUsingPrincess);
	}
}
