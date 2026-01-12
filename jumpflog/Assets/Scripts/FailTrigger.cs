using System;
using UnityEngine;

public sealed class FailTrigger : MonoBehaviour
{
    //[SerializeField] private FrogState frogState;

    // 게임오버 이벤트
    public event Action OnGameOver;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Frog")) return;

        triggered = true;

        //if (frogState != null)
            //frogState.SetDead(true);

        // 게임오버 이벤트 발행
        OnGameOver?.Invoke();
    }
}