using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	[SerializeField]
	private GameObject m_TargetLift;
	[SerializeField]
	private int m_MoveDistance;

	private bool m_isPushed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	
		if (Input.GetKeyDown (KeyCode.A)) {
			Push ();
		}
	}

	public void Push(){
		m_isPushed = !m_isPushed;
		m_TargetLift.GetComponent<LiftScript> ().Move (m_isPushed, m_MoveDistance);
	}

	public void Reset(){
		this.m_isPushed = false;
	}
}
