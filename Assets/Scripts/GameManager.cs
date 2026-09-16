using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int killCounter = 0;
    public Canvas winScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        killCounter = 0;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (killCounter >= 3)
        {
            killCounter = 0;
            winScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
