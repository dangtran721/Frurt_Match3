using UnityEngine;

public class ConnectionAlgorithm : MonoBehaviour
{
    class VirtualBlock
    {
        public int Row, Col;
        public VirtualBlock(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
        }
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
                if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
                {
                    Debug.Log("line blocked");
                    return false;
                }
            }
        }
        Debug.Log("line free");
        return true;
    }
    public bool CheckOnePath (BlockButton blockA, BlockButton blockB)
    {
        return CheckOnePathByVirtual(new VirtualBlock(blockA.Row, blockA.Col),
        new VirtualBlock(blockB.Row, blockB.Col));
    }

    bool CheckOnePathByVirtual(VirtualBlock blockA, VirtualBlock blockB)
    {
        int rA = blockA.Row;
        int cA = blockA.Col;
        int rB = blockB.Row;
        int cB = blockB.Col;
        if (GameController.Instance.CheckType(rA, cB) == BlockType.Empty)
        {//c1 (rA, cB)
            if (this.CheckLineFree(rA, cA, rA, cB) &&
            this.CheckLineFree(rA, cB, rB, cB))
            {
                Debug.Log("one path");
                return true;
            }
        }
        else if (GameController.Instance.CheckType(rB, cA) == BlockType.Empty)
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

    public bool CheckTwoPath(BlockButton blockA, BlockButton blockB)
    {
        return this.CheckTwoPathByVirtual(new VirtualBlock(blockA.Row, blockA.Col),
        new VirtualBlock(blockB.Row, blockB.Col));
    }

    bool CheckTwoPathByVirtual(VirtualBlock blockA, VirtualBlock blockB)
    {
        int rA = blockA.Row;
        int cA = blockA.Col;
        int rB = blockB.Row;
        int cB = blockB.Col;

        for (int i = rA + 1; i < GameController.Instance.InternalRows + 1; i++)
        {
            if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(i, cA);
            if (this.CheckOnePathByVirtual(P, blockB))
            {
                Debug.Log("2 path");
                return true;
            }
        }
        for (int i = rA -1; i >= -1 ; i--)
        {
            if (GameController.Instance.CheckType(i, cA) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(i, cA);
            if (this.CheckOnePathByVirtual(P, blockB))
            {
                Debug.Log("2 path");
                return true;
            }
        }
        for (int i = cA + 1; i < GameController.Instance.InternalCols +1; i++)
        {
            if (GameController.Instance.CheckType(rA,i) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(rA,i);
            if (this.CheckOnePathByVirtual(P, blockB))
            {
                Debug.Log("2 path");
                return true;
            }
        }
           for(int i = cA - 1; i >= -1;i--)
        {
            if (GameController.Instance.CheckType(rA,i) != BlockType.Empty)
            {
                break;
            }
            VirtualBlock P = new VirtualBlock(rA,i);
            if(this.CheckOnePathByVirtual(P,blockB))
            {
                Debug.Log("2 path");
               return true;
            }
        }
        return false;
    }
    
}
