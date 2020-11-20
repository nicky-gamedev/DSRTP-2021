using UnityEngine;

public class TimeTree : MonoBehaviour
{
    public Material leafs;
    public Transform top, bot;

    public void RemoveLeaf(float percentage)
    {
        leafs.SetFloat("_CutoffHeight", Mathf.Lerp(bot.position.y, top.position.y, percentage));
    }
}
