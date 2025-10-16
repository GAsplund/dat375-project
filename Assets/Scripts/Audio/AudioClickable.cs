using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioClickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip mouseEnterSound;
    [SerializeField] private AudioClip mouseDownSound;
    [SerializeField] private AudioClip mouseUpSound;

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance?.PlayOneShotEffect(mouseDownSound, transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.instance?.PlayOneShotEffect(mouseUpSound, transform);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance?.PlayOneShotEffect(mouseEnterSound, transform);
    }
}
