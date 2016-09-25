using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainCanvasScript : SingletonMonoBehaviour<MainCanvasScript> {

	private GameObject ResetUI;
	private GameObject GameOverUI;

	// Use this for initialization
	void Start () {
		ResetUI = this.transform.FindChild ("Reset").gameObject;
		GameOverUI = this.transform.FindChild ("GameOver").gameObject;

		ResetUI.transform.FindChild("ResetButton").GetComponent<Button>().onClick.AddListener (() => Reset());
		ResetUI.transform.FindChild("CancelButton").GetComponent<Button>().onClick.AddListener (() => PauseCancel());
		GameOverUI.transform.FindChild("RetryButton").GetComponent<Button>().onClick.AddListener (() => Reset());

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
        {
			ResetPause ();
			//Reset();
        }
	}

	void ResetPause(){
		Time.timeScale = 0;
		ResetUI.SetActive (true);
	}

	void Reset(){
		PauseCancel ();
		GameManager.I.CallReset();
	}

	public void GameOver(){
		Time.timeScale = 0;
		GameOverUI.SetActive (true);
	}

	void PauseCancel(){
		Time.timeScale = 1;
		ResetUI.SetActive (false);
		GameOverUI.SetActive (false);
	}
}
