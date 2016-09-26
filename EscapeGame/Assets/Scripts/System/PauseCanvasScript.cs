using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseCanvasScript : SingletonMonoBehaviour<PauseCanvasScript> {

	[SerializeField]
	private GameObject ResetUI;
	[SerializeField]
	private GameObject GameOverUI;

	static bool isPausing = false;

	void Awake()
	{
		if (this != I)
		{
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		if (ResetUI == null) {
			ResetUI = GameObject.Find ("Reset");
		}
		if (GameOverUI == null) {
			GameOverUI = GameObject.Find ("GameOver");
		}


		ResetUI.transform.FindChild("ResetButton").GetComponent<Button>().onClick.AddListener (Reset);
		ResetUI.transform.FindChild("CancelButton").GetComponent<Button>().onClick.AddListener (PauseCancel);
		GameOverUI.transform.FindChild("RetryButton").GetComponent<Button>().onClick.AddListener (Reset);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) && !PauseCanvasScript.isPausing)
        {
			ResetPause ();
        }
	}

	void ResetPause(){
		PauseCanvasScript.isPausing = true;
		Time.timeScale = 0;
		ResetUI.transform.FindChild ("ResetButton").GetComponent<Button> ().Select ();
		ResetUI.SetActive (true);
	}

	public void GameOver(){
		PauseCanvasScript.isPausing = true;
		Time.timeScale = 0;
		GameOverUI.transform.FindChild ("RetryButton").GetComponent<Button> ().Select ();
		GameOverUI.SetActive (true);
	}

	public void Reset(){
		GameManager.I.CallReset();
		PauseCancel ();
	}

	public void PauseCancel(){
		StartCoroutine (PauseCancelCoroutine ());
	}



	private IEnumerator PauseCancelCoroutine() {  
		Time.timeScale = 1;
		ResetUI.SetActive (false);
		GameOverUI.SetActive (false);

		yield return new WaitForSeconds (1.0f);  
		PauseCanvasScript.isPausing = false;
	}  
}
