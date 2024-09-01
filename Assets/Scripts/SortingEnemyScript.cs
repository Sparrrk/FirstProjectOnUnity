using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SortingEnemyScript : MonoBehaviour
{
    public static SortingEnemyScript instance;

    private void Awake()
    {
        instance = this;
    }

    List<SpriteRenderer> spriteRenderersList = new List<SpriteRenderer>();

    SpriteRenderer[] spriteRenderers;
    float[] posY;

    private void Start()
    {
        StartCoroutine(Sorter());
    }

    public void AddEnemy(SpriteRenderer spr)
    {
        spriteRenderersList.Add(spr);
    }

    public void RemoveEnemy(SpriteRenderer spr)
    {
        spriteRenderersList.Remove(spr);
    }

    IEnumerator Sorter()
    {
        yield return new WaitForSeconds(1f);

        int length = spriteRenderersList.Count;

        posY = new float[length];
        spriteRenderers = new SpriteRenderer[length];

        for (int i = 0; i < length; i++)
        {
            posY[i] = spriteRenderersList[i].transform.position.y;
            spriteRenderers[i] = spriteRenderersList[i];
        }

        Array.Sort(posY, spriteRenderers);

        for (int i = 0; i < length; i++)
        {
            spriteRenderers[i].sortingOrder = -i;
        }

        StartCoroutine(Sorter());
    }
}
