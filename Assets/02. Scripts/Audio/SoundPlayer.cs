using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;

    [Header("이동 및 액션")]
    //public AudioClip walkFootstep; // 발소리?
    public AudioClip jumpSound;    // 점프 소리                

    public void PlayAttackSound(int step)
    {
        AudioClip clip = step == 1 ? attack1 : (step == 2 ? attack2 : attack3);
        PlaySound(clip);
    }
    //public void PlayFootstep()
    //{
    //    PlaySound(walkFootstep, 0.8f);
    //}


    public void PlayJump()
    {
        PlaySound(jumpSound);
    }
    

    private void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clip, volume);
        }
    }
}