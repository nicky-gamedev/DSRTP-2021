using UnityEngine;

public class TimeTree : MonoBehaviour
{
    public MeshRenderer[] leafs = new MeshRenderer[5];
    public bool[] activeLeafs = new bool[5];

    private void Start()
    {
        for (int i = 0; i < activeLeafs.Length; i++)
        {
            activeLeafs[i] = true;
        }
    }

    public void RemoveLeaf(int leaf)
    {
        leafs[leaf].material.color = Color.red;
        activeLeafs[leaf] = false;
    }
}
