using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance { get; private set; }
    [SerializeField] private Image dashBar;
    [SerializeField] private Image healthSlider;
    [SerializeField] private Image flashEffectImage;

    private void Start()
    {
        Player.Instance.OnPlayerHealthUpdated += PlayerOnPlayerHealthUpdated;
        Player.Instance.OnPlayerDashUpdated += PLayerOnPlayerDashUpdated; 
        healthSlider.fillAmount = 1f;
    }
    private void Awake()
    {
        Instance = this;
    }

    private void PLayerOnPlayerDashUpdated(object sender, EventArgs e)
    {
        float[] dashParametres = (float[])sender;
        StartCoroutine(DashCoolDownResetCoroutine(dashParametres[0], dashParametres[1]));
    }

    private void PlayerOnPlayerHealthUpdated(object sender, EventArgs e)
    {
        healthSlider.fillAmount = Player.Instance.GetBubbleAirPercentage();
    }
    public IEnumerator ShowHealFlash()
    {
        float duration = 0.2f; // Efektin süresi
        float elapsedTime = 0f;

        Color initialColor = flashEffectImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f); // Yarý þeffaf kýrmýzý

        // Alpha deðerini artýr
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            flashEffectImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        elapsedTime = 0f;

        // Alpha deðerini azalt
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            flashEffectImage.color = Color.Lerp(targetColor, initialColor, elapsedTime / duration);
            yield return null;
        }

        flashEffectImage.color = initialColor;
    }

    public void UpdateVisuals()
    {
        //dashBar.fillAmount = Player.Instance.
        //healthSlider.value = Player.Instance.GetBubbleAirCurrent();
    }

    IEnumerator DashCoolDownResetCoroutine(float dashingTime, float dashingCooldown)
    {
        float elapsed = 0f;
        while (elapsed < dashingTime)
        {
            dashBar.fillAmount = Mathf.Lerp(1f, 0f, elapsed / dashingTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dashBar.fillAmount = 0f;

        elapsed = 0f;
        while (elapsed < dashingTime)
        {
            dashBar.fillAmount = Mathf.Lerp(0f, 1f, elapsed / dashingCooldown);
            elapsed += Time.deltaTime;
            yield return null;
        }
        dashBar.fillAmount = 1f;
    }
}
