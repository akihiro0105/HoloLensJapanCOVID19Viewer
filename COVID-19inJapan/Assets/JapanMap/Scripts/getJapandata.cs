using System;
using System.Collections.Generic;
using System.IO;

public class getJapandata
{
    public List<CSVobject> list { get; private set; }

    public void GetJapandata()
    {
        list = new List<CSVobject>();
        var obj = UnityEngine.Resources.Load("000701583J") as UnityEngine.TextAsset;
        var reader = new StringReader(obj.text);
        while (true)
        {
            var line = reader.ReadLine();
            if (line == null) break;
            var split = line.Split(',');
            list.Add(new CSVobject()
            {
                prefecture = split[0],
                name = split[1],
                gender = split[2],
                count = int.Parse(split[3]),
                count_age = int.Parse(split[4]),
            });
        }
    }

    [Serializable]
    public class CSVobject
    {
        public string prefecture;
        public string name;
        public string gender;
        public int count;
        public int count_age;
    }
}
