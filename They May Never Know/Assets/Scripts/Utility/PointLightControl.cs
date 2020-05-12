using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class PointLightControl : MonoBehaviour
{
    public Light2D pointLight;
    public DayCycle dayCycle;
    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponent<Light2D>();
        dayCycle = GameObject.FindGameObjectWithTag("GridController").GetComponent<DayCycle>();
        //dayCycle.nightLights.Add(pointLight);
    }

    private void FixedUpdate()
    {
        if(pointLight.enabled && (int)dayCycle.time >= 2 && (int)dayCycle.time <= 4)
        {
            pointLight.enabled = false;
        } else if(!pointLight.enabled && ((int)dayCycle.time < 2 || (int)dayCycle.time > 4))
        {
            pointLight.enabled = true;
        }
    }
}
