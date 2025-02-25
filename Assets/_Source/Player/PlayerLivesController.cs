using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerLivesController
    {
        private PlayerLivesModel model;
        private PlayerLivesView view;

        public PlayerLivesController(PlayerLivesModel model, PlayerLivesView view)
        {
            this.model = model;
            this.view = view;

            this.model.LivesReachedZero += OnAllLivesLost;
        }

        public void InvokeLivesUpdate(int livesRemoved)
        {
            model.Lives -= livesRemoved;
            view.UpdateLivesUI(model.Lives);
        }

        private void OnAllLivesLost()
        {
            SceneManager.LoadScene("Game");
        }
    }
}