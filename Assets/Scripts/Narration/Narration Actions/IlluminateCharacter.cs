using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminateCharacter : NarrationAction
{
    [SerializeField]
    private Color illumColor;
    [SerializeField]
    private bool isRyo;
    private Color originalColor;
    private bool canBlink;
    private SpriteRenderer objectToIllum;

    public override void Execute()
    {
        canBlink = true;
        objectToIllum = PlayerManager.Instance.GetDualCharacterController().GetCharacter(isRyo).GetComponent<SpriteRenderer>();
        originalColor = objectToIllum.color;
        PlayerManager.Instance.StartCoroutine(Illuminate(0.5f));
    }

    public override void EndAction()
    {
        canBlink = false;
        PlayerManager.Instance.StopCoroutine("Illuminate");
        objectToIllum.color = originalColor;
    }

    IEnumerator Illuminate( float delayBetweenBlinks)
    {
        while( canBlink )
        {
            objectToIllum.color = objectToIllum.color == originalColor ? illumColor : originalColor;
            yield return new WaitForSeconds( delayBetweenBlinks );
        }
    }
}
