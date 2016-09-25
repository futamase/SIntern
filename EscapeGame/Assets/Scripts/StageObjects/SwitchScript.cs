using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

	[SerializeField]
	private GameObject m_TargetLift;
	[SerializeField]
	private int m_MoveDistance;

    [SerializeField]
	private bool m_isPushed = false;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		if (m_TargetLift == null) {
			m_TargetLift = GameObject.FindGameObjectWithTag ("Lift");
		}

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
		//animator.SetFloat ("Speed", -1.0f);
		animator.SetBool ("IsPush", m_isPushed);
	}

	public void Reset(){
		this.m_isPushed = false;
	}
}
