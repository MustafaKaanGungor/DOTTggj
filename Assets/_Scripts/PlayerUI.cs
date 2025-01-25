using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image dashBar;
    [SerializeField] private Image healthSlider;

    private void Start() {
        Player.Instance.OnPlayerHealthUpdated += PlayerOnPlayerHealthUpdated;
        healthSlider.fillAmount = 1f;
    }

    private void PlayerOnPlayerHealthUpdated(object sender, EventArgs e)
    {
        healthSlider.fillAmount = Player.Instance.GetBubbleAirPercentage();
    }

    public void UpdateVisuals() {
        //dashBar.fillAmount = Player.Instance.
        //healthSlider.value = Player.Instance.GetBubbleAirCurrent();
    }
}
