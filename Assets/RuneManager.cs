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
    public int position;
    public GameObject door;

    void OnEnable()
    {
        pickedRuneGroup = allRunes[Random.Range(0, allRunes.Count)].runes;
        gameRunes = GetComponentsInChildren<Rune>().ToList();
        var rnd = new System.Random();
        gameRunes = gameRunes.OrderBy(item => rnd.Next()).ToList();

        List<char> characterList = new List<char>();
        foreach (var item in gameRunes)
        {
            int randomRange;
            do
            {
                randomRange = Random.Range(0, pickedRuneGroup.Count);
                item.text = pickedRuneGroup[randomRange];
            } while (characterList.Contains(item.text));

            characterList.Add(item.text);
            orderList.Add(randomRange);
        }
    }

    public void CheckOrder(Rune rune)
    {
        bool error = !(rune.text == pickedRuneGroup[orderList[position]]);

        if (error)
        {
            position = 0;
            foreach (var item in gameRunes)
            {
                item.selected = false;
            }
            GameManager.instance.Strike();
        }
        else position++;

        if (position >= 4 && !won)
        {
            won = true;
            door.SetActive(false);
        }
    }
}
