using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealtBarHandler : MonoBehaviour
{
    [SerializeField] private Slider bossHealtBar;

    void Start()
    {
        bossHealtBar.maxValue = Boss.Instance.bossHealthCurrent;
    }
    
    void Update()
    {
        bossHealtBar.value = Boss.Instance.bossHealthCurrent;

    }
}
