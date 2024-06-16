using System;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public Transform dock;

    public String type = "";

    public bool inHover = false;

    public MeshRenderer m;
    private Color initialColor;

    private HomeLocation h;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialColor = m.material.color;
        h = GameObject.FindFirstObjectByType<HomeLocation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m)
        {
            // if (inHover)
            // {
            //     m.material.color = Color.red;
            // }
            // else
            // {
            //     m.material.color = initialColor;
            // }
        }
    }

    public float DistanceToHome() {
        return Vector3.Distance(transform.position, h.transform.position);
    }
}
