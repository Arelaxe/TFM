using UnityEngine;
using System.Collections;

public class HackingEndArea : MonoBehaviour
{
    [SerializeField]
    private HackingManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnergyFlowController flow = collision.gameObject.GetComponent<EnergyFlowController>();
        if (flow)
        {
            StartCoroutine(manager.End());
        }
    }
}
