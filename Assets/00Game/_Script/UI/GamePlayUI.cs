using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField] Image _modelTimeSlide;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] float TimeMax = 60;
    int Score = 0;
    float TimeLeft = 0;

    void Start()
    {
        this.LoadBase();
        StartCoroutine(this.TimeSlideCal());
    }

    void LoadBase()
    {
        EventBus.Instance.Sub(Constant.GainScore, this.GainScore);
        EventBus.Instance.Sub(Constant.GainTime, this.GainTime);
        _scoreText.text = Score.ToString();
        TimeLeft = TimeMax;
    }

    void TimeSlideControl()
    {
        TimeLeft--;
        Debug.Log(TimeLeft);
        _modelTimeSlide.fillAmount = TimeLeft / TimeMax;
    }

    void GainScore(object[] data)
    {
        Score += 10;
        _scoreText.text = Score.ToString();
    }
    
    void GainTime(object[] data)
    {
        TimeLeft += 3;
        _modelTimeSlide.fillAmount = TimeLeft / TimeMax;
    }

    IEnumerator TimeSlideCal()
    {
        while (TimeLeft > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            this.TimeSlideControl();
        }
    }
}