using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UltraGauge : MonoBehaviour
{
    [SerializeField] private Slider ultraGaugeSlider;
    [SerializeField] private Material ultraReadyMaterial;
    [SerializeField] private Material ultraNotReadyMaterial;
    [SerializeField] private Image ultraGaugeFillImage;

    private bool isUltraActive = false;

    public void SetUp(float maxValue)
    {
        ultraGaugeSlider.maxValue = maxValue;
        ultraGaugeSlider.value = 0;
        ultraGaugeFillImage.material = ultraNotReadyMaterial;
    }

    public float SetUltraGaugeValue(float value)
    {
        if (isUltraActive) return ultraGaugeSlider.value;
        ultraGaugeSlider.value = Mathf.Clamp(value, 0, ultraGaugeSlider.maxValue);

        if (ultraGaugeSlider.value >= ultraGaugeSlider.maxValue)
        {
            Debug.Log("Ultra is ready!");

            ultraGaugeFillImage.material = ultraReadyMaterial;
        }
        else
        {
            ultraGaugeFillImage.material = ultraNotReadyMaterial;
        }

        return ultraGaugeSlider.value;
    }

    public void OnActiveUltra()
    {
        isUltraActive = true;
        DOVirtual.Float(ultraGaugeSlider.value, 0, 5.1f, (value) =>
        {
            ultraGaugeSlider.value = value;
        }).OnComplete(() =>
        {
            ultraGaugeSlider.value = 0;
            ultraGaugeFillImage.material = ultraNotReadyMaterial;
            isUltraActive = false;
        });
    }

    public bool IsUltraReady()
    {
        return ultraGaugeSlider.value >= ultraGaugeSlider.maxValue;
    }
}