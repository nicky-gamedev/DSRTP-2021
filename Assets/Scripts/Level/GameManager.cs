using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    void LateUpdate()
    {
        if (counting)
        {
            timeRemaining -= Time.deltaTime;

            if (timeTree == null)
            {
                timeTree = FindObjectOfType<TimeTree>();
            }

            //Folhas virando loucuras

            SetTreeTime(fullTime, timeRemaining);
            
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
    }

    public void Hit()
    {
        timeRemaining -= 15f;
    }

    private void SetTreeTime(float maxTime, float currentTime)
    {
        timeTree.RemoveLeaf(currentTime / maxTime);
    }
}
