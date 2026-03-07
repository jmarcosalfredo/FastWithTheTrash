using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public TrashType acceptedType;

    [Header("Debug Interacao")]
    public float interactionRadius = 1.2f;
    public LayerMask playerLayer;
    public bool drawOnlyWhenSelected = false;

    private bool isPlayerInRange;

    void Update()
    {
        // Apenas para debug visual do gizmo
        isPlayerInRange = Physics2D.OverlapCircle(transform.position, interactionRadius, playerLayer) != null;
    }

    public bool IsPlayerInsideRange(Vector2 playerPosition)
    {
        float sqrRadius = interactionRadius * interactionRadius;
        return ((Vector2)transform.position - playerPosition).sqrMagnitude <= sqrRadius;
    }

    void OnDrawGizmos()
    {
        if (drawOnlyWhenSelected) return;

        Gizmos.color = isPlayerInRange ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawOnlyWhenSelected) return;

        Gizmos.color = isPlayerInRange ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}