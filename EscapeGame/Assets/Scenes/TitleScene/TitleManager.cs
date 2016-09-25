using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

    bool m_IsAlreadyCalled = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (m_IsAlreadyCalled)
                return;
            m_IsAlreadyCalled = true;

            // ステージ１へ遷移
            FadeManager.I.Fade(1f, () =>
             {
                 SceneManager.LoadScene("Scene1");
             });
        }
	
	}
}
