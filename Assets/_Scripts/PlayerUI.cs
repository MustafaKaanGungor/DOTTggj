using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image dashBar;
    [SerializeField] private Image healthSlider;

    private void Start()
    {
        Player.Instance.OnPlayerHealthUpdated += PlayerOnPlayerHealthUpdated;
        Player.Instance.OnPlayerDashUpdated += PLayerOnPlayerDashUpdated; 
        healthSlider.fillAmount = 1f;
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
