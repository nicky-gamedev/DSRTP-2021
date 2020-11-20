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

    [System.Serializable]
    public class RunePrefabs
    {
        public List<GameObject> runes;
    }
    public List<RunePrefabs> allPrefabs;
    #endregion

    [SerializeField] List<char> pickedRuneGroup;
    [SerializeField] List<GameObject> pickedRunePrefabs;
    [SerializeField] List<Rune> gameRunes;
    [SerializeField] List<int> orderList;

    public Dictionary<char, GameObject> runePrefabs = new Dictionary<char, GameObject>();

    public bool won;
    public int position;
    public GameObject door;
    private AudioSource ad => GetComponent<AudioSource>();

    void OnEnable()
    {
        int rand = Random.Range(0, allRunes.Count);
        pickedRuneGroup = allRunes[rand].runes;
        pickedRunePrefabs = allPrefabs[rand].runes;

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
                item.runePrefab = pickedRunePrefabs[randomRange];
            } while (characterList.Contains(item.text));

            characterList.Add(item.text);
            orderList.Add(randomRange);

            item.Steup();
        }

        orderList = Sort(orderList);
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
            ad.Play();
        }
    }

    List<int> Sort(List<int> data)
    {
        int i, j;
        int N = data.Count;

        for (j = 1; j < N; j++)
        {
            for (i = j; i > 0 && data[i] < data[i - 1]; i--)
            {
                exchange(data, i, i - 1);
            }
        }

        return data;
    }

    void exchange(List<int> data, int m, int n)
    {
        int temporary;

        temporary = data[m];
        data[m] = data[n];
        data[n] = temporary;
    }
}
