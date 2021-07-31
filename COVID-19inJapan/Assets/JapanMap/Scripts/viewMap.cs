using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class viewMap : MonoBehaviour
{
    [SerializeField] private Transform prefectureRoot;
    [SerializeField] private TextMeshProUGUI frontData;
    [SerializeField] private Transform mapRoot;

    private List<Renderer> renderers = new List<Renderer>();
    private Dictionary<string, viewPrefectures> viewPrefectures = new Dictionary<string, viewPrefectures>();

    private LearpAnimation lerpModelAlpha = new LearpAnimation(0.0f);
    private LearpAnimation lerpFontAlpha = new LearpAnimation(0.0f);
    private LearpAnimation lerpPos = new LearpAnimation(Vector3.zero);
    private LearpAnimation lerpRot = new LearpAnimation(Quaternion.identity);
    private LearpAnimation lerpScale = new LearpAnimation(Vector3.one);

    public void SetFrontData(string data) => frontData.text = data;

    public void SetPrefectureData(string key, float data, float speed, float max)
    {
        viewPrefectures.Where(item => item.Key.IndexOf(key) >= 0).FirstOrDefault().Value?.SetData(data, speed, max);
    }

    public void SetAlpha(float alpha, float speed)
    {
        SetModelAlpha(alpha, speed);
        SetFontAlpha(alpha, speed);
    }

    public void SetModelAlpha(float alpha, float speed) => lerpModelAlpha.Set(alpha, speed);

    public void SetFontAlpha(float alpha, float speed) => lerpFontAlpha.Set(alpha, speed);

    public void SetTransform(Transform t, float speed)
    {
        lerpPos.Set(t.localPosition, speed);
        lerpRot.Set(t.localRotation, speed);
        lerpScale.Set(t.localScale, speed);
    }

    public void Init()
    {
        for (int i = 0; i < prefectureRoot.childCount; i++)
        {
            var child = prefectureRoot.GetChild(i);
            var prefecture = child.GetComponent<viewPrefectures>();
            prefecture.Init();
            viewPrefectures.Add(child.name, prefecture);
            renderers.Add(prefecture.GetRenderer());
        }

        for (int i = 0; i < mapRoot.childCount; i++)
        {
            var child = mapRoot.GetChild(i);
            var renderer = child.GetComponent<Renderer>();
            renderers.Add(renderer);
        }
    }

    public void UpdateData()
    {
        // set alpha
        frontData.color = new Color(frontData.color.r, frontData.color.g, frontData.color.b, lerpFontAlpha.Update());
        var current = lerpModelAlpha.Update();
        foreach (var item in renderers)
        {
            item.material.color = new Color(item.material.color.r, item.material.color.g, item.material.color.b, current);
        }
        // set position
        transform.localPosition = lerpPos.Update();
        transform.localRotation = lerpRot.Update();
        transform.localScale = lerpScale.Update();
        foreach (var item in viewPrefectures.Values) item.UpdateData();
    }
}
