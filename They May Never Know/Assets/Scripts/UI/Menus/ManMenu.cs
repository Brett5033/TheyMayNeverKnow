using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() => { close(); });
        ControlFactors.setPlayerCast(false);
        man.setStatMenuOpen(true);
        _Label.SetText("Man Stats:"); // Reserved for names
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
        GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIHandler>().manMenuOpen = false;
        man.setStatMenuOpen(false);
        ControlFactors.setPlayerCast(true);
        Destroy(gameObject);
    }

   
}
