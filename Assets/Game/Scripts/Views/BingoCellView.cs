using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BingoGame.Commands
{
    internal class BingoCellView : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Vector2Int> OnClicked;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _background;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _markedColor = Color.green;

        public Vector2Int Coordinates;
        private bool _isMarked;

        public void Initialize(Vector2Int coordinates, string text)
        {
            Coordinates = coordinates;
            _text.text = text;
        }

        private void OnEnable()
        {
            _background.color = _defaultColor;
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _isMarked = !_isMarked;
            if (_isMarked)
            {
                _background.color = _markedColor;
                Taptic.Medium();
            }
            else
            {
                _background.color = _defaultColor;
                Taptic.Light();
            }

            OnClicked?.Invoke(Coordinates);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // _background.color = Color.red;
            // OnClicked?.Invoke(Coordinates);
        }
    }
}