using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 spawnPosition;
    public float levelTime = 30f; // Bölüm süresi (saniye)
    private float timer;
    private bool isTimeRunning = true;

    public TMP_Text timerText; // UI Timer için

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;

    public Dictionary<int, float> levelDurations = new Dictionary<int, float>()
    {
        { 1, 30f },  // Level 1 süresi (30 saniye)
        { 2, 45f },  // Level 2 süresi (45 saniye)
        { 3, 60f },  // Level 3 süresi (60 saniye)
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), cursorMode);

        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Start");
        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("SpawnPoint bulunamadı! Lütfen sahnede bir SpawnPoint nesnesi oluştur.");
        }

        timer = levelTime; // Zamanı sıfırla
    }

    void Update()
    {
        if (isTimeRunning)
        {
            timer -= Time.deltaTime;
            if (timerText != null)
                timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                isTimeRunning = false;
                TimerEnded();
            }
        }
    }

    void SetLevelTimer()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (levelDurations.ContainsKey(currentScene))
        {
            timer = levelDurations[currentScene];
        }
        else
        {
            Debug.LogWarning("Bu sahne için süre belirlenmemiş! Varsayılan süre: 30 saniye.");
            timer = 30f; // Varsayılan süre
        }
    }

    void TimerEnded()
    {
        Debug.Log("Süre bitti! Yeniden başlıyorsun...");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerDied(player);
        }
    }

    public void PlayerDied(GameObject player)
    {
        Debug.Log("Öldün! Başlangıç noktasına ışınlanıyorsun...");
        player.transform.position = spawnPosition;
        ResetTimer();
    }

    public void NextLevel()
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Tebrikler! Tüm bölümleri tamamladın.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void ResetTimer()
    {
        SetLevelTimer();
        isTimeRunning = true;
    }
}
