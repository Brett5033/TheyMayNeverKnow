using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    public float developmentScoreConstant;
    public float LoveFearConstant;
    public float admirationDegrade;
    public float expansionSpeedModifier;
    public float developmentFactorScalar;
    public float energyMax;
    public float tempEnergyRefill;

    public GridTester gt;

    void Start()
    {
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();

    }

    void FixedUpdate()
    {
        gt.population.updateLoveHateStat();

        if (ControlFactors.LOVE < 0)
            ControlFactors.LOVE = 0;
        if (ControlFactors.FEAR < 0)
            ControlFactors.FEAR = 0;
          
        /*
        if (ControlFactors.ENERGY > energyMax)
        {
            ControlFactors.ENERGY = energyMax;
        }
        */
    }

    
}
