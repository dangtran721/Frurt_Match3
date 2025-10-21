using UnityEngine;

public class ConnectionAlgorithm : MonoBehaviour
{
    public bool CheckLineFree(int rA, int cA, int rB, int cB)
    {
        if (rA != rB && cB != cA) return false;
        if (rA == rB)
        {
            // Dùng Min và Max để đảm bảo vòng lặp luôn đúng chiều
        int minCol = Mathf.Min(cA, cB);
        int maxCol = Mathf.Max(cA, cB);
            for (int i = minCol + 1; i < maxCol; i++)
            {
                if (GameController2.Instance.CheckType(rA, i) != BlockType.Empty)
                {
                    Debug.Log("line blocked");
                    return false;
                }
            }
        }
        else if (cA == cB)
        {
            // Dùng Min và Max để đảm bảo vòng lặp luôn đúng chiều
        int minRow = Mathf.Min(rA, rB);
        int maxRow = Mathf.Max(rA, rB);
            for (int i = minRow + 1; i < maxRow; i++)
            {
                if (GameController2.Instance.CheckType(i, cA) != BlockType.Empty)
                {
                    Debug.Log("line blocked");
                    return false;
                }
            }
        }
        Debug.Log("line free");
        return true;
    }

    public bool CheckOnePath(BlockButton blockA, BlockButton blockB)
    {
        int rA = blockA.Row;
        int cA = blockA.Col;
        int rB = blockB.Row;
        int cB = blockB.Col;
        if (GameController2.Instance.CheckType(rA, cB) == BlockType.Empty)
        {//c1 (rA, cB)
            if (this.CheckLineFree(rA, cA, rA, cB) &&
            this.CheckLineFree(rA, cB, rB, cB))
            {
                Debug.Log("one path");
                return true;
            }
        }
        else if (GameController2.Instance.CheckType(rB, cA) == BlockType.Empty)
        {//c2 (rB, cA)
            if (this.CheckLineFree(rA, cA, rB, cA) &&
            this.CheckLineFree(rB, cA, rB, cB))
            {
                Debug.Log("one path");
                return true;
            }
        }
        Debug.Log("one path can not line");
        return false;
    }

    // public bool CheckTwoPath(int VisualR, int VisualC, int TargetRow, int TargetCol)
    // {
    //     BlockButton blockA;
    //     BlockButton blockB;
    //     blockA.Row = VisualR;
    //     blockA.Col = VisualC;
    //     blockB.Row = TargetRow;
    //     blockB.Col = TargetCol;

    // }
    
}
