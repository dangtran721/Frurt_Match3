using UnityEngine;

public class BGBlock : MonoBehaviour
{
    public RectTransform rect;
    public int Row, Col;

    void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        Debug.Log("rect bg");
    }
}
