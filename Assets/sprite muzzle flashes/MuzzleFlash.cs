using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite[] sprites;

    int nextFrame;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / 30.0f, 1 / 30.0f);
    }

    void NextFrame()
    {
        Debug.Log("1");
        spriteRenderer.sprite = sprites[nextFrame];
        nextFrame++;
        nextFrame =nextFrame % sprites.Length;
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Close());
    }
}