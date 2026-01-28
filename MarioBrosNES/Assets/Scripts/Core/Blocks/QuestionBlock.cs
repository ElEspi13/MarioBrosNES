using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : BlockBase
{
    private bool isUsed = false;
    [SerializeField] private Sprite usedSprite;

    protected override void OnHit()
    {
        if (isUsed) return;

        isUsed = true;
        GetComponent<SpriteRenderer>().sprite = usedSprite;

        base.OnHit();
    }

}
