using UnityEngine;

public class UpdateBoxTriggerAction : Action
{
    [SerializeField]
    private GameObject trigger;

    [SerializeField]
    private Vector2 movement;
    [SerializeField]
    private Vector2 colliderOffset;
    [SerializeField]
    private Vector2 colliderSize;

    public override void Execute()
    {
        Vector3 currentPosition = trigger.transform.position;
        trigger.transform.position = new(currentPosition.x + movement.x, currentPosition.y + movement.y, currentPosition.z);

        BoxCollider2D collider = trigger.GetComponent<BoxCollider2D>();
        collider.offset = new(collider.offset.x + colliderOffset.x, collider.offset.y + colliderOffset.y);
        collider.size = new(collider.size.x + colliderSize.x, collider.size.y + colliderSize.y);
    }

    protected override void PrepareToSaveObjectState(ObjectState objectState)
    {
        BoxCollider2D collider = trigger.GetComponent<BoxCollider2D>();
        objectState.extendedData[GetPersistentName()] = new TriggerData(timesExecuted, collider.enabled, collider.offset.x, collider.offset.y, collider.size.x, collider.size.y);
    }

    protected override void LoadObjectState(ObjectState objectState)
    {
        TriggerData data = PersistenceUtils.Get<TriggerData>(objectState.extendedData[GetPersistentName()]);

        timesExecuted = data.timesExecuted;

        BoxCollider2D collider = trigger.GetComponent<BoxCollider2D>();
        collider.enabled = data.enabled;
        collider.offset = data.Offset;
        collider.size = data.Size;
    }
}
