namespace BingoGame.Commands
{
    internal class InitializeCardViewCommand
    {
        private readonly BingoCardView _cardView;
        
        public InitializeCardViewCommand(BingoCardView cardView)
        {
            _cardView = cardView;
        }
        
        public BingoGameState Execute(BingoGameState gameState)
        {
            _cardView.Initialize(gameState);
            return gameState;
        }
    }
}