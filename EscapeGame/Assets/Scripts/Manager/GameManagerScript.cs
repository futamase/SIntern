using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	private GameObject m_Princess;
	private GameObject m_Robot;
	private bool m_IsUsingPrincess;

	// Use this for initialization
	void Start () {
		m_Princess = GameObject.Find ("Princess");
		m_Robot = GameObject.Find ("Robot");
		SetUseCharacter ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SetUseCharacter(){
		m_IsUsingPrincess = !m_IsUsingPrincess;
			m_Princess.GetComponent<PrincessScript> ().ChangeCharacter (m_IsUsingPrincess);
			m.GetComponent<PrincessScript> ().ChangeCharacter (m_IsUsingPrincess);
	}
}
