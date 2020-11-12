using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeRemaining;
    private float fullTime = 300f;

    private void OnEnable()
    {
        timeRemaining = fullTime;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining >= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
