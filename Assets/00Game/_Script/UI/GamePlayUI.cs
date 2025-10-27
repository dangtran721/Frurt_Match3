using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField] Image _modelTimeSlide;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] float TimeMax = 60;
    float TimeLeft = 0;
    void Start()
    {
        TimeLeft = TimeMax;
        StartCoroutine(this.TimeSlideCal());
    }

    void TimeSlideControl()
    {
        TimeLeft--;
        Debug.Log(TimeLeft);
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