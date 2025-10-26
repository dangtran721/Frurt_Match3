using System.Collections.Generic;
using UnityEngine;

public class ConnectionAlgorithm : MonoBehaviour
{
    [SerializeField] List<VirtualBlock> currentPath = new();
    public class VirtualBlock
    {
        public int Row, Col;
        public VirtualBlock(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
        }
    }

    public List<VirtualBlock> FindPath(BlockButton blockA, BlockButton blockB)
    {
        currentPath.Clear();

        VirtualBlock start = new(blockA.Row, blockA.Col);
        VirtualBlock end = new(blockB.Row, blockB.Col);
        if (CheckLineFree(start.Row, start.Col, end.Row, end.Col))
        {
            currentPath.Add(start);
            currentPath.Add(end);
            return currentPath;
        }
        else if (CheckOnePathByVirtual(start, end))
        {
            return currentPath;
        }
        else if (CheckTwoPathByVirtual(start, end))
        {
            return currentPath;
        }
        return null;
    }

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
                if (GameController.Instance.CheckType(rA, i) != BlockType.Empty)
                {
                    //Debug.Log("line blocked");
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
                if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
                {

                    //Debug.Log("line blocked");
                    return false;
                }
            }
        }
        // Debug.Log("line free");
        return true;
    }

    bool CheckOnePathByVirtual(VirtualBlock blockA, VirtualBlock blockB)
    {
        int rA = blockA.Row;
        int cA = blockA.Col;
        int rB = blockB.Row;
        int cB = blockB.Col;
        VirtualBlock C1 = new(rA, cB);
        if (GameController.Instance.CheckType(C1.Row, C1.Col) == BlockType.Empty)
        {
            if (this.CheckLineFree(rA, cA, rA, cB) &&
            this.CheckLineFree(rA, cB, rB, cB))
            {
                currentPath.Clear();
                currentPath.Add(blockA);
                currentPath.Add(C1);
                currentPath.Add(blockB);
                return true;
            }
        }

        VirtualBlock C2 = new(rB, cA);
        if (GameController.Instance.CheckType(C2.Row, C2.Col) == BlockType.Empty)
        {
            if (this.CheckLineFree(rA, cA, rB, cA) &&
            this.CheckLineFree(rB, cA, rB, cB))
            {
                currentPath.Clear();
                currentPath.Add(blockA);
                currentPath.Add(C2);
                currentPath.Add(blockB);
                return true;
            }
        }
        //   Debug.Log("one path can not line");
        return false;
    }

    bool CheckTwoPathByVirtual(VirtualBlock blockA, VirtualBlock blockB)
    {
        int rA = blockA.Row;
        int cA = blockA.Col;
        int rB = blockB.Row;
        int cB = blockB.Col;
        bool TryPathThroughPoint(VirtualBlock P)
        {
            if (this.CheckOnePathByVirtual(P, blockB))
            {
                currentPath.Insert(0, blockA);
                // current path {blockA,P,C,blockB}
                return true;
            }
            return false;
        }
        for (int i = rA + 1; i < GameController.Instance.InternalRows + 1; i++)
        {
            if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(i, cA);
            if (TryPathThroughPoint(P))
            {
                return true;
            }
        }
        for (int i = rA - 1; i >= -1; i--)
        {
            if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(i, cA);
            if (TryPathThroughPoint(P))
            {
                return true;
            }
        }
        for (int i = cA + 1; i < GameController.Instance.InternalCols + 1; i++)
        {
            if (GameController.Instance.CheckType(rA, i) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(rA, i);
            if (TryPathThroughPoint(P))
            {
                return true;
            }
        }
        for (int i = cA - 1; i >= -1; i--)
        {
            if (GameController.Instance.CheckType(rA, i) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(rA, i);
            if (TryPathThroughPoint(P))
            {
                return true;
            }
        }
        return false;
    }
}