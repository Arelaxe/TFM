using UnityEngine;

public class BinaryMovable : MonoBehaviour
{
    private Vector2 initPosition;
    private Vector2 targetPosition;

    [SerializeField]
    private Vector2 movement;

    [SerializeField]
    private float speed;

    private bool moving;
    private bool atTarget;

    private void Awake()
    {
        initPosition = transform.position;
        targetPosition = initPosition + movement;

        InitDynamicObject();
    }

    void Update()
    {
        if (moving)
        {
            Vector3 target = atTarget ? initPosition : targetPosition;

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                moving = false;
                atTarget = !atTarget;
            }
        }
    }

    public void Move()
    {
        if (moving)
        {
            atTarget = !atTarget;
        }

        moving = true;
    }

    private void InitDynamicObject()
    {
        DynamicObject dynamicObject = GetComponent<DynamicObject>();
        dynamicObject.OnPrepareToSave += PrepareToSaveObjectState;
        dynamicObject.OnLoadObjectState += LoadObjectState;
    }

    private void PrepareToSaveObjectState(ObjectState objectState)
    {
        objectState.extendedData[GetType().Name] = new BinaryMovableData(initPosition.x, initPosition.y, moving, atTarget);
    }

    private void LoadObjectState(ObjectState objectState)
    {
        BinaryMovableData data = PersistenceUtils.Get<BinaryMovableData>(objectState.extendedData[GetType().Name]);
        initPosition = data.InitPosition;
        targetPosition = initPosition + movement;
        moving = data.moving;
        atTarget = data.atTarget;
    }
}
