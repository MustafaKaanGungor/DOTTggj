using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealtBarHandler : MonoBehaviour
{
    [SerializeField] private Slider playerHealtBar;
    [SerializeField] private Slider bossHealtBar;

    void Start()
    {
        playerHealtBar.value = Player.Instance.bubbleAirMax;
    }
    void Update()
    {
        playerHealtBar.value = Player.Instance.bubbleAirCurrent;
    }
}
