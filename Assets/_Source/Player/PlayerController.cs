using Movement;

namespace Player
{
    public class PlayerController
    {
        private readonly PlayerView view;

        public PlayerController(PlayerView view)
        {
            this.view = view;
        }

        public void InvokeUpdatePlayerDirection(Direction direction)
        {
            view.UpdatePlayerDirection(direction);
        }
    }
}