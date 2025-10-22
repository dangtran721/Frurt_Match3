using System.Collections.Generic;
using UnityEngine;

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
    ConnectionAlgorithm _connection;
    [SerializeField] List<BlockButton> List = new();

    void OnEnable()
    {
        _connection = this.GetComponent<ConnectionAlgorithm>();
    }
    public void Selected(BlockButton block)
    {
        if (List.Contains(block))
        {
            this.UnSelected(block);
            return;
        }
        if (List.Count < 2)
        {
            List.Add(block);
        }
        if (List.Count >= 2)
        {
            this.CheckMatch();
        }
    }
    
    public void UnSelected(BlockButton block)
    {
        if (block == null) return;
        List.Remove(block);
    }

    void CheckMatch()
    {
        Debug.Log("Check Match");
        BlockButton blockA = List[0];
        BlockButton blockB = List[1];
        if (blockA.Type == blockB.Type)
        {
            bool IsConnected = false;
            if (_connection.CheckLineFree
            (blockA.Row, blockA.Col, blockB.Row, blockB.Col))
            {
                Debug.Log("check line free");
                IsConnected = true;
            }
            else if (_connection.CheckOnePath(blockA, blockB))
            {
                Debug.Log("check one path");
                IsConnected = true;
            }
            else if (_connection.CheckTwoPath(blockA, blockB))
            {
                Debug.Log("check TWO path");
                IsConnected = true;
            }

            if (IsConnected)
            {
                this.WhenConnected(blockA, blockB);
            }
        }
        List.Clear();
    }

    void WhenConnected(BlockButton blockA, BlockButton blockB)
    {
        // 5. TODO: Kích hoạt hiệu ứng rơi gạch
        // FindObjectOfType<GameController>().HandleTilesDestroyed(blockA.Row, blockA.Col, blockB.Row, blockB.Col);
        GameController.Instance.OnBlockDestroy
        (blockA.Row, blockA.Col, blockB.Row, blockB.Col);
            
        Debug.Log("Match successful! Destroying blocks.");
        Destroy(blockA.gameObject);
        Destroy(blockB.gameObject);
    }
    
}
