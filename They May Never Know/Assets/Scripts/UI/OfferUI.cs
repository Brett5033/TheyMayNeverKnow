using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI prompt;
    public Offer offer;


    void Start()
    {
        offer = Offerings.getRandomOffer(Offerings.offeringList1);
        prompt.SetText(offer.Offering);
        ControlFactors.setPlayerCast(false);

    }

    // choice: 1 = yes, 2 = no, 3 = ignore
    public void chooseOption(int choice)
    {
        if(choice == 1)
        {
            // Applies Yes value if Yes is chosen
            if(offer.yesVal >= 0)
            {
                ControlFactors.LOVE += offer.yesVal;
                ControlFactors.FEAR -= offer.yesVal;
            }
            else
            {
                ControlFactors.FEAR += Mathf.Abs(offer.yesVal);
                ControlFactors.LOVE -= Mathf.Abs(offer.yesVal);
            }
        }
        else if(choice == 2)
        {
            // Applies No value if no is chosen
            if (offer.noVal >= 0)
            {
                ControlFactors.LOVE += offer.noVal;
                ControlFactors.FEAR -= offer.noVal;
            }
            else
            {
                ControlFactors.FEAR += Mathf.Abs(offer.noVal);
                ControlFactors.LOVE -= Mathf.Abs(offer.noVal);
            }
        }
        else if(choice == 3)
        {
            float remainingLoss = offer.ignoreVal;
            // Takes what needs to be deducted and finds which factor is larger (Love or Fear), then deducteds the ignore val from the largest.
            // If there is a remainder, then the remainder is applied to the other value.
            if (ControlFactors.LOVE >= ControlFactors.FEAR)
            {
                if(ControlFactors.LOVE >= remainingLoss)
                {
                    ControlFactors.LOVE -= remainingLoss;
                    remainingLoss = 0;
                } else
                {
                    remainingLoss -= ControlFactors.LOVE;
                    ControlFactors.LOVE = 0;
                    ControlFactors.FEAR -= remainingLoss;
                }
            }
            else
            {
                if (ControlFactors.FEAR >= remainingLoss)
                {
                    ControlFactors.FEAR -= remainingLoss;
                    remainingLoss = 0;
                }
                else
                {
                    remainingLoss -= ControlFactors.FEAR;
                    ControlFactors.FEAR = 0;
                    ControlFactors.LOVE -= remainingLoss;
                }
            }
        }
        ControlFactors.setPlayerCast(true);
        SceneLoader.unloadOptionScene();
    }
   
}
