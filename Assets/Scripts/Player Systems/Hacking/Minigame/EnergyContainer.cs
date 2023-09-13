using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyContainer : MonoBehaviour
{
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private float fillDelay;

    [Header("Audio")]
    [SerializeField]
    private AudioClip increaseSound;

    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;
    }

    public IEnumerator AddEnergy(List<EnergyPoint> energy)
    {
        foreach (EnergyPoint point in energy)
        {
            SoundManager.Instance.PlayEffectOneShot(increaseSound);
            slider.value++;
            if (slider.value == slider.maxValue) break;
            yield return new WaitForSeconds(fillDelay);
        }
    }

    public int GetEnergyLevel()
    {
        int energyLevel = 1;
        if (maxValue * 0.5f < slider.value && slider.value <= maxValue * 0.85f)
        {
            energyLevel = 2;
        }
        else if (maxValue * 0.85f < slider.value && slider.value <= maxValue)
        {
            energyLevel = 3;
        }
        return energyLevel;
    }
}
