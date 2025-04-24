namespace Module3LabC
{
    public partial class MainPage : ContentPage
    {
        private int currentQuestionIndex = 0;
        private int score = 0;
        private double xOffset = 0;

        private List<(string Question, string ImagePath)> questions = new()
    {
        ("You enjoy being the center of attention.", "question1.jpg"),
        ("You prefer planning over spontaneity.", "question2.jpg"),
        ("You often rely on logic more than emotions.", "question3.jpg"),
        ("You enjoy working alone rather than in a team.", "question4.jpg"),
        ("You believe rules are meant to be broken.", "question5.jpg")
    };

        public MainPage()
        {
            InitializeComponent();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var q = questions[currentQuestionIndex];
                QuestionText.Text = q.Question;
                QuestionImage.Source = q.ImagePath;

                // Reset card position and rotation
                Card.TranslationX = 0;
                Card.Rotation = 0;
                xOffset = 0;
            }
            else
            {
                ShowResult();
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    xOffset = e.TotalX;
                    Card.TranslationX = xOffset;
                    break;

                case GestureStatus.Completed:
                    if (xOffset > 100)
                    {
                        OnAnswerSelected(true);  // Swiped right
                    }
                    else if (xOffset < -100)
                    {
                        OnAnswerSelected(false); // Swiped left
                    }
                    else
                    {
                        // Snap back if not far enough
                        Card.TranslateTo(0, 0, 250, Easing.SpringOut);
                    }
                    break;
            }
        }

        private void OnAnswerSelected(bool isTrue)
        {
            if (isTrue)
                score++;

            currentQuestionIndex++;
            LoadQuestion();
        }

        private void ShowResult()
        {
            Card.IsVisible = false;

            string result;
            string imagePath;

            if (score >= 4)
            {
                result = "You are The Leader: Bold, confident, and natural at taking charge.";
                imagePath = "leader.jpg";
            }
            else if (score >= 2)
            {
                result = "You are The Thinker: Logical, curious, and reflective.";
                imagePath = "thinker.jpg";
            }
            else
            {
                result = "You are The Dreamer: Creative, idealistic, and free-spirited.";
                imagePath = "dreamer.jpg";
            }

            ResultText.Text = result;
            ResultImage.Source = imagePath;

            ResultText.IsVisible = true;
            ResultImage.IsVisible = true;
            RestartButton.IsVisible = true;
        }

        private void OnRestartClicked(object sender, EventArgs e)
        {
            currentQuestionIndex = 0;
            score = 0;

            ResultText.IsVisible = false;
            ResultImage.IsVisible = false;
            RestartButton.IsVisible = false;
            Card.IsVisible = true;

            // Reset card transform
            Card.TranslationX = 0;
            Card.Rotation = 0;

            LoadQuestion();
        }
    }

}
