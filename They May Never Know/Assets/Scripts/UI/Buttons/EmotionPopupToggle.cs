using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EmotionPopupToggle : MonoBehaviour
{
    public Button button;
    public GridTester gt;
    bool ePopsOn = false;
    
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
    }

    public void toggleEPops()
    {
        ePopsOn = !ePopsOn;
        Debug.Log("Pressed: " + ePopsOn);
        foreach(Man m in gt.population.populationList)
        {
            m.toggleEmotionPopup(ePopsOn); // Opens or closes all ePops
        }
        if (ePopsOn)
        {
            StartCoroutine(updateEPops());
            GetComponent<Image>().fillCenter = false;
        }
        else
        {
            GetComponent<Image>().fillCenter = true;
        }
           
    }

    IEnumerator updateEPops() // Catches any humans that have been created since the button was pressed
    {
        while (ePopsOn)
        {
            yield return new WaitForSeconds(5f);
            bool holder = ePopsOn;
            if (holder)
            {
                foreach (Man m in gt.population.populationList)
                {
                    m.toggleEmotionPopup(holder); // Opens or closes all ePops
                }
            }
            
        }
    }
    
}
