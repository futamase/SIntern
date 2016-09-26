using UnityEngine;
using System.Collections.Generic;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField]
    private AudioClip[] m_BGM;
    [SerializeField]
    private AudioClip[] m_SE;

    private Dictionary<string, int> m_BGMDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> m_SEDictionary = new Dictionary<string, int>();

    private AudioSource m_BGMSource, m_SESource;

    void Awake()
    {
        if (this != I)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < m_BGM.Length; i++)
        {
            m_BGMDictionary.Add(m_BGM[i].name, i);
        }
        for (int i = 0; i < m_SE.Length; i++)
        {
            m_SEDictionary.Add(m_SE[i].name, i);
        }

        m_BGMSource = this.gameObject.AddComponent<AudioSource>();
        m_SESource = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(string name)
    {
        m_BGMSource.clip = m_BGM[m_BGMDictionary[name]];
        m_BGMSource.Play();
        m_BGMSource.loop = true;
    }

    public void StopBGM()
    {
        m_BGMSource.Stop();
    }

    public void PlaySE(string name)
    {
        m_SESource.PlayOneShot(m_SE[m_SEDictionary[name]]);
    }
}