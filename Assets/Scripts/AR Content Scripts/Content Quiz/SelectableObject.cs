using UnityEngine;

namespace ARudzbenik.ARContent.ContentQuiz
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _correctColor = Color.white;
        [SerializeField] private Color _incorrectColor = Color.white;

        private Material _originalMaterial = null;
        private Renderer _renderer = null;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _originalMaterial = _renderer.material;
        }

        private void OnMouseDown()
        {
            ContentWithQuiz.OnObjectSelectedAction?.Invoke(this);
        }

        public void UpdateCorrectnessVisual(bool isCorrect)
        {
            if (_renderer)
            {
                Material material = new Material(_originalMaterial);
                material.color = isCorrect ? _correctColor : _incorrectColor;
                _renderer.material = material;
            }
        }

        public void UpdateSelectionVisual(bool isSelected)
        {
            if (_renderer)
            {
                Material material = new Material(_originalMaterial);
                material.color = isSelected ? _selectedColor : _originalMaterial.color;
                _renderer.material = material;
            }
        }
    }
}