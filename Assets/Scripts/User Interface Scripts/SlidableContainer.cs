using DG.Tweening;
using System;
using UnityEngine;

namespace ARudzbenik.UserInterface
{
    public class SlidableContainer : MonoBehaviour
    {
        private const float _slideAnimationDuration = 0.75f;

        private Vector3 _positionOnScreen = Vector3.zero;
        private Vector2 _positionOffScreenDown = Vector2.zero;
        private Vector3 _positionOffScreenLeft = Vector3.zero;
        private Vector3 _positionOffScreenRight = Vector3.zero;
        private Vector3 _positionOffScreenUp = Vector3.zero;

        private void Awake()
        {
            _positionOnScreen = transform.position;
            _positionOffScreenDown = transform.position + Screen.height * Vector3.down;
            _positionOffScreenLeft = transform.position + Screen.width * Vector3.left;
            _positionOffScreenRight = transform.position + Screen.width * Vector3.right;
            _positionOffScreenUp = transform.position + Screen.height * Vector3.up;
        }

        public void Slide(ContainerPosition destination, Action onContainerMoved = null, bool moveInstantly = false)
        {
            transform.DOKill();

            Vector3 position = _positionOnScreen;
            switch (destination)
            {
                case ContainerPosition.OFF_SCREEN_DOWN: position = _positionOffScreenDown; break;
                case ContainerPosition.OFF_SCREEN_LEFT: position = _positionOffScreenLeft; break;
                case ContainerPosition.OFF_SCREEN_RIGHT: position = _positionOffScreenRight; break;
                case ContainerPosition.OFF_SCREEN_UP: position = _positionOffScreenUp; break;
            }

            if (moveInstantly)
            {
                transform.position = position;
                onContainerMoved?.Invoke();
                return;
            }

            transform
                .DOMove(position, _slideAnimationDuration)
                .OnComplete(() =>
                {
                    transform.position = position;
                    onContainerMoved?.Invoke();
                });
        }
    }

    public enum ContainerPosition
    {
        ON_SCREEN,
        OFF_SCREEN_DOWN,
        OFF_SCREEN_LEFT,
        OFF_SCREEN_RIGHT,
        OFF_SCREEN_UP,
    }
}