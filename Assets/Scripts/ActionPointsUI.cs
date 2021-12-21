using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] Image[] pointUI = new Image[8];
    [SerializeField] Color usableAP = Color.green;
    [SerializeField] Color costAP = Color.red;

    public void ShowAP(int currentAP,int estimateCost)
    {
        for (int i = 0; i < pointUI.Length; i++)
        {
            if (i < currentAP)
            {
                pointUI[i].gameObject.SetActive(true);
                pointUI[i].color = usableAP;

                if (i >= (currentAP - estimateCost))
                {
                    pointUI[i].color = costAP;
                }
            }
            else pointUI[i].gameObject.SetActive(false);
        }


    }
}
