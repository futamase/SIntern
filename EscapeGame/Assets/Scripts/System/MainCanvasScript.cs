using UnityEngine;
using System.Collections;

public class MainCanvasScript : SingletonMonoBehaviour<MainCanvasScript> {

	private GameObject ResetUI;
	private GameObject GameOverUI;

	// Use this for initialization
	void Start () {
		ResetUI = this.transform.FindChild ("Reset").gameObject;
		GameOverUI = this.transform.FindChild ("GameOver").gameObject;

		Debug.Log (ResetUI.activeSelf);
		Debug.Log (GameOverUI.activeSelf);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
        {
			ResetPause ();

        }
	}

	void ResetPause(){
		Time.timeScale = 0;
		ResetUI.SetActive (true);
	}

	void Reset(){
		GameManager.I.CallReset();
	}

	public void GameOver(){
		GameOverUI.SetActive (true);
	}

	void PauseCancel(){
		Time.timeScale = 1;
		ResetUI.SetActive (false);
		GameOverUI.SetActive (false);
	}
}
