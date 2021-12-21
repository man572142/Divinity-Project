using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public float maxHP;
    public float hP;
    public bool isHPMax = true;
    [SerializeField] Image hPBar = null;
    [SerializeField] Image mainHPBar = null;
    [SerializeField] Text hPAmount = null;
    bool canRevealMainBar = false;

    RaycastHit hit;

    int hPChange;
    float lastHPPercentage = 0f;

    //public PlayerTeam playerTeam = PlayerTeam.None;

    void Start()
    {
        HPMax();
    }

    void Update()
    {
        HPUpdate();
    }

    public void SetMainBarState(bool state)
    {
        canRevealMainBar = state;
    }

    public void SetHP(float value)
    {
        maxHP = value;
        hP = maxHP;
    }

    void HPMax()
    {
        if (isHPMax) { hP = maxHP; }
    }

    //public void Hit()
    //{
    //    HPChange();
    //}

    public void HPChange(float damage)
    {
        if (hP <= 0)
        {
            return;
        }

        hP -= damage;
        Debug.Log("attacked.");

        if (hP >= maxHP)
        {
            hP = maxHP;
        }
        Debug.Log(hP);
    }

    void HPUpdate()
    {
        float hPPercentage = hP / maxHP;

        if (hPPercentage != lastHPPercentage)
        {
            if (canRevealMainBar && hPAmount != null)
            {
                hPAmount.text = hP.ToString("F0") + " / " + maxHP.ToString("F0");
            }

            if (hPBar != null)
            {
                hPBar.fillAmount = hPPercentage;
            }
            if(canRevealMainBar && mainHPBar != null)
            {
                mainHPBar.fillAmount = hPPercentage;
            }
        }
    }
}
//public enum PlayerTeam 
//{ 
//    None,
//    Team1, 
//    Team2,
//}
