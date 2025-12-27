using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Image targetImage;      // 애니메이션을 보여줄 UI Image
    public Sprite[] sprites;       // 애니메이션 프레임들
    public float frameRate = 0.1f; // 프레임 속도

    private int currentFrame;
    private float timer;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % sprites.Length;
            targetImage.sprite = sprites[currentFrame];
        }
    }
}