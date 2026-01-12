using UnityEngine;

/// <summary>
/// Rigidbody2D/3D의 회전을 막는 스크립트.
/// - 2D: FreezeRotation 사용
/// - 3D: X/Y/Z 회전 Lock
/// </summary>
[DisallowMultipleComponent]
public sealed class DisableRigidbodyRotation : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private bool applyOnAwake = true;

    [Tooltip("회전값이 이미 틀어져 있으면 0으로 리셋")]
    [SerializeField] private bool resetRotationToZero = false;

    [Tooltip("매 프레임 강제로 회전 고정(외부에서 constraints를 바꾸는 경우 대비)")]
    [SerializeField] private bool enforceEveryFrame = false;

    private Rigidbody2D rb2D;
    private Rigidbody rb3D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb3D = GetComponent<Rigidbody>();

        if (applyOnAwake) Apply();
    }

    private void FixedUpdate()
    {
        if (!enforceEveryFrame) return;
        Apply();
    }

    [ContextMenu("Apply (Disable Rotation)")]
    public void Apply()
    {
        if (rb2D != null)
        {
            rb2D.freezeRotation = true;

            if (resetRotationToZero)
                rb2D.SetRotation(0f);

            return;
        }

        if (rb3D != null)
        {
            rb3D.constraints |= RigidbodyConstraints.FreezeRotation;

            if (resetRotationToZero)
                rb3D.MoveRotation(Quaternion.identity);
        }
    }
}
