using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Image fadeImage;

    public GameObject bg;
    public GameObject overGame;
    public GameObject buttonGamePlay;
    public AudioSource source;

    private Blade blade;
    private Spawner spawner;

    private int head = 0;


    private int score = 0;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();

        bg.SetActive(false);
        overGame.SetActive(false);
        buttonGamePlay.SetActive(false);
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        
        if (head < 2)
        {
            bg.SetActive(false);
            overGame.SetActive(false);
            buttonGamePlay.SetActive(false);

            Time.timeScale = 1f;

            blade.enabled = true;
            spawner.enabled = true;

            
            scoreText.text = score.ToString();

            ClearScene();
        }
        else
        {
            score = 0;
            ClearScene();
            spawner.enabled = false;

            bg.SetActive(true);
            overGame.SetActive(true);
            buttonGamePlay.SetActive(true);
        }
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;



        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = UnityEngine.Color.Lerp(UnityEngine.Color.clear, UnityEngine.Color.white, t);
            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;
            
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);

        NewGame();
        
        elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = UnityEngine.Color.Lerp(UnityEngine.Color.white, UnityEngine.Color.clear, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        head++;
        Debug.Log(head);
    }

    public void RestartGame()
    {
        head = 0;
        source.Play();
        NewGame();
    }
}
