﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ManMenu : MonoBehaviour
{
    public Man man = null;
    public TMPro.TextMeshProUGUI _Label;
    public TMPro.TextMeshProUGUI _Love;
    public TMPro.TextMeshProUGUI _Fear;
    public TMPro.TextMeshProUGUI _State;
    public Button closeButton;
    public RequestMenu manRequest = null;

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() => { close(); });
        ControlFactors.setPlayerCast(false);
        if(man != null)
        {
            man.setStatMenuOpen(true);
            if (man.activeRequest)
            {
                man.activeRequest.gameObject.SetActive(true);
                manRequest = man.activeRequest;
                manRequest.currentMenu = this;
            }
        }
        
        _Label.SetText(man.title);
        _Love.SetText("Love: " + ((man.brain.Love / man.brain.maxEmotion) * 100f).ToString("F0") + "%");
        _Fear.SetText("Fear: " + ((man.brain.Fear / man.brain.maxEmotion) * 100f).ToString("F0") + "%");
        _State.SetText("State: " + man.state);
    }

    // Update is called once per frame
    void Update()
    {
        if(man != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                close();
            }
            else
            {
                //transform.position = Camera.main.WorldToScreenPoint(man.transform.position);//new Vector3(man.transform.position.x, man.transform.position.y + 5f, 0));
             
                _Love.SetText("Love: " + ((man.brain.Love / man.brain.maxEmotion) * 100f).ToString("F0") + "%");
                _Fear.SetText("Fear: " + ((man.brain.Fear / man.brain.maxEmotion) * 100f).ToString("F0") + "%");
                _State.SetText("State: " + man.state);
            }
            
            
        }
    }

    public void close()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManualCamera>().setTarget(null);
        GameObject.FindGameObjectWithTag("GridController").GetComponent<UIHandler>().manMenuOpen = false;
        man.setStatMenuOpen(false);
        if (manRequest)
        {
            manRequest.currentMenu = null;
            manRequest.gameObject.SetActive(false);
        }
            
        ControlFactors.setPlayerCast(true);
        Destroy(gameObject);
    }

   
}
