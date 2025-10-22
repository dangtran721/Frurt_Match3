using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DVAH.Lib
{
    // NOTE: Tên class đang bị sai chính tả: "Felexiable" -> "Flexible" (không bắt buộc sửa)
    // Kế thừa LayoutGroup để tự sắp xếp các phần tử con (UI) theo dạng lưới linh hoạt
    public class FelexiableLayoutGroup : LayoutGroup
    {
        // Kiểu fit để tính số hàng/cột và cell size
        public FitType fitType;

        // Số hàng/cột (có thể tự tính tùy fitType)
        public int rows, columns;

        // Kích thước mỗi ô và khoảng cách giữa các ô
        public Vector2 cellSize, spacing;

        // Có ép fit chiều ngang/dọc theo kích thước parent không
        public bool FitX, FitY;

        // Unity gọi hàm này khi layout cần tính theo trục dọc
        // (Với LayoutGroup, thông thường bạn cũng nên override CalculateLayoutInputHorizontal,
        // nhưng ở đây bạn gom hết vào Vertical cũng vẫn chạy được)
        [SerializeField] bool _turnOn;

        protected override void Start()
        {
            _turnOn = true;
        }

        public override void CalculateLayoutInputVertical()
        {
            if (!_turnOn) return;
            // Nếu chọn Width/Heigh/Uniform thì bắt Fit cả X lẫn Y
            // và mặc định rows/columns theo căn bậc 2 số child (gần như grid vuông)
            if (fitType == FitType.Width || fitType == FitType.Heigh || fitType == FitType.Uniform)
            {
                FitX = true;
                FitY = true;

                // Số child (được LayoutGroup chấp nhận) = rectChildren.Count
                // Ở đây dùng this.transform.childCount (tính cả inactive) -> có thể lệch so với rectChildren
                // Nếu muốn chính xác theo LayoutGroup, nên dùng rectChildren.Count
                float sqrRt = Mathf.Sqrt(this.transform.childCount);

                // Làm tròn lên để có ma trận vuông: rows = cols = ceil(sqrt(n))
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            // Nếu fit theo WIDTH (hoặc bạn fix cột), thì columns đã biết -> tính rows = ceil(n / columns)
            if (fitType == FitType.Width || fitType == FitType.FixedColum)
            {
                // Chú ý ép về float để chia chính xác
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            // Nếu fit theo HEIGHT (hoặc bạn fix hàng), thì rows đã biết -> tính columns = ceil(n / rows)
            if (fitType == FitType.Heigh || fitType == FitType.FixedRow)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            // Lấy kích thước khung cha (RectTransform gắn trên LayoutGroup)
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            // TÍNH CELL SIZE:
            // Công thức width: (tổng rộng parent trừ padding 2 bên, trừ spacing giữa cột) / số cột
            float cellWidth =
                (parentWidth - (spacing.x * (columns - 1)) - padding.left - padding.right) / (float)columns;

            // Công thức height (LƯU Ý dấu ngoặc):
            // Hiện tại bạn viết: (parentHeight - (spacing.y * (rows -1) - padding.top - padding.bottom)) / rows
            // Nghĩa là padding.top/bottom đang bị trừ sai dấu. Công thức đúng nên là:
            // (parentHeight - padding.top - padding.bottom - spacing.y * (rows - 1)) / rows
            float cellHeight =
                (parentHeight - (spacing.y * (rows - 1) - padding.top - padding.bottom)) / (float)rows;

            // Nếu bật FitX/FitY thì cập nhật cellSize theo tính toán; nếu không, giữ nguyên giá trị inspector
            cellSize.x = FitX ? cellWidth : cellSize.x;
            cellSize.y = FitY ? cellHeight : cellSize.y;

            // Dùng để xác định vị trí hàng/cột của từng item
            int columnCount = 0, rowCount = 0;

            // Xác định có phải canh giữa theo chiều ngang không (Center Left/Middle/Right)
            // childAlignment enum: 0=UpperLeft,1=UpperCenter,2=UpperRight,3=MiddleLeft,4=MiddleCenter,5=MiddleRight,6=LowerLeft,7=LowerCenter,8=LowerRight
            bool isCenter = (int)childAlignment == 1 || (int)childAlignment == 4 || (int)childAlignment == 7;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                // Tính hàng/cột của item thứ i
                rowCount = i / columns;
                columnCount = i % columns;

                float center = 0;
                // Số phần tử dư ở hàng cuối cùng (n % columns)
                int leftOver = rectChildren.Count % columns;

                // Nếu canh giữa và đang ở hàng cuối, và hàng cuối không đủ cột
                if (isCenter && (rowCount == rows - 1 && leftOver != 0))
                {
                    // Mục tiêu: đẩy cụm LeftOver item vào giữa chiều ngang
                    // Hiện công thức lấy spacing.x*(columns-1) -> KHÔNG đúng khi hàng cuối chỉ có "leftOver" item.
                    // Đúng ra phải dùng (leftOver - 1) cho số khoảng cách thực tế trên hàng cuối.
                    center = (parentWidth
                                - cellSize.x * leftOver
                                - spacing.x * (columns - 1)      // <-- chỗ này hợp lý hơn là (leftOver - 1)
                                - padding.left - padding.right) / 2.0f;
                }

                var item = rectChildren[i];

                // Vị trí X: padding.left + (cellSize + spacing)*column + offset center nếu có
                var xPos = center + cellSize.x * columnCount + spacing.x * columnCount + padding.left;

                // Vị trí Y: padding.top + (cellSize + spacing)*row
                // LƯU Ý: hệ toạ độ UI (Top-Left) tuỳ canvas, nhưng SetChildAlongAxis sẽ lo việc căn chuẩn theo anchor/pivot
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;

                // Đặt kích thước & vị trí item theo 2 trục
                SetChildAlongAxis(item, 0, xPos, cellSize.x); // trục X
                SetChildAlongAxis(item, 1, yPos, cellSize.y); // trục Y
            }
            _turnOn = false;
            GameController.Instance.Suffer();
        }

        public override void SetLayoutHorizontal()
        {
            // Ở đây bạn không làm gì, vì đã đặt trong CalculateLayoutInputVertical.
            // Thực tế, chuẩn hơn là chia logic: tính toán trong Calculate..., và set ở SetLayoutHorizontal/Vertical.
            // Nhưng cách này vẫn hoạt động.
        }

        public override void SetLayoutVertical()
        {
            // Tương tự như trên.
        }

        public enum FitType
        {
            Uniform,   // Ưu tiên ma trận vuông (rows ~ cols)
            Width,     // Fix số cột, tự tính số hàng
            Heigh,     // (Typo) -> Height: Fix số hàng, tự tính số cột
            FixedRow,  // Cố định rows (bạn tự set), auto columns
            FixedColum // (Typo) -> FixedColumn: Cố định columns (bạn tự set), auto rows
        }
    }
}
