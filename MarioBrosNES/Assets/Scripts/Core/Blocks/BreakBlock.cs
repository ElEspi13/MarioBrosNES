using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : BlockBase
{
    [SerializeField] private bool isBreakable;
    [SerializeField] private Sprite usedSprite;
    [SerializeField] private bool isUsed;
    protected override void OnHit()
    {
        if (isBreakable) {
            GetComponent<SpriteRenderer>().sprite = null;
            StartCoroutine(WaitDestroy());
            return;
        }
        if (isUsed) return;
        
        isUsed = true;
        GetComponent<SpriteRenderer>().sprite = usedSprite;
        base.OnHit();
    }

    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
