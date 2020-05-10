using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EmotionPopup : MonoBehaviour
{
    public Man man = null;
    public TMPro.TextMeshProUGUI _Streak;
    public Image colorBox;
    
    // Start is called before the first frame update
    void Start()
    {
        _Streak.SetText(man.brain.streakCounter.ToString());
        if (man.brain.emotionStreakState)
        {
            colorBox.color = Color.blue;
        }
        else
        {
            colorBox.color = Color.red;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(man != null)
        {
            _Streak.SetText(man.brain.streakCounter.ToString());
            if (man.brain.emotionStreakState)
            {
                colorBox.color = Color.blue;
            }
            else
            {
                colorBox.color = Color.red;
            }
            transform.position = Camera.main.WorldToScreenPoint(new Vector3(man.transform.position.x, man.transform.position.y + 2f, 0f));
        }
            
    }

    public void close()
    {
        Destroy(gameObject);
    }
}
