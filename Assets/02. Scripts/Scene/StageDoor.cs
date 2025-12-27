using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDoor : MonoBehaviour
{
    [SerializeField] int stageIndex; // 1~5
    private Animator anim;
    private int state; // 0: Locked, 1: Available, 2: Cleared

    void Start()
    {
        anim = GetComponent<Animator>();
        int stageIdx = stageIndex; // 1~5

        // PlayerPrefs에서 스테이지 상태 로드
        // 1스테이지만 기본 1(진입가능), 나머지는 0(잠금)
        int defaultVal = (stageIdx == 1) ? 1 : 0;
        int savedState = PlayerPrefs.GetInt($"Stage_{stageIdx}_State", defaultVal);

        // 애니메이터에 전달
        anim.SetInteger("State", savedState);
    }

    void LoadState()
    {
        // 1스테이지는 기본적으로 '반쯤 열림(1)' 상태, 나머지는 '닫힘(0)'이 기본값
        int defaultState = (stageIndex == 1) ? 1 : 0;
        state = PlayerPrefs.GetInt($"Stage_{stageIndex}_State", defaultState);

        // 애니메이터 파라미터 업데이트 (정수형 파라미터 "State" 가정)
        anim.SetInteger("State", state);
    }

    // 플레이어 상호작용 시 호출
    public void OnInteract() // 상호작용 시
    {
        int currentState = anim.GetInteger("State");

        if (currentState >= 1) // 진입 가능(1) 혹은 클리어(2) 상태일 때
        {
            // 문이 활짝 열리는 연출을 위해 잠시 State를 2로 변경
            anim.SetInteger("State", 2);

            // 현재 선택한 스테이지 저장 후 이동
            PlayerPrefs.SetInt("SelectedStage", stageIndex);
            Invoke("EnterStage", 0.5f); // 문 열리는 시간 잠깐 대기
        }
    }

    void GoToStage()
    {
        SceneManager.LoadScene("Stage" + stageIndex);
    }
}