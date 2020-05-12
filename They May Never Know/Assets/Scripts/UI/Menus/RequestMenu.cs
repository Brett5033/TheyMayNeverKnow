using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RequestMenu : MonoBehaviour
{
    public Man man = null;
    public TMPro.TextMeshProUGUI _Message;
    public TMPro.TextMeshProUGUI _Benefit;
    public TMPro.TextMeshProUGUI _GrantText;
    public Button _GrantButton;
    public ManMenu currentMenu = null;
    public float energyCost = 0;
    public float loveBenefit;

    // Start is called before the first frame update
    void Start()
    {
        // How much Love the request generates
        loveBenefit = Mathf.Floor(Random.Range(10f, man.brain.maxEmotion - 10f));
        // How much the request costs
        energyCost = Mathf.Floor(Random.Range(5f, ControlFactors.MAX_ENERGY));

        _GrantButton.onClick.AddListener(() => { grant(); });
        _Message.SetText(man.prof.getRequest());
        _Benefit.SetText("+" + loveBenefit.ToString("F0") + " L");
        _GrantText.SetText("Grant: " + energyCost.ToString("F0") + " E");

        if(ControlFactors.ENERGY < energyCost)
        {
            _GrantButton.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!_GrantButton.interactable && ControlFactors.ENERGY >= energyCost)
        {
            _GrantButton.interactable = true;
        }
    }

    public void grant()
    {
        ControlFactors.ENERGY -= energyCost;
        man.brain.changeLove(loveBenefit);
        man.requestNotification.enabled = false;
        man.activeRequest = null;
        currentMenu.manRequest = null;
        Destroy(gameObject);
    }

    public void timeOut()
    {
        man.brain.changeFear(loveBenefit);
        man.requestNotification.enabled = false;
        man.activeRequest = null;
        currentMenu.manRequest = null;
        Destroy(gameObject);
    }
}
