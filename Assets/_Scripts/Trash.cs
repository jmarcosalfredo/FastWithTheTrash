using System.Collections;
using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public TrashType trashType;

    [SerializeField] private float triggerTime = 10f;
    private Collider2D trashCollider;
    private SpriteRenderer sr;

    private void Awake()
    {
        trashCollider = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(DisableTriggerAfterTime());
    }

    private IEnumerator DisableTriggerAfterTime()
    {
        yield return new WaitForSeconds(triggerTime);

        if (trashCollider != null)
        {
            trashCollider.isTrigger = false;
            sr.color = Color.gray;
        }

        yield break;
    }
}