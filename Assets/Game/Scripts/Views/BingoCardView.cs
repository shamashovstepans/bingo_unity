using System;
using UnityEngine;

namespace BingoGame.Commands
{
    internal class BingoCardView : MonoBehaviour
    {
        public event Action<Vector2Int> OnCellClicked;

        [SerializeField] private BingoCellView _cellPrefab;

        private BingoCellView[][] _cells;

        private BingoGameState _state;

        public void Initialize(BingoGameState state)
        {
            _state = state;
            _state.Updated += OnGameStateUpdated;
            _cells = new BingoCellView[BingoCard.SIZE][];
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                _cells[i] = new BingoCellView[BingoCard.SIZE];
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    var cell = Instantiate(_cellPrefab, transform);
                    cell.Initialize(new Vector2Int(i, j), state.Card.cells[i][j].title);
                    cell.OnClicked += OnCellClickedHandler;
                    _cells[i][j] = cell;
                }
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < BingoCard.SIZE; i++)
            {
                for (var j = 0; j < BingoCard.SIZE; j++)
                {
                    _cells[i][j].OnClicked -= OnCellClickedHandler;
                    Destroy(_cells[i][j].gameObject);
                }
            }

            _state.Updated -= OnGameStateUpdated;
            _state = null;
            _cells = null;
        }

        private void OnGameStateUpdated()
        {
        }

        private void OnCellClickedHandler(Vector2Int obj)
        {
            OnCellClicked?.Invoke(obj);
        }
    }
}