using UnityEngine;

public sealed class WallTrigger : MonoBehaviour
{
    [Header("This wall side")]
    [SerializeField] private Facing wallSide = Facing.Left; // 인스펙터에서 Left/Right 지정

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;

        // 개구리 쪽 컴포넌트들 가져오기
        var stick = other.GetComponent<WallStickHandler>();
        var state = other.GetComponent<FrogState>();

        if (stick == null) return;

        // 기존 벽붙기 로직
        if (stick != null)
            stick.NotifyWall(other);
        else
            stick.NotifyWall(other); // (너 구조에 맞게 택1)

        // 스프라이트/상태 갱신
        if (state != null && !state.IsDead)
            state.SetOnWall(true, wallSide);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;

        var stick = other.GetComponent<WallStickHandler>();
        var state = other.GetComponent<FrogState>();

        if (stick == null) return;

        if (stick != null)
            stick.NotifyWallExit(other);
        else
            stick.NotifyWallExit(other); // (너 구조에 맞게 택1)

        if (state != null && !state.IsDead)
            state.SetOnWall(false, state.CurrentFacing);
    }
}
