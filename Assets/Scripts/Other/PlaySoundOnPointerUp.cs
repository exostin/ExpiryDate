using UnityEngine;
using UnityEngine.EventSystems;

namespace Other
{
    public class PlaySoundOnPointerUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private AudioSource sound;

        //OnPointerDown is also required to receive OnPointerUp callbacks
        public void OnPointerDown(PointerEventData eventData)
        {
        }

        //Do this when the mouse click on this selectable UI object is released.
        public void OnPointerUp(PointerEventData eventData)
        {
            sound.Play();
        }
    }
}