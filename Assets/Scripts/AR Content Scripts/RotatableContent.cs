using UnityEngine;

namespace ARudzbenik.ARContent
{
    public class RotatableContent : MonoBehaviour
    {
        [SerializeField] private bool _useFunctionality = false;
        [SerializeField] private float _rotationSpeed = 0.0f;

        private Quaternion _rotationOriginal = Quaternion.identity;

        private void Awake()
        {
            if (!_useFunctionality) Destroy(this);
            _rotationOriginal = transform.localRotation;
        }

        private void OnBecameVisible()
        {
            transform.localRotation = _rotationOriginal;
        }

        private void Update()
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) Rotate(-_rotationSpeed * Input.GetTouch(0).deltaPosition.x * Time.deltaTime);
        }

        private void Rotate(float value)
        {
            transform.Rotate(Vector3.up, value);
        }
    }
}