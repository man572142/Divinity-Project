using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Profile : MonoBehaviour
{
    [SerializeField] GameObject highlight = null;
    [SerializeField] TextMeshProUGUI occupation = null;
    [SerializeField] TextMeshProUGUI health = null;
    [SerializeField] TextMeshProUGUI ability = null;

    public void SetHighlight(bool state)
    {
        if (highlight != null)
        {
            highlight.SetActive(state);
        }
        
    }

    public void SetOccupation(string value)
    {
        occupation.text = value;
    }
    public void SetHealth(float value)
    {
        health.text = value.ToString();
    }
    public void SetAbility(float value)
    {
        ability.text = value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
