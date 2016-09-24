using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

	[SerializeField]
	private bool m_Haskey = false;
	[SerializeField]
	private GameObject m_Key;
	private bool m_IsQuitting = false;

	private Vector3 m_FirstPosition;


	// Use this for initialization
	void Start () {
		m_FirstPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset(){
		this.transform.position = m_FirstPosition;
	}

	void OnApplicationQuit ()
	{
		m_IsQuitting = true;
	}

	void OnDestroy(){
		if (m_Haskey && !m_IsQuitting) {
			Vector3 pos = this.transform.position;
			Instantiate(m_Key, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
		}
	}

}
