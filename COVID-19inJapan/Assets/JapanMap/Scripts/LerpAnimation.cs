using System;
using UnityEngine;

public class LearpAnimation
{
    private float speed = 1.0f;
    private float time = 0.0f;
    private Type type;
    private dynamic target, start, current;

    public LearpAnimation(dynamic init)
    {
        target = start = current = init;
        type = init.GetType();
    }

    public void Set<T>(T data, float speed)
    {
        if (typeof(T) != type) throw new Exception("Type Error");
        start = current;
        target = data;
        time = 0.0f;
        this.speed = speed;
    }

    public dynamic Update()
    {
        if (typeof(float) == type) current = Mathf.Lerp(start, target, updateTime());
        if (typeof(Vector3) == type) current = Vector3.Lerp(start, target, updateTime());
        if (typeof(Quaternion) == type) current = Quaternion.Lerp(start, target, updateTime());
        return current;
    }

    private float updateTime()
    {
        time += Time.deltaTime;
        if (time > speed) time = speed;
        return (speed != 0.0f) ? time / speed : 1.0f;
    }
}
