using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

	[SerializeField]
	private bool m_IsHidden = false;
	[SerializeField]
	private GameObject m_Obj;
	private bool m_IsQuitting = false;

	private Vector3 m_FirstPosition;
    private GameObject m_Instance;

	// Use this for initialization
	void Start () {
		m_FirstPosition = this.transform.position;

		if (m_IsHidden && !m_IsQuitting) {
			Vector3 pos = this.transform.position;
			m_Instance = Instantiate(m_Obj, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;

            m_Instance.SetActive(false);
            GameManager.I.AddGameObject(m_Instance, GameManager.Type.Static);
		}
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
		if (m_IsHidden && !m_IsQuitting) {
            if(m_Instance != null)
                m_Instance.SetActive(true);
		}
	}

}
