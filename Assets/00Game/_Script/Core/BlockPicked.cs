using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPicked : MonoBehaviour
{
    #region Singleton
    private static BlockPicked _instance;
    public static BlockPicked Instance => _instance;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    #endregion

    private ConnectionAlgorithm _connection;
    [SerializeField] private List<BlockButton> _selected = new();

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private RectTransform _lineContainer;

    void OnEnable()
    {
        _connection = GetComponent<ConnectionAlgorithm>();

        // Khởi tạo trạng thái cho LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("Chưa gán LineRenderer vào BlockPicked!");
        }
    }

    public void Selected(BlockButton block)
    {
        if (_selected.Contains(block))
        {
            block.BorderImage.gameObject.SetActive(false);
            _selected.Remove(block);
            return;
        }
        block.BorderImage.gameObject.SetActive(true);

        _selected.Add(block);

        if (_selected.Count == 2)
        {
            
            CheckMatch();
        }
    }

    void CheckMatch()
    {
        if (_selected.Count < 2) return;

        BlockButton A = _selected[0];
        BlockButton B = _selected[1];
        A.BorderImage.gameObject.SetActive(false);
        B.BorderImage.gameObject.SetActive(false);
        if (A.Type == B.Type)
        {
            List<ConnectionAlgorithm.VirtualBlock> path = _connection.FindPath(A, B);
            if (path != null && path.Count >= 2)
            {
                DrawConnectionLine(path);
                WhenConnected(A, B);
            }
        }
        _selected.Clear();
    }
    

    void WhenConnected(BlockButton a, BlockButton b)
    {
        EventBus.Instance.Notify(Constant.GainScore);
        EventBus.Instance.Notify(Constant.GainTime);
        GameController.Instance.OnBlockDestroy(a.Row, a.Col, b.Row, b.Col);
        Destroy(a.gameObject);
        Destroy(b.gameObject);
    }
    // ==
    Vector2 ToContainerLocalPos(RectTransform rect)
    {
        Canvas canvas = _lineContainer.GetComponentInParent<Canvas>();
        Vector2 screen = RectTransformUtility.WorldToScreenPoint
        (canvas.worldCamera, rect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            _lineContainer, screen, canvas.renderMode ==
            RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null,
            out Vector2 local
        );
        return local;
    }

    Vector2 ToContainerLocalPos(ConnectionAlgorithm.VirtualBlock vb)
    {
        int r = Mathf.Clamp(vb.Row, 0, GameController.Instance.InternalRows - 1);
        int c = Mathf.Clamp(vb.Col, 0, GameController.Instance.InternalCols - 1);

        BGBlock bg = GameController.Instance._bgMatrix[r, c];
        if (bg == null || bg.rect == null) return Vector2.zero;

        Vector2 baseLocal = ToContainerLocalPos(bg.rect);

        // Tính toán kích thước cell từ BGBlock
        // Quan trọng: Phải lấy kích thước đã scale thực tế của RectTransform
        float cellW = bg.rect.rect.width;
        float cellH = bg.rect.rect.height;

        float dRow = vb.Row - r;
        float dCol = vb.Col - c;

        // Trục Y của UI ngược với trục Row (Row tăng -> Y giảm)
        return baseLocal + new Vector2(dCol * cellW, -dRow * cellH);
    }
// ==
    void DrawConnectionLine(List<ConnectionAlgorithm.VirtualBlock> path)
    {
        if (lineRenderer == null || path == null || path.Count < 2) return;

        // Set số lượng điểm cho LineRenderer
        lineRenderer.positionCount = path.Count;

        // Chuyển đổi List<VirtualBlock> thành mảng Vector3 (local)
        Vector3[] positions = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            // Lấy tọa độ local (Vector2) trong _lineContainer
            Vector2 localPos = ToContainerLocalPos(path[i]);

            // Gán vào mảng (Z = 0 vì là 2D UI)
            positions[i] = new Vector3(localPos.x, localPos.y, 0);
        }

        // Gán toàn bộ các điểm cho LineRenderer
        lineRenderer.SetPositions(positions);

        // Hiển thị line
        lineRenderer.enabled = true;

        // Bắt đầu Coroutine để ẩn line sau 1 khoảng thời gian
        StartCoroutine(DestroyLinesAfterDelay(0.3f));
    }

    IEnumerator DestroyLinesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearLines();
    }

    void ClearLines()
    {
        if (lineRenderer != null)
        {
             lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
    }
}