using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text timeReductionText;
    [SerializeField] float timeLeft = 30f;
    [SerializeField] float timeRedTextPopTime = 1f;


    public float TimeLeft{ get { return timeLeft; } set { timeLeft = value; }}
    void Update()
    {
        updateTime();
    }

    private void updateTime()
    {
        timeLeft = Math.Max(timeLeft-Time.deltaTime, 0f);
        timeText.text = timeLeft.ToString("F1");
    }

    public void TimeReduction(float val)
    {
        timeLeft -= val;
        StartCoroutine(TimeReductionCoroutine(-val));
    }

    IEnumerator TimeReductionCoroutine(float val)
    {
        timeReductionText.text = val.ToString();
        timeReductionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeRedTextPopTime);
        timeReductionText.gameObject.SetActive(false);
    }
}
