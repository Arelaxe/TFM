using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyContainer : MonoBehaviour
{
    [SerializeField]
    private int levelAmount;
    [SerializeField]
    private float fillDelay;

    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = levelAmount * 3;
    }

    public IEnumerator AddEnergy(List<EnergyPoint> energy)
    {
        foreach (EnergyPoint point in energy)
        {
            slider.value++;
            if (slider.value == slider.maxValue) break;
            yield return new WaitForSeconds(fillDelay);
        }
    }

    public int GetEnergyLevel()
    {
        int energyLevel = 1;
        if (levelAmount < slider.value && slider.value <= levelAmount * 2)
        {
            energyLevel = 2;
        }
        else if (levelAmount * 2 < slider.value && slider.value <= levelAmount * 3)
        {
            energyLevel = 3;
        }
        return energyLevel;
    }
}
