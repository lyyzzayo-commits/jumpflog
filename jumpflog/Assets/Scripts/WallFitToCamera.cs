using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public sealed class WallFitToCamera : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Camera targetCamera;   // 보통 MainCamera
    [SerializeField] private SpriteRenderer sr;     // DrawMode = Tiled 권장
    [SerializeField] private BoxCollider2D col;

    [Header("Behavior")]
    [Tooltip("벽의 바닥 기준 Y(이 지점부터 위로 벽이 채워짐).")]
    [SerializeField] private float baseY;

    [Tooltip("카메라 상단보다 얼마나 더 위까지 벽을 덮을지")]
    [SerializeField] private float topBuffer = 20f;

    [Tooltip("성능 최적화: 높이 변화가 이 값보다 작으면 갱신 생략")]
    [SerializeField] private float minUpdateDelta = 0.25f;

    private float lastAppliedHeight = -999f;

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (col == null) col = GetComponent<BoxCollider2D>();

        // baseY를 비워뒀다면 "현재 벽 위치"를 바닥 기준으로 사용
        if (Mathf.Approximately(baseY, 0f))
            baseY = transform.position.y;

        // Tiled 권장(텍스처가 늘어나지 않고 반복됨)
        // sr.drawMode = SpriteDrawMode.Tiled; // 필요하면 강제(단, 에디터 설정 권장)
    }

    private void LateUpdate()
    {
        if (targetCamera == null) return;

        float camTopY = targetCamera.transform.position.y + targetCamera.orthographicSize;
        float targetTopY = camTopY + topBuffer;

        float height = Mathf.Max(1f, targetTopY - baseY);

        // 너무 자잘한 변동은 스킵(성능)
        if (Mathf.Abs(height - lastAppliedHeight) < minUpdateDelta) return;

        ApplyHeight(height);
        lastAppliedHeight = height;
    }

    private void ApplyHeight(float height)
    {
        // Sprite 타일 길이
        Vector2 s = sr.size;
        s.y = height;
        sr.size = s;

        // Collider 길이 + 아래 기준 고정(baseY부터 위로 자라게)
        Vector2 c = col.size;
        c.y = height;
        col.size = c;

        Vector2 off = col.offset;
        off.y = height * 0.5f; // 아래(baseY)에 붙이고 위로 자라게
        col.offset = off;
    }
}
