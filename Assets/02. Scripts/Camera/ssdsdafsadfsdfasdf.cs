using UnityEngine;
using UnityEngine.Video; // 비디오 기능을 위해 필수

public class VideoQuitController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        // 비디오 재생이 끝났을 때 실행될 이벤트 연결
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        // 이벤트 연결 해제 (메모리 관리)
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer source)
    {
        Debug.Log("비디오 재생 완료. 앱을 종료합니다.");

        // 1. 실제 빌드된 게임 종료
        Application.Quit();

        // 2. 유니티 에디터 상에서 테스트 중일 때 종료 (빌드 후에는 무시됨)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}