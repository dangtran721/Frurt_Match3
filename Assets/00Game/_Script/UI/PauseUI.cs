using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] Button _retry, _home, _resume;
    void Start()
    {
        this.LoadBase();
    }
    void LoadBase()
    {
        _retry.onClick.AddListener(this.OnRetryClick);
        _home.onClick.AddListener(this.OnHomeClick);
        _resume.onClick.AddListener(this.OnResumeClick);
    }
    void OnRetryClick()
    {

    }

    void OnHomeClick()
    {

    }
    
    void OnResumeClick()
    {
        
    }
}
