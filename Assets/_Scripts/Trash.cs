using System.Collections;
using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public TrashType trashType;

    [SerializeField] private float triggerTime = 10f;
    private Collider2D trashCollider;
    private SpriteRenderer sr;

    [Header("Floaty Movement")]
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float floatRange = 0.5f;
    private Vector3 startPosition;
    private bool isFloating = false;

    private void Awake()
    {
        trashCollider = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }

    private void Start()
    {
        isFloating = true;
        StartCoroutine(DisableTriggerAfterTime());
    }

    private void FixedUpdate()
    {
        if (isFloating)
            FloatingEffect();
    }

    private IEnumerator DisableTriggerAfterTime()
    {
        yield return new WaitForSeconds(triggerTime);

        if (trashCollider != null)
        {
            trashCollider.isTrigger = false;
            sr.color = Color.gray;
            isFloating = false;
            FloatingEffect(false);
        }

        yield break;
    }

    private void FloatingEffect(bool isFloating = true)
    {
        if (isFloating == true)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            transform.position = startPosition + new Vector3(0, yOffset);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}