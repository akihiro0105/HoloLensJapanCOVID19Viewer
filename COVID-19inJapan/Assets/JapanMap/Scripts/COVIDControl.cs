using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class COVIDControl : MonoBehaviour
{
    [Header("map data")]
    [SerializeField] private viewMap startMap;
    [SerializeField] private viewMap countMap;
    [SerializeField] private viewMap rateMap;
    [Header("move point")]
    [SerializeField] private Transform hidePoint;
    [SerializeField] private Transform nextHidePoint;
    [SerializeField] private Transform centerPoint;
    [Header("back view")]
    [SerializeField] private backPanel backView;
    [Space(14)]
    [SerializeField] [Range(1, 1000)] private int switchMillisecSpeed = 200;
    [SerializeField] [Range(0.0f, 2.0f)] private float animationSpeed = 0.5f;

    private getCOVIDdata covidList = new getCOVIDdata();
    private getJapandata csvList = new getJapandata();

    private delegate KeyValuePair<string, float> PrefectureAction(string name, IGrouping<string, getCOVIDdata.Rootobject> prefecture);
    private bool stopAutoRun = false;

    // Start is called before the first frame update
    async void Start()
    {
        startMap.Init();
        countMap.Init();
        rateMap.Init();
        backView.Init();
        // start
        await startAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        startMap.UpdateData();
        countMap.UpdateData();
        rateMap.UpdateData();
    }

    public async void AutoRunDataCount()
    {
        await nextMapAnimation(rateMap, countMap, "接種数日次推移");
        await autoRun(countMap, (name, prefecture) =>
         {
             var raw = prefecture.Sum(item => item.count);
             var data = (float)raw / 100000.0f;
             return new KeyValuePair<string, float>(raw.ToString("N0") + "回", data);
         });
    }

    public async void AutoRunDataRate()
    {
        await nextMapAnimation(countMap, rateMap, "接種率日次推移");
        var totalList = new Dictionary<string, int>();
        await autoRun(rateMap, (name, prefecture) =>
         {
             var all = csvList.list.Where(item => item.prefecture == prefecture.Key).Sum(item => item.count);
             if (!totalList.ContainsKey(prefecture.Key)) totalList.Add(prefecture.Key, 0);
             totalList[prefecture.Key] += prefecture.Where(item => item.status == 1).Sum(item => item.count);
             var data = (float)totalList[prefecture.Key] / all;
             return new KeyValuePair<string, float>((data * 100).ToString("N") + "%", data);
         });
    }

    public void StopAutoRun() => stopAutoRun = true;

    private async Task startAnimation()
    {
        startMap.SetFrontData("");
        countMap.SetFrontData("");
        rateMap.SetFrontData("");
        startMap.SetTransform(centerPoint, 0.0f);
        countMap.SetTransform(hidePoint, 0.0f);
        rateMap.SetTransform(hidePoint, 0.0f);
        startMap.SetAlpha(0.5f, 0.0f);
        countMap.SetAlpha(0.0f, 0.0f);
        rateMap.SetAlpha(0.0f, 0.0f);
        await Task.Delay(1000);
        startMap.SetFrontData("データ受信中");
        await Task.Delay(1000);
        csvList.GetJapandata();
        var wait = Task.Delay(1000);
        var main = covidList.GetCOVIDdata();
        await Task.WhenAll(wait, main);
        startMap.SetFrontData("データ読み込み完了");
        await Task.Delay(1000);
        startMap.SetFrontData("待機中");
    }

    private async Task nextMapAnimation(viewMap currentMap, viewMap nextMap, string label)
    {
        startMap.SetAlpha(0.0f, 0.2f);
        startMap.SetTransform(nextHidePoint, 0.2f);
        currentMap.SetFrontData("");
        currentMap.SetAlpha(0.0f, 0.2f);
        currentMap.SetTransform(hidePoint, 0.2f);
        nextMap.SetFrontData("");
        nextMap.SetAlpha(0.5f, 0.2f);
        nextMap.SetTransform(centerPoint, 0.2f);
        await Task.Delay(200);
        startMap.SetTransform(centerPoint, 0.0f);
        startMap.SetModelAlpha(0.0f, 0.0f);
        startMap.SetFontAlpha(0.5f, 0.0f);
        startMap.SetFrontData(label);
        await Task.Delay(1000);
        await Task.Delay(1000);
        startMap.SetAlpha(0.0f, 1.0f);
        startMap.SetTransform(nextHidePoint, 0.0f);
    }

    private async Task autoRun(viewMap targetMap, PrefectureAction action)
    {
        stopAutoRun = false;
        var dic = new Dictionary<string, string>();
        // 日付の古い順に整列
        var dateList = covidList.list.Select(item => item.date).Distinct().OrderBy(item => item);
        foreach (var date in dateList)
        {
            targetMap.SetFrontData(date);
            // 都道府県毎に分類
            var list = covidList.list.Where(item => item.date.Equals(date)).GroupBy(item => item.prefecture);
            foreach (var item in list)
            {
                var name = csvList.list.Where(csv => csv.prefecture == item.Key).First().name;
                var nameData = action?.Invoke(name, item);
                targetMap.SetPrefectureData(item.Key, nameData.Value.Value, animationSpeed, 2.0f);
                if (!dic.ContainsKey(name)) dic.Add(name, null);
                dic[name] = nameData.Value.Key;
                backView.SetDictionaryText(dic);
            }
            await Task.Delay(switchMillisecSpeed);
            if (stopAutoRun)
            {
                stopAutoRun = false;
                break;
            }
        }
    }
}
