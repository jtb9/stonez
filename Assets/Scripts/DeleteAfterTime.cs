using System;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    public int lifeInMilliseconds = 500;
    DateTime spawnTime = DateTime.Now;

    void Update()
    {
        if ((DateTime.Now - spawnTime).TotalMilliseconds >= lifeInMilliseconds) {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}
