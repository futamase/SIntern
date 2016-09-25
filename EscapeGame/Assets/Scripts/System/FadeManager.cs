using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : SingletonMonoBehaviour<FadeManager> 
{
    private float m_alpha = 0f;
    private Texture2D m_texture;
    private bool m_isEnable = false;

    void Awake()
    {
        if (this != I)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

		m_texture = new Texture2D( 1, 1, TextureFormat.ARGB32, false );
		m_texture.SetPixel(0,0, Color.white );
		m_texture.Apply();
    }

    void OnGUI()
    {
        if (!m_isEnable)
            return;
        GUI.color = new Color(0,0,0, m_alpha);
        GUI.DrawTexture(new Rect( 0, 0, Screen.width, Screen.height ), m_texture );
    }

    public void Reset()
    {
        m_isEnable = false;
        m_alpha = 0f;
    }

    public void Fade(float duration, Action callback)
    {
        if (m_isEnable)
            return;

        m_isEnable = true;
        m_alpha = 0;

        DOTween.To(() => m_alpha, (x) => m_alpha = x, 1f, duration)
            .OnComplete(()=> 
            {
                if (callback != null)
                    callback();
            });
    }
}

