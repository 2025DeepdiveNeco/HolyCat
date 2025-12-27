using UnityEngine;

public class FadeObstacle : MonoBehaviour
{
    [Header("설정")]
    public float fadeAlpha = 0.5f;    // 겹쳤을 때 알파값
    public int objectSortingOrder = 5; // 이 오브젝트의 레이어 순서

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isOnTop = false; // 플레이어가 위에 있는지 상태 저장

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        sr.sortingOrder = objectSortingOrder;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CatMove catMove = other.GetComponent<CatMove>();
            if (catMove == null) return;

            // 1. 점프 중이면 "위에 있음" 상태로 전환
            if (catMove.IsJumping())
            {
                isOnTop = true;
            }

            // 2. 상태에 따른 시각적 처리
            if (isOnTop)
            {
                // [상태: 위] 플레이어는 위로, 오브젝트는 원래대로(알파 100%)
                catMove.playerVisualSr.sortingOrder = objectSortingOrder + 1;
                sr.color = originalColor;
            }
            else
            {
                // [상태: 아래] 플레이어는 아래로, 오브젝트는 투명하게
                catMove.playerVisualSr.sortingOrder = objectSortingOrder - 1;
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAlpha);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 영역을 완전히 벗어나면 상태 초기화
            isOnTop = false;
            sr.color = originalColor;

            CatMove pc = other.GetComponent<CatMove>();
            if (pc != null)
            {
                // 플레이어 레이어를 기본값으로 복구
                pc.playerVisualSr.sortingOrder = pc.defaultSortingOrder;
            }
        }
    }
}