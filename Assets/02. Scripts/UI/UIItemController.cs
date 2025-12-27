using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 마우스 이벤트를 위해 필수

public class UIItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Animator anim;
    private bool isSelected = false;

    // "다른 이미지를 클릭하면 풀리는" 기능을 위해 static이나 매니저가 필요함
    public static UIItemController currentSelected;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 1. 마우스가 올라갈 때 (Hover 시작)
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("IsHover", true);
    }

    // 2. 마우스가 나갈 때 (Hover 종료)
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("IsHover", false);
    }

    // 3. 마우스를 클릭할 때 (Click + Selected)
    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭 애니메이션 실행
        anim.SetTrigger("Click");

        // 선택 상태 처리
        SelectThisItem();
    }

    public void SelectThisItem()
    {
        // 이전에 선택된 게 있다면 해제
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.Deselect();
        }

        // 자신을 선택 상태로 변경
        isSelected = true;
        currentSelected = this;
        anim.SetBool("IsSelected", true);
    }

    public void Deselect()
    {
        isSelected = false;
        anim.SetBool("IsSelected", false);
    }
}