using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractible : MonoBehaviour
{
    public bool hasActionAfterTalk = false;
    public String message = "";
    public String uniqueName = "";
    public GameObject questIndicator;
    
    public bool hasQuest = false;

    public List<DialogMessage> dialogOptions = new List<DialogMessage>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
