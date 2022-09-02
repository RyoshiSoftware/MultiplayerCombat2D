using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseAfterSeconds : MonoBehaviour
{
    [SerializeField] float releaseAfterSeconds;
    [SerializeField] float fadeTime;
    SpawnableObject spawn;
    SpriteRenderer sprite;

    void Awake() 
    {
        spawn = GetComponent<SpawnableObject>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        StartCoroutine(ReleaseCoroutine(fadeTime));
    }

    void Release()
    {
        sprite.color = Color.white;
        spawn.ReleaseObject();
    }

    IEnumerator ReleaseCoroutine(float fadeTime)
    {
        yield return new WaitForSeconds(releaseAfterSeconds);

        float alpha = sprite.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            sprite.color = newColor;
            yield return null;
        }

        Release();
    }
}
