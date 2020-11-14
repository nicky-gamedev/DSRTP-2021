using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public float timeRemaining;
    public float fullTime = 300f;
    public bool counting;
    public int ActualTime { get { return Mathf.RoundToInt(timeRemaining); } }

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (counting)
        {
            timeRemaining -= Time.deltaTime;

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
