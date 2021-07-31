using UnityEngine;

public class viewPrefectures : MonoBehaviour
{
    private Transform cube;
    private float maxSize = 2.0f;
    private LearpAnimation lerp = new LearpAnimation(0.0f);

    public void Init()
    {
        cube = transform.GetChild(0);
        var scale = cube.localScale;
        scale.y = 0.0f;
        cube.localScale = scale;
    }

    public void UpdateData()
    {
        var current = lerp.Update();
        cube.localScale = new Vector3(cube.localScale.x, current * maxSize, cube.localScale.z);
        cube.localPosition = new Vector3(cube.localPosition.x, current * maxSize / 2.0f, cube.localPosition.z);
    }

    public void SetData(float prefecture, float speed, float maxSize)
    {
        lerp.Set(prefecture, speed);
        this.maxSize = maxSize;
    }

    public Renderer GetRenderer() => transform.GetChild(0).GetComponent<Renderer>();
}
