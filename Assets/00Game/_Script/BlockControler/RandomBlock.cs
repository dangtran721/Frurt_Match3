using UnityEngine;

public class RandomBlock : MonoBehaviour
{
    [SerializeField] BlockButton[] _block;
    
    int rand,count = 0;

    public BlockButton BlockRand()
    {
        if (count == 0) rand = Random.Range(0, _block.Length);

        count++;
        
        if (count >=2) count = 0;
        return _block[rand];
    }
}
