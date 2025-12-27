using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("UI 연결")]
    public Slider scoreGauge;       // 점수 게이지(Slider)
    public TextMeshProUGUI timerText;
    public GameObject successPanel; // 성공 UI
    public GameObject failurePanel; // 실패 UI

    [Header("게임 설정")]
    public float totalTime = 30f;   // 제한 시간
    public int targetScore = 100;   // 목표 점수

    private float currentTime;
    private int currentScore = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        isDestroyOnLoad = true;
    }
    void Start()
    {
        currentTime = totalTime;

        // 게이지 초기화
        scoreGauge.minValue = 0;
        scoreGauge.maxValue = targetScore;
        scoreGauge.value = 0;

        successPanel.SetActive(false);
        failurePanel.SetActive(false);
        Time.timeScale = 1f; // 게임 속도 정상화
    }

    void Update()
    {
        //if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    Debug.Log("스페이스바 입력됨 (New Input System)");
        //    AddScore(10);
        //}
        if (isGameOver) return;

        // 시간 차감
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            CheckGameOver();
        }
        
    }

    // 시간 표시 업데이트
    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = "Time: " + seconds.ToString();

        if (currentTime <= 5f) timerText.color = Color.red; // 5초 남으면 빨간색
        
    }

    // 점수 추가 
    public void AddScore(int amount)
    {
        if (isGameOver) return;

        currentScore += amount;
        scoreGauge.value = currentScore; // 게이지에 즉시 반영

        
    }

    // 게임 종료 판정
    void CheckGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // 게임 일시정지

        if (currentScore >= targetScore)
        {
            successPanel.SetActive(true); // 목표 달성 성공
        }
        else
        {
            failurePanel.SetActive(true); // 목표 달성 실패
        }
    }

    // 재시작 버튼 기능
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}