using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    private static GameController _instance;
    public static GameController Instance => _instance;
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

    [SerializeField] Transform _bgBoard, _board;
    RandomBlock _randomBlock;
    [SerializeField] List<BlockButton> list = new();
    [SerializeField] BGBlock _bg;
    public int InternalRows = 10;
    public int InternalCols = 6;
    public BlockType[,] logicMatrix;
    public BlockButton[,] _blockMatrix;
    public BGBlock[,] _bgMatrix;

    void OnEnable()
    {
        _randomBlock = this.GetComponent<RandomBlock>();
        this.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Suffer();
            Debug.Log("switch");
        }
    }
    void Init()
    {
        list.Clear();
        logicMatrix = new BlockType[InternalRows + 2, InternalCols + 2];
        _blockMatrix = new BlockButton[InternalRows, InternalCols];
        _bgMatrix = new BGBlock[InternalRows, InternalCols];
        for (int r = 0; r < InternalRows; r++)
        {
           // Debug.Log("row");
            for (int c = 0; c < InternalCols; c++)
            {
                //Debug.Log("col");
                BlockButton Block = this.BlockSpawner();
                Block.Row = r;
                Block.Col = c;
                _blockMatrix[r, c] = Block;

                logicMatrix[r + 1, c + 1] = Block.Type;

                BGBlock bg = this.BGBlockSpawner();
                bg.gameObject.SetActive(true);
                bg.Row = r;
                bg.Col = c;
                _bgMatrix[r, c] = bg;
                list.Add(Block);
            }
        }
    }

   public void Suffer()
    {
        for (int r = 0; r < InternalRows; r++)
        {
            for (int c = 0; c < InternalCols; c++)
            {
                int randR = Random.Range(0, InternalRows);
                int randC = Random.Range(0, InternalCols);
                BlockButton blockA = _blockMatrix[r, c];
                BlockButton blockB = _blockMatrix[randR, randC];
                (_blockMatrix[r, c], _blockMatrix[randR, randC]) = (blockB, blockA);
              //  Debug.Log("suffer");
            }
        }
        this.UpdateAllBlockVisuals();
    }

    void UpdateAllBlockVisuals()
    {
        for (int r = 0; r < InternalRows; r++)
        {
            for (int c = 0; c < InternalCols; c++)
            {
                BGBlock targetBG = _bgMatrix[r, c];

                BlockButton block = _blockMatrix[r, c];
                block.Col = c;
                block.Row = r;
                logicMatrix[r + 1, c + 1] = block.Type;
              //  Debug.Log("update visual: " + logicMatrix[r + 1, c + 1]);
                block.rect.position = targetBG.rect.position;
            }
        }

    }

    public void OnBlockDestroy(int rA, int cA, int rB, int cB)
    {
        logicMatrix[rA + 1, cA + 1] = BlockType.Empty;
        logicMatrix[rB + 1, cB + 1] = BlockType.Empty;
        //Debug.Log(logicMatrix[rA + 1, cA + 1] + "/" + logicMatrix[rB + 1, cB + 1]);
        // Gán null để tránh lỗi tham chiếu đến đối tượng đã bị hủy
        _blockMatrix[rA, cA] = null;
        _blockMatrix[rB, cB] = null;
    }

    BlockButton BlockSpawner()
    {
        return Instantiate(_randomBlock.BlockRand(), _board).Init();
    }

    BGBlock BGBlockSpawner()
    {
        return Instantiate(_bg, _bgBoard);
    }

    public BlockType CheckType(int r, int c)
    {
        if (r < 0 || r >= InternalRows || c < 0 || c >= InternalCols)
        {
            return BlockType.Empty;
        }
        return logicMatrix[r + 1, c + 1];
    }
}