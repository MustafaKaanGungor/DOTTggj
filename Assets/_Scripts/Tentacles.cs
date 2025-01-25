using System;
using System.Collections;
using UnityEngine;

public class Tentacles : MonoBehaviour
{
    [SerializeField] private GameObject tentacle;

    [SerializeField] private TentacleType tentacleType;

    private readonly float life = 0.5f;

    private bool isAttacking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            // can azalt
        }
    }

    private void OnEnable()
    {
        switch (tentacleType)
        {
            case TentacleType.BottomUp:
                StartCoroutine(TentacleBottomUpAttackCoroutine());
                break;
            case TentacleType.Rotate:
                StartCoroutine(TentacleRotateAttackCoroutine());
                break;
            default:
                break;
        }
    }

    IEnumerator TentacleBottomUpAttackCoroutine()
    {
        yield return new WaitForSeconds(life);
        tentacle.SetActive(true);
        isAttacking = true;
        yield return new WaitForSeconds(life);
        isAttacking = false;
        tentacle.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator TentacleRotateAttackCoroutine()
    {
        isAttacking = true;
        float duration = life;
        Vector3 startScale = tentacle.transform.localScale;
        Vector3 targetScale = new Vector3(1f, startScale.y, startScale.z);
        Quaternion startRotation = tentacle.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, tentacle.transform.eulerAngles.z - 90);

        yield return LerpTransform(duration, t =>
        {
            tentacle.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
        });

        yield return new WaitForSeconds(duration);

        yield return LerpTransform(duration, t =>
        {
            tentacle.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        });

        yield return new WaitForSeconds(duration);

        yield return LerpTransform(duration, t =>
        {
            tentacle.transform.localScale = Vector3.Lerp(targetScale, startScale, t);
        });

        isAttacking = false;
        gameObject.SetActive(false);
    }

    IEnumerator LerpTransform(float duration, Action<float> updateAction)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            updateAction(Mathf.Clamp01(elapsedTime / duration));
            yield return null;
        }
        updateAction(1f);
    }
}

public enum TentacleType
{
    BottomUp,
    Rotate
}
