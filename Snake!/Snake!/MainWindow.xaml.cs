using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Snake_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Snake : Window
    {
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer();
        
        const int SquareSize = 20;
        const int StartLength = 5;
        const int StartSpeed = 400;
        const int SpeedThreshold = 100;

        private Random rnd = new Random();

        private SolidColorBrush snakeBody = Brushes.MidnightBlue;
        private SolidColorBrush snakeHead = Brushes.DarkViolet;
        private List<SnakePart> snakeParts = new List<SnakePart>();

        private UIElement snakefood = null;
        private SolidColorBrush foodBrush = Brushes.LimeGreen;

        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection snakeDirection = SnakeDirection.Right;
        private int snakeLength = 0;
        private int currentScore = 0;

        public ObservableCollection<SnakeHighScore> HightScoreList { get; set; } = new ObservableCollection<SnakeHighScore>();
        public int MaxHighScoreListEntryCount = 10;

        public Snake()
        {
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            LoadHighScoreList();
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            SnakeDirection originalDirection = snakeDirection;
            switch(e.Key)
            {
                case Key.Up:
                    if (snakeDirection != SnakeDirection.Down)
                        snakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (snakeDirection != SnakeDirection.Up)
                        snakeDirection = SnakeDirection.Down;
                    break;
                case Key.Left:
                    if (snakeDirection != SnakeDirection.Right)
                        snakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (snakeDirection != SnakeDirection.Left)
                        snakeDirection = SnakeDirection.Right;
                    break;
                case Key.Space:
                    StartNewGame();
                    break;
            }
            if (snakeDirection != originalDirection)
                MoveSnake();
        }

        private void StartNewGame()
        {
            bdrWelcomeMessage.Visibility = Visibility.Collapsed;
            bdrHighScoreList.Visibility = Visibility.Collapsed;
            bdrEndOfGame.Visibility = Visibility.Collapsed;

            //remove parts & food
            foreach(SnakePart snakeBodyPart in snakeParts)
            {
                if (snakeBodyPart.UIElement != null)
                    GameArea.Children.Remove(snakeBodyPart.UIElement);
            }
            snakeParts.Clear();
            if (snakefood != null)
                GameArea.Children.Remove(snakefood);

            //reset
            currentScore = 0;
            snakeLength = StartLength;
            snakeDirection = SnakeDirection.Right;
            snakeParts.Add(new SnakePart() { Position = new Point(SquareSize * 5, SquareSize * 5) });
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(StartSpeed);

            DrawSnake();
            DrawSnakeFood();

            UpdateGameStatus();

            gameTickTimer.IsEnabled = true;
        }

        private void DoCollisionCheck()
        {
            SnakePart head = snakeParts[snakeParts.Count - 1];

            if ((head.Position.X == Canvas.GetLeft(snakefood)) && (head.Position.Y == Canvas.GetTop(snakefood)))
            {
                EatSnakeFood();
                return;
            }

            if ((head.Position.Y < 0) || (head.Position.Y >= GameArea.ActualHeight) || (head.Position.X < 0) || (head.Position.X >= GameArea.ActualWidth))
            {
                EndGame();
            }

            foreach(SnakePart snakeBodyPart in snakeParts.Take(snakeParts.Count - 1))
            {
                if ((head.Position.X == snakeBodyPart.Position.X) && (head.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }

        private void DrawGameArea()
        {
            bool doneDrawing = false;
            int nextX = 0, nextY = 0;
            int rows = 0;
            bool oddNext = false;

            while (doneDrawing == false)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SquareSize,
                    Height = SquareSize,
                    Fill = oddNext ? Brushes.PeachPuff : Brushes.Tomato
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                oddNext = !oddNext;
                nextX += SquareSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SquareSize;
                    rows++;
                    oddNext = (rows % 2 != 0);
                }
                if (nextY >= GameArea.ActualHeight)
                    doneDrawing = true;
                
            }
        }

        private void DrawSnake()
        {
            foreach(SnakePart snakePart in snakeParts)
            {
                if(snakePart.UIElement == null)
                {
                    snakePart.UIElement = new Rectangle()
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                        Fill = (snakePart.IsHead ? snakeHead : snakeBody)
                    };
                    GameArea.Children.Add(snakePart.UIElement);
                    Canvas.SetTop(snakePart.UIElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UIElement, snakePart.Position.X);
                }
            }
        }

        private void MoveSnake()
        {
            //remove last part
            while(snakeParts.Count >= snakeLength)
            {
                GameArea.Children.Remove(snakeParts[0].UIElement);
                snakeParts.RemoveAt(0);
            }

            //mark non-head
            foreach(SnakePart snakePart in snakeParts)
            {
                (snakePart.UIElement as Rectangle).Fill = snakeBody;
                snakePart.IsHead = false;
            }

            //chose direction
            SnakePart snakehead = snakeParts[snakeParts.Count - 1];
            double nextX = snakehead.Position.X;
            double nextY = snakehead.Position.Y;
            switch(snakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= SquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += SquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= SquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += SquareSize;
                    break;
            }
            //add part
            snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY),
                IsHead = true
            });

            DrawSnake();

            DoCollisionCheck();
        }

        private Point GetNextFoodPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / SquareSize);
            int maxY = (int)(GameArea.ActualHeight / SquareSize);
            int foodX = rnd.Next(0, maxX) * SquareSize;
            int foodY = rnd.Next(0, maxY) * SquareSize;

            foreach(SnakePart snakePart in snakeParts)
            {
                if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
                    return GetNextFoodPosition();
            }
            return new Point(foodX, foodY);
        }

        private void DrawSnakeFood()
        {
            Point foodPos = GetNextFoodPosition();
            snakefood = new Ellipse()
            {
                Width = SquareSize,
                Height = SquareSize,
                Fill = foodBrush
            };
            GameArea.Children.Add(snakefood);
            Canvas.SetTop(snakefood, foodPos.Y);
            Canvas.SetLeft(snakefood, foodPos.X);
        }

        private void EatSnakeFood()
        {
            snakeLength++;
            currentScore++;
            int timerInterval = Math.Max(SpeedThreshold, (int)gameTickTimer.Interval.TotalMilliseconds - (currentScore * 2));
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            GameArea.Children.Remove(snakefood);
            DrawSnakeFood();
            UpdateGameStatus();
        }

        private void UpdateGameStatus()
        {
            this.tbStatusScore.Text = currentScore.ToString();
            this.tbStatusSpeed.Text = gameTickTimer.Interval.TotalMilliseconds.ToString();
        }

        private void EndGame()
        {
            bool isNewHighscore = false;
            if(currentScore > 0)
            {
                int lowestHighscore = (this.HightScoreList.Count > 0 ? this.HightScoreList.Min(x => x.Score) : 0);
                if((currentScore > lowestHighscore) || (this.HightScoreList.Count > MaxHighScoreListEntryCount))
                {
                    bdrNewHighScore.Visibility = Visibility.Visible;
                    txtPlayerName.Focus();
                    isNewHighscore = true;
                }
            }
            if(!isNewHighscore)
            {
                tbFinalScore.Text = currentScore.ToString();
                bdrEndOfGame.Visibility = Visibility.Visible;
            }

            gameTickTimer.IsEnabled = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnShowHighScoreList_Click(object sender, RoutedEventArgs e)
        {
            bdrWelcomeMessage.Visibility = Visibility.Collapsed;
            bdrHighScoreList.Visibility = Visibility.Visible;
        }

        private void bthAddToHighscoreList_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;


            if ((this.HightScoreList.Count > 0) && (currentScore < this.HightScoreList.Max(x => x.Score)))
            {
                SnakeHighScore justAbove = this.HightScoreList.OrderByDescending(x => x.Score).First(x => x.Score >= currentScore);
                if (justAbove != null)
                    newIndex = this.HightScoreList.IndexOf(justAbove) + 1;
            }

            this.HightScoreList.Insert(newIndex, new SnakeHighScore()
            {
                PlayerName = txtPlayerName.Text,
                Score = currentScore
            });

            while (this.HightScoreList.Count > MaxHighScoreListEntryCount)
                this.HightScoreList.RemoveAt(MaxHighScoreListEntryCount);

            SaveHighScoreList();

            bdrNewHighScore.Visibility = Visibility.Collapsed;
            bdrHighScoreList.Visibility = Visibility.Visible;

        }

        private void LoadHighScoreList()
        {
            if(File.Exists("snake_highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SnakeHighScore>));
                using(Stream reader = new FileStream("snake_highscorelist.xml", FileMode.Open))
                {
                    List<SnakeHighScore> templist = (List<SnakeHighScore>)serializer.Deserialize(reader);
                    this.HightScoreList.Clear();
                    foreach (var item in templist.OrderByDescending(x => x.Score))
                        this.HightScoreList.Add(item);
                }
            }
        }

        private void SaveHighScoreList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<SnakeHighScore>));
            using(Stream writer = new FileStream("snake_highscorelist.xml", FileMode.Create))
            {
                serializer.Serialize(writer, this.HightScoreList);
            }
        }
    }
}