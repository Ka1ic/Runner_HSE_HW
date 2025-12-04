using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public UIController uiControl;
    public PlayerController player;
    public GroundGenerator groundGenerator;
    [SerializeField] private float scoreGainSpeed = 1f;
    public int score = 0;

    void Update()
    {
        score = Mathf.RoundToInt(player.transform.position.z * scoreGainSpeed);
        uiControl.SetScoreText($"Score: {score}");
    }

    public void UpdateHealth(int health)
    {
        uiControl.SetHearts(health);
    }

    public void StartIndestructibleBonus(float duration)
    {
        uiControl.StartIndestructibleBonus(duration);
    }

    public void EndRun()
    {
        uiControl.SetFinalScoreText($"Your score: {score}");
        uiControl.SetActiveFinalPanel(true);
        // Time.timeScale = 0;
    }
    
    public void RestartRun()
    {
        // Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
