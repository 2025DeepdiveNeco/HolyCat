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
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("게임 설정")]
    float totalTime = 300f;   // 제한 시간
    public int targetScore;   // 목표 점수

    [Header("Stage")]
    int currentStage;

    private float currentTime;
    private int currentScore = 0;
    private bool isGameOver = false;


    protected override void Init()
    {
        isDestroyOnLoad = true;
        base.Init();
    }

    void Start()
    {
        currentStage = PlayerPrefs.HasKey("Stage") ? PlayerPrefs.GetInt("Stage") : 1;
        targetScore = DataTableManager.Instance.GetScoreData(currentStage).Gaol_Score;
        totalTime = DataTableManager.Instance.GetScoreData(currentStage).MaxTime;
        currentTime = totalTime;
        Debug.Log($"목표 점수 :{targetScore} / 현재 Stage : {currentStage}");

        // 게이지 초기화
        scoreGauge.minValue = 0;
        scoreGauge.maxValue = targetScore;
        scoreGauge.value = 0;

        successPanel.SetActive(false);
        failurePanel.SetActive(false);
        Time.timeScale = 1f; // 게임 속도 정상화
        scoreText.text = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetInt("Score").ToString() : "0";
    }

    void Update()
    {
        //if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    Debug.Log("스페이스바 입력됨 (New Input System)");
        //    AddScore(10);
        //}
        if (targetScore <= currentScore) CheckGameOver();   // 목표 점수 도달 시 게임 종료
            
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
        scoreText.text = currentScore.ToString();
    }

    // 게임 종료 판정
    public void CheckGameOver()
    {
        isGameOver = true;
        //Time.timeScale = 0f;

        int stageIdx = PlayerPrefs.GetInt("SelectedStage", 1);

        if (currentScore >= targetScore)
        {
            // [성공]
            successPanel.SetActive(true);

            // 1. 현재 스테이지를 '완전 열림(2)' 상태로 저장
            PlayerPrefs.SetInt($"Stage_{stageIdx}_State", 2);

            // 2. 다음 스테이지가 있다면 '반쯤 열림(1)'으로 해금 (이미 클리어한 게 아니라면)
            if (stageIdx < 5)
            {
                int nextState = PlayerPrefs.GetInt($"Stage_{stageIdx + 1}_State", 0);
                if (nextState == 0)
                    PlayerPrefs.SetInt($"Stage_{stageIdx + 1}_State", 1);
            }
        }
        else
        {
            // [실패]
            failurePanel.SetActive(true);
            // 실패 시에는 아무것도 저장하지 않으므로, 
            // 선택 씬으로 돌아가면 자동으로 '반쯤 열림(1)' 상태가 유지됩니다.
        }
    }

    // 재시작 버튼 기능
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextStage()
    {
        currentStage++;
        PlayerPrefs.SetInt("Stage", currentStage);
        PlayerPrefs.SetInt("Score", currentScore);
        // TODO 시간 저장할거임?
        SceneManager.LoadScene($"Stage{currentStage}");
        //SceneLoader.Instance.LoadWithDelay($"Stage{currentStage}", 1f);
    }
    public void GoToSelectScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageSelectScene");
    }
}