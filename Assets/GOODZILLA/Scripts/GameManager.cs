using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gameDurationMinute = 5f;
    public float maxAllowedDamage = 10000f;
    public float currentDamage = 0;

    public MeteorSpawner meteorSpawner;
    public bool startOnPlay = false;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI timeText;

    private bool isPlaying = false;
    private DateTime startTime;
    private DateTime stopTime;
    private DateTime currentTime;

    private void Start()
    {
        if (startOnPlay) Play();
    }

    public void Play()
    {
        meteorSpawner.StartWave();
        isPlaying = true;
        startTime = DateTime.Now;
        currentTime = startTime;
        stopTime = startTime.AddMinutes(gameDurationMinute);
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (isPlaying)
        {
            yield return new WaitForSecondsRealtime(1f);
            currentTime = currentTime.AddSeconds(1f);
            if (currentTime > stopTime)
            {
                GameOver();
                Debug.Log("Times up!");
            }
            else
            {
                Debug.Log("Remaining time " + (stopTime - currentTime).TotalSeconds + " s");
                timeText.text = "";// "Remaining time " + (stopTime - currentTime).TotalSeconds + " s";
            }

        }

    }


    public void AddDamage(float damage)
    {
        if (isPlaying)
        {
            if (currentDamage + damage > maxAllowedDamage)
            {
                GameOver();
            }
            else
            {
                currentDamage += damage;
                damageText.text = "";// "Damage $" + currentDamage + " / $" + maxAllowedDamage;
            }
        }

    }

    public void GameOver()
    {
        isPlaying = false;
        meteorSpawner.StopWave();
        Debug.Log("GAME OVER! HAHA");
    }


}
