using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public float timeRemaining;
    private float fullTime = 300f;
    public int ActualTime { get { return Mathf.RoundToInt(timeRemaining); } }

    private void OnEnable()
    {
        timeRemaining = fullTime;
        DontDestroyOnLoad(this);
        instance = this;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            SceneManager.LoadScene(0);
        }

        //Folhas virando loucuras

        if (fullTime - 120f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 60f)
        {
            //diminui folha
        }
        else if (fullTime - 180f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 120f)
        {
            //diminui folha
        }
        else if (fullTime - 240f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 180f)
        {
            //diminui folha
        }
        else if (fullTime - 300f < Mathf.RoundToInt(timeRemaining) && Mathf.RoundToInt(timeRemaining) < fullTime - 240f)
        {
            //diminui folha
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
