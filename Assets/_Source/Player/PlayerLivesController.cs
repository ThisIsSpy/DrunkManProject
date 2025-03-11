using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerLivesController
    {
        private readonly PlayerLivesModel model;
        private readonly PlayerLivesView view;

        public PlayerLivesController(PlayerLivesModel model, PlayerLivesView view)
        {
            this.model = model;
            this.view = view;

            this.model.LivesReachedZero += OnAllLivesLost;
        }

        public void InvokeLivesUpdate(int livesRemoved)
        {
            model.Lives -= livesRemoved;
            if (livesRemoved < 0 && !model.HadDiedOnThisLevel) model.HadDiedOnThisLevel = true;
            view.UpdateLivesUI(model.Lives);
        }

        private void OnAllLivesLost()
        {
            SceneManager.LoadScene("Game");
        }
    }
}