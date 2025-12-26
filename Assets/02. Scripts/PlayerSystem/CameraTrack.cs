using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform target; // 따라갈 대상
    public Vector3 offset;   // 카메라와 대상 간의 오프셋

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }


}
