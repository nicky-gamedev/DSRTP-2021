using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    #region Rune list
    [System.Serializable]
    public class RuneGroup
    {
        public List<char> runes;
    }
    public List<RuneGroup> allRunes;
    #endregion

    [SerializeField] List<char> pickedRuneGroup;
    [SerializeField] List<Rune> gameRunes;

    [SerializeField] List<int> orderList;

    public bool won;

    private void Update()
    {
        if(orderList.Count == 4)
        {
            won = true;
        }
    }
    void OnEnable()
    {
        pickedRuneGroup = allRunes[Random.Range(0, allRunes.Count)].runes;
        gameRunes = GetComponentsInChildren<Rune>().ToList();

        List<char> characterList = new List<char>();
        foreach (var item in gameRunes)
        {
            do
            {
                int randomRange = Random.Range(0, gameRunes.Count);
                item.text = pickedRuneGroup[randomRange];
                item.order = randomRange;
            } while (characterList.Contains(item.text));

            characterList.Add(item.text);
        }
    }

    public void CheckOrder(Rune rune)
    {
        bool error = false;
        foreach (var item in gameRunes)
        {
            if (item.selected) continue;

            if (rune.order < item.order) continue;

            error = true;
        }

        if (error)
        {
            orderList.Clear();
            foreach (var item in gameRunes)
            {
                item.selected = false;
            }
        }
        else orderList.Add(rune.order);
    }
}
