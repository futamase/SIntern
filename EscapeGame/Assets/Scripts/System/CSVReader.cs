using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CSVReader{

    private TextAsset m_textAsset;

    List<List<string>> m_data = new List<List<string>>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool LoadFile(string fileName)
    {
        TextAsset text = Resources.Load<TextAsset>(fileName);
        if (text == null)
            return false;
        TextReader reader = new StringReader(text.text);

        if (text == null || reader == null)
            return false;

        int row = 0;
        string line = "";
        while((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            m_data.Add(new List<string>());

            for(int i =0; i < fields.Length; i++)
            {
                m_data[row].Add(fields[i]);
            }
            row++;
        }

        Resources.UnloadAsset(text);
        text = null;
        Resources.UnloadUnusedAssets();

        return true;
    }

    public List<List<string>> GetData()
    {
        return m_data;
    }
}
