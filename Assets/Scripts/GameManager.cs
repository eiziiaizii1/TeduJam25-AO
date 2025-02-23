using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 spawnPosition;
    public float levelTime = 30f;
    private float timer;
    private bool isTimeRunning = true;

    public TMP_Text timerText;
    public TMP_Text deathMessage;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;

    public Dictionary<int, float> levelDurations = new Dictionary<int, float>()
    {
        { 1, 30f },  // Level 1 30 sec
        { 2, 40f },  // Level 2 40 sec
        { 3, 50f },  // Level 3 50 sec
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    void Start()
    {
#if UNITY_WEBGL
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.ForceSoftware);
#else
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
#endif

        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Start");
        if (spawnPoint != null)
        {
            spawnPosition = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("No SpawnPoint!");
        }

        timer = levelTime; // Reset timer
        SetLevelTimer();

        if (deathMessage != null)
        {
            deathMessage.gameObject.SetActive(false);
        }
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
            Debug.LogWarning("No time is defined, default time: 30 seconds.");
            timer = 30f;
        }
    }

    void TimerEnded()
    {
        Debug.Log("Time's up! Teleported to start pos...");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerDied(player);
            ResetTimer();
        }
    }

    public void PlayerDied(GameObject player)
    {
        Debug.Log("You are dead, teleported to start pos...");
        if (deathMessage != null)
        {
            SoundManager.Instance.PlayEffectSound(SoundManager.Instance.DamageSE);
            StartCoroutine(ShowDeathMessage());
        }
        player.transform.position = spawnPosition;
    }

    public void NextLevel()
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
            SoundManager.Instance.PlayEffectSound(SoundManager.Instance.LvlCompletedSE);
            SoundManager.Instance.StopRunSound();
        }
        else
        {
            Debug.Log("Congrats! All levels are completed.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void ResetTimer()
    {
        SetLevelTimer();
        isTimeRunning = true;
    }

    IEnumerator ShowDeathMessage()
    {
        deathMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        deathMessage.gameObject.SetActive(false);
    }
}
