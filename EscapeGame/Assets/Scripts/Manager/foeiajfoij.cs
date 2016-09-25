using UnityEngine;
using System.Collections;

public class foeiajfoij : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
            FadeManager.I.Fade(3f, () => { Debug.Log("Fade end!"); });
	}
}
