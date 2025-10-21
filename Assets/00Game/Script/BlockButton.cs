using System;
using UnityEngine;
using UnityEngine.UI;

public enum BlockType
{
    Empty,
    Apple,
    Banana,
    Broccoli,
    Carrot,
    ChickenThigh,
    Coconut,
    Donut,
    Grape,
    Lemon,
    Omelet,
    PieceCake,
    PurpleStar,
    SandWich,
    Scrim,
    Spinach,
    Strawberry,
    ToiletPaper
}

public class BlockButton : MonoBehaviour
{
    [SerializeField] public BlockType Type;
    [SerializeField] public BGBlock CurrentBG;
   public RectTransform rect;
    Button _block;

    [SerializeField] public int Row, Col;


    void Awake()
    {
        _block = this.GetComponent<Button>();
        rect = this.GetComponent<RectTransform>();
        this.StringToEnum();
    }
    public BlockButton Init()
    {
        _block.onClick.AddListener(OnBlockChange);
        return this;
    }
    void OnBlockChange()
    {
        Debug.Log("logicmatrix: " +
        GameController2.Instance.logicMatrix[Row + 1, Col + 1] +
        " / " + this.Type);
        Debug.Log("Tọa độ: " + this.Row + " / " + this.Col);
        BlockPicked.Instance.Selected(this);
    }
    
    void StringToEnum ()
    {
        // --- PHẦN CODE TỰ ĐỘNG GÁN TYPE ---
        // 1. Lấy tên của GameObject (ví dụ: "Apple(Clone)")
        string objectName = gameObject.name;
        
        // 2. Dọn dẹp tên, xóa bỏ "(Clone)" để chỉ còn lại "Apple"
        string cleanedName = objectName.Replace("(Clone)", "").Trim();

        // 3. Dùng Enum.TryParse để chuyển đổi tên (string) thành BlockType (enum)
        BlockType parsedType;
        if (Enum.TryParse(cleanedName, true, out parsedType)) // 'true' để không phân biệt hoa/thường
        {
            this.Type = parsedType;
        }
        else
        {
            Debug.LogWarning($"Không thể gán Type từ tên: '{cleanedName}'. Hãy đảm bảo tên Prefab khớp với một giá trị trong Enum BlockType.", this.gameObject);
        }
    }
}
