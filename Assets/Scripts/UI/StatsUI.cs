using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    // Serialized *****
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text timeText;

    // MonoBehavior Callbacks *****
    private void OnEnable()
    {
        GameManager.Instance.OnTimeChange += GameManager_OnTimeChange;
        GameManager.Instance.OnCurrencyChange += GameManager_OnCurrencyChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnTimeChange -= GameManager_OnTimeChange;
        GameManager.Instance.OnCurrencyChange -= GameManager_OnCurrencyChange;
    }
    
    // Private Methods *****
    private void GameManager_OnTimeChange(object sender, float newTime)
    {
        timeText.text = ToClock(newTime);
    }

    private void GameManager_OnCurrencyChange(object sender, int newCurrency)
    {
        coinsText.text = newCurrency.ToString();
    }
    
    private string ToClock(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt((time % 60));

        string sMinutes = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        string sSeconds = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        return sMinutes + ":" + sSeconds;
    }
}
