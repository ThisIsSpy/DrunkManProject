using TMPro;

namespace Score
{
    public class ScoreView
    {
        private readonly TextMeshProUGUI scoreField;

        public ScoreView(TextMeshProUGUI scoreField)
        {
            this.scoreField = scoreField;
        }

        public void UpdateScoreUI(int score)
        {
            scoreField.text = score.ToString();
        }
    }
}