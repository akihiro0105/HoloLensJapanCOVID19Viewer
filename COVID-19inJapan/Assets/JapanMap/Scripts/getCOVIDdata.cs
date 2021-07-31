using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class getCOVIDdata
{
    public List<Rootobject> list { get; private set; }

    private static readonly HttpClient httpClient = new HttpClient();

    public async Task GetCOVIDdata()
    {
        var uri = "https://vrs-data.cio.go.jp/vaccination/opendata/latest/prefecture.ndjson";
        var gzipFile = @"..\data.gzip";
        var jsonFile = @"..\data.json";
#if WINDOWS_UWP
        gzipFile = Path.Combine(UnityEngine.Application.persistentDataPath, "data.gzip");
        jsonFile = Path.Combine(UnityEngine.Application.persistentDataPath, "data.json");
#endif
        await DownloadFile(uri, gzipFile);
        await UnpackFile(gzipFile, jsonFile);
        var lines = await ReadFile(jsonFile);
        list = LoadFile(lines);
    }

    #region Data import
    // Download gzip
    private async Task DownloadFile(string downloadURL, string gzipPath)
    {
        var request = await httpClient.GetStreamAsync(downloadURL);
        using (var file = new FileStream(gzipPath, FileMode.Create, FileAccess.Write))
        {
            await request.CopyToAsync(file);
        }
    }

    // Unpack gzip to json
    private async Task UnpackFile(string gzipPath, string jsonPath)
    {
        using (var file = new FileStream(gzipPath, FileMode.Open, FileAccess.Read))
        {
            var gzip = new GZipStream(file, CompressionMode.Decompress);
            using (var jsonData = new FileStream(jsonPath, FileMode.Create, FileAccess.Write))
            {
                await gzip.CopyToAsync(jsonData);
            }
        }
    }

    // Read json
    private async Task<string[]> ReadFile(string jsonPath)
    {
        using (var jsonData = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            using (var data = new StreamReader(jsonData))
            {
                var alldata = await data.ReadToEndAsync();
                return alldata.Split(new[] { '\n', '\r' });
            }
        }
    }

    // Load json
    private List<Rootobject> LoadFile(string[] lines)
    {
        var list = new List<Rootobject>();
        foreach (var item in lines)
        {
            var ms = UnityEngine.JsonUtility.FromJson<Rootobject>(item);
            list.Add(ms);
        }
        return list.Where(item => item != null).ToList();
    }
    #endregion

    [Serializable]
    public class Rootobject
    {
        public string date;
        public string prefecture;
        public string gender;
        public string age;
        public bool medical_worker;
        public int status;
        public int count;
    }
}
