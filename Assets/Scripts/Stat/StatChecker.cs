using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatChecker : MonoBehaviour
{
    [SerializeField] StatReader[] statReaders = new StatReader[4];
    [SerializeField] Text warningText = null;
    [SerializeField] Text highNumber = null;
    [SerializeField] Text midNumber = null;
    [SerializeField] Text lowNumber = null;

    int none = 0;
    int highRank = 3;
    int midRank = 2;
    int lowRank = 3;

    private void Start()
    {
        ShowStatNumber();
    }

    private void ShowStatNumber()
    {
        highNumber.text = highRank.ToString();
        midNumber.text = midRank.ToString();
        lowNumber.text = lowRank.ToString();
    }

    public bool StatCheck()
    {
        none = 0;
        highRank = 3;
        midRank = 2;
        lowRank = 3;
        foreach (StatReader stat in statReaders)
        {
            switch(stat.HealthDropDown.value)
            {
                case 0:
                    none++;
                    break;
                case 1:
                    highRank--;
                    break;
                case 2:
                    midRank--;
                    break;
                case 3:
                    lowRank--;
                    break;
            }
            switch (stat.AbilityDropDown.value)
            {
                case 0:
                    none++;
                    break;
                case 1:
                    highRank--;
                    break;
                case 2:
                    midRank--;
                    break;
                case 3:
                    lowRank--;
                    break;
            }

            if (stat.OccupationDropDown.value == 0) none++;
        }

        ShowStatNumber();

        if (highRank < 0 || midRank < 0 || lowRank < 0)
        {
            warningText.text = "有數值超過使用上限";
            return false;
        }
        else if (none > 0)
        {
            warningText.text = "尚有數值未被設定";
            return false;
        }
        else
        {
            warningText.text = "";
            return true;
        }
    }


    }
