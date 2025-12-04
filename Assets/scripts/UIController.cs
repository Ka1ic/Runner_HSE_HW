using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject indestructibleBonusPanel;
    [SerializeField] private UnityEngine.UI.Slider indestructibleBonusSlider;
    [SerializeField] private float frequencySliderUpdate = 0.05f;

    private Coroutine indestructibleCoroutine= null;
    public void StartIndestructibleBonus(float duration)
    {
        indestructibleBonusPanel.SetActive(true);
        if (indestructibleCoroutine != null) StopCoroutine(indestructibleCoroutine);
        
        indestructibleCoroutine = StartCoroutine(UpdateSlider(duration, duration));
    }

    private IEnumerator UpdateSlider(float duration, float stillDuration)
    {
        indestructibleBonusSlider.value = stillDuration / duration;
        yield return new WaitForSeconds(frequencySliderUpdate);

        float newDuration = stillDuration - frequencySliderUpdate;
        if (newDuration  > 0) indestructibleCoroutine = StartCoroutine(UpdateSlider(duration, newDuration));
        else
        {
            indestructibleBonusPanel.SetActive(false);
            indestructibleCoroutine = null;
        }
    }
    
    public void SetActiveFinalPanel(bool state)
    {
        losePanel.SetActive(state);
    }
    
    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }
    
    public void SetFinalScoreText(string text)
    {
        finalScoreText.text = text;
    }
    
    public void SetHearts(int numberHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numberHearts)
                hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);
        }
    }
}
