using System;
using UnityEngine;

public sealed class FailTrigger : MonoBehaviour
{
    [SerializeField] private FrogState frogState;

    // 게임오버 이벤트
    public static event Action OnAnyGameOver;    
    private bool triggered;

    public void ResetTrigger()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("첫 if문 통과 전");
        if (triggered) return;
        Debug.Log("두 번째 if문 통과 전");
        if (!other.CompareTag("Frog")) return;

        triggered = true;
        if (frogState != null)
            frogState.SetDead(true);

        // 게임오버 이벤트 발행
        Debug.Log("if문 다 통과");
        OnAnyGameOver?.Invoke();

        triggered = false;
    }
}