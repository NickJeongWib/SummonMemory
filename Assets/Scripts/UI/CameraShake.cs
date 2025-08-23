using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float _duration, float _magnitude)
    {
       Vector3 originalPos = transform.position;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            float offsetX = Random.Range(-1f, 1f) * _magnitude;
            float offsetY = Random.Range(-1f, 1f) * _magnitude;

            transform.position = new Vector3(originalPos.x + offsetX, originalPos.y + offsetY, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원래 위치로 복구
        transform.position = originalPos;
    }
}
