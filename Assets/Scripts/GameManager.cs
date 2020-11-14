using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeRemaining;
    public float fullTime = 300f;
    public bool counting;
    public int ActualTime { get { return Mathf.RoundToInt(timeRemaining); } }

    private TimeTree timeTree;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        timeTree = FindObjectOfType<TimeTree>();
    }

    void Update()
    {
        if (counting)
        {
            timeRemaining -= Time.deltaTime;

            //Folhas virando loucuras

            if (timeTree.activeLeafs[4] && fullTime - 121f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 60f)
            {
                Debug.Log("Removing leaf 4");
                timeTree.RemoveLeaf(4);
            }
            else if (timeTree.activeLeafs[3] && fullTime - 181f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 120f)
            {
                Debug.Log("Removing leaf 3");
                timeTree.RemoveLeaf(4);
                timeTree.RemoveLeaf(3);
            }
            else if (timeTree.activeLeafs[2] && fullTime - 241f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 180f)
            {
                Debug.Log("Removing leaf 2");
                timeTree.RemoveLeaf(4);
                timeTree.RemoveLeaf(3);
                timeTree.RemoveLeaf(2);
            }
            else if (timeTree.activeLeafs[1] && fullTime - 271f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 240f)
            {
                Debug.Log("Removing leaf 1");
                timeTree.RemoveLeaf(4);
                timeTree.RemoveLeaf(3);
                timeTree.RemoveLeaf(2);
                timeTree.RemoveLeaf(1);
            }
            else if (timeTree.activeLeafs[0] && fullTime - 300f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 270f)
            {
                Debug.Log("Removing leaf 0");
                timeTree.RemoveLeaf(4);
                timeTree.RemoveLeaf(3);
                timeTree.RemoveLeaf(2);
                timeTree.RemoveLeaf(1);
                timeTree.RemoveLeaf(0);
            }
        }
        if (timeRemaining < 0)
        {
            SceneManager.LoadScene(0);
            counting = false;
            timeRemaining = 0;
        }
    }

    public void Strike()
    {
        timeRemaining -= 60f;
        if (timeRemaining <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
