using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class ScalableContent : MonoBehaviour
    {
        [SerializeField] private bool _useFunctionality = false;
        [SerializeField] private Vector2 _scaleRange = Vector2.zero;
        [SerializeField] private float _scalingSpeed = 0.0f;

        private Vector3 _scaleOriginal = Vector3.zero;

        private void Awake()
        {
            if (!_useFunctionality) Destroy(this);
            _scaleOriginal = transform.localScale;
        }

        private void OnBecameVisible()
        {
            transform.localScale = _scaleOriginal;
        }

        private void Update()
        {
            if (Input.touchCount == 2)
            {
                float distanceOld = ((Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition) - (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition)).magnitude;
                float distanceNew = (Input.GetTouch(1).position - Input.GetTouch(0).position).magnitude;
                Scale(_scalingSpeed * Time.deltaTime * (distanceNew - distanceOld));
            }
        }

        private void Scale(float value)
        {
            float scaleFactor = Mathf.Clamp(transform.localScale.x + value, _scaleRange.x, _scaleRange.y);
            transform.localScale = scaleFactor * Vector3.one;
        }
    }
}