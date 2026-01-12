using Unity.VisualScripting;
using UnityEngine;

public class CameraClimb : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;

    [SerializeField] private float maxY;
    [SerializeField] private float yVelocity;
    public float MaxY => maxY;

    [SerializeField] private bool isActive = true;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private bool useSmoothDamp = true;
    [SerializeField] private float smoothTime = 0.12f;

    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
    }

    public float TopY
    {
        get
        {
            if (cam == null) return transform.position.y;
            return transform.position.y + cam.orthographicSize;
        }
    }

    public float BottomY
    {
        get
        {
            if (cam == null) return transform.position.y;
            return transform.position.y - cam.orthographicSize;
        }
    }

    public void SetTarget(Transform newTarget) //카메라가 따라갈 타겟 설정
    {
        if (newTarget == null)
        {
            Debug.LogWarning("[CameraClimb] SetTarget called with null");
            target = null;
            return;
        }

        target = newTarget;
    }

    public void ResetY(float y) //
    {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;

        maxY = y;

        yVelocity = 0f;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
    
    public void Tick(float dt) //카메라 위치 조정
    {
        if (!isActive) return;
        if (target == null) return;

        float currentY = transform.position.y;
        float desiredY = target.position.y + offsetY;

        

        float nextY;
        if (useSmoothDamp)
            nextY = Mathf.SmoothDamp(currentY, desiredY, ref yVelocity, smoothTime);
        else
            nextY = Mathf.Lerp(currentY, desiredY, 1f - Mathf.Exp(-dt * 20f));

        Vector3 pos = transform.position;
        pos.y = nextY;
        transform.position = pos;
    }
}
