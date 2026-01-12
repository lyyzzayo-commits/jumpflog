using UnityEngine;

public sealed class DespawnBelowLava : MonoBehaviour
{
    [SerializeField] private LavaManager lava;
    [SerializeField] private float marginBelow = 1.0f;

    private void Awake()
    {
        if (lava == null) lava = FindFirstObjectByType<LavaManager>();
    }

    private void Update()
    {
        if (lava == null) return;

        // 용암 topY보다 marginBelow만큼 더 아래로 내려가면 제거
        float killY = lava.transform.position.y - marginBelow;
        if (transform.position.y < killY)
            Destroy(gameObject);
    }
}
