using System.Security.Cryptography;
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

namespace _2048_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
            SetColors();
        }

        public void SetColors()
        {

            
            List<string> ColorHexCodes = new List<string>
            {
            "#f6e8dd", //2
            "#f9e5cd", //4
            "#feb274", //8
            "#fe975c", //16
            "#fe7e5b", //32
            "#fe5d31", //64
            "#fecd64", //128
            "#fecd64", //256
            "#fec830", //512
            "#df9a01", //1024
            "#df9a01", //2048
            "#fe5d31", //4096
            "#fe5d31"  //8192
            };

            List<string> Numbers = new List<string>
            {
                "2",
                "4",
                "8",
                "16",
                "32",
                "64",
                "128",
                "256",
                "512",
                "1024",
                "2048",
                "4096",
                "8192"
            };

            //This is for Color:
            {
                TextBlock[,] Tiles;
                Tiles = new TextBlock[,]
    {
                { Tile1, Tile2, Tile3, Tile4 },
                { Tile5, Tile6, Tile7, Tile8 },
                { Tile9, Tile10, Tile11, Tile12 },
                { Tile13, Tile14, Tile15, Tile16 }
    };
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int n = 0;
                        if (Tiles[i,j].Text == "")
                        {
                            Tiles[i,j].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d8c1b3"));
                        }
                        else
                        {
                            
                            while (!(Tiles[i,j].Text == Numbers[n]))
                            {
                                n++;
                            }
                            Tiles[i, j].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorHexCodes[n]));
                        }
                        Tiles[i,j].FontSize = 45 - Numbers[n].Length * 5;
                    }
                }

                
            }


        }

        public void Start()
        {
            Tile1.Text = "";
            Tile2.Text = "";
            Tile3.Text = "";
            Tile4.Text = "";
            Tile5.Text = "";
            Tile6.Text = "";
            Tile7.Text = "";
            Tile8.Text = "";
            Tile9.Text = "";
            Tile10.Text = "";
            Tile11.Text = "";
            Tile12.Text = "";
            Tile13.Text = "";
            Tile14.Text = "";
            Tile15.Text = "";
            Tile16.Text = "";
            PickRandom("start");
            PickRandom("start");
            GameOver.Text = ""; 
            LastMove.Text = "Nothing";
            SetColors();
            moveHistory.Clear();
            MoveHistoryList.Items.Clear();
        }
        static int Score = 0;
        static int Highscore = Score;
        static bool boardChanged = false;
        public void PickRandom(string startorplay)
        {
            TextBlock[,] Tiles;
            Tiles = new TextBlock[,]
            {
            { Tile1, Tile2, Tile3, Tile4 },
            { Tile5, Tile6, Tile7, Tile8 },
            { Tile9, Tile10, Tile11, Tile12 },
            { Tile13, Tile14, Tile15, Tile16 }
            };

            List<(int, int)> emptyTiles = new List<(int, int)>();

            // Find all empty tiles
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Tiles[i, j].Text == "")
                    {
                        emptyTiles.Add((i, j));
                    }
                }
            }
            if (startorplay == "play")
            {
                if (boardChanged == true)
                {
                    if (emptyTiles.Count > 0)
                    {
                        Random rnd = new Random();
                        var (row, col) = emptyTiles[rnd.Next(0, emptyTiles.Count)];

                        int twoorfour = rnd.Next(1, 10);
                        if (twoorfour == 10)
                        {
                            Tiles[row, col].Text = "4";
                            Score = Score + 4;
                            YourScore.Text = Score.ToString();
                        }
                        else
                        {
                            Tiles[row, col].Text = "2";
                            Score = Score + 2;
                            YourScore.Text = Score.ToString();
                        }
                    }
                }
            }
            else if(startorplay == "start")
            {
                if (emptyTiles.Count > 0)
                {
                    Random rnd = new Random();
                    var (row, col) = emptyTiles[rnd.Next(0, emptyTiles.Count)];

                    int twoorfour = rnd.Next(1, 10);
                    if (twoorfour == 10)
                    {
                        Tiles[row, col].Text = "4";
                    }
                    else
                    {
                        Tiles[row, col].Text = "2";
                    }
                }
            }
            if (Score > Highscore)
            {
                Highscore = Score;
                HighScore.Text = Highscore.ToString();
            }
        }


        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            YourScore.Text = "0";
            Score = 0;
            Start();
        }

        private void Move_KeyDown(object sender, KeyEventArgs e)
        {
            boardChanged = false;
            if (e.Key == Key.Up)
            {
                LastMove.Text = "Up";
                Compress("up");
                Merge("up");
                Compress("up");
                RecordMove("Up");
            }
            else if (e.Key == Key.Down)
            {
                LastMove.Text = "Down";
                Compress("down");
                Merge("down");
                Compress("down");
                RecordMove("Down");
            }
            else if (e.Key == Key.Left)
            {
                LastMove.Text = "Left";
                Compress("left");
                Merge("left");
                Compress("left");
                RecordMove("Left");
            }
            else if (e.Key == Key.Right)
            {
                LastMove.Text = "Right";
                Compress("right");
                Merge("right");
                Compress("right");
                RecordMove("Right");
            }
            PickRandom("play");
            SetColors();
            IsGameOver();
            
        }
        

        public void IsGameOver()
        {
            if (boardChanged == false)
            {
                if (!(Tile1.Text == "") && !(Tile2.Text == "") && !(Tile3.Text == "") && !(Tile4.Text == ""))
                {
                    if (!(Tile5.Text == "") && !(Tile6.Text == "") && !(Tile7.Text == "") && !(Tile8.Text == ""))
                    {
                        if (!(Tile9.Text == "") && !(Tile10.Text == "") && !(Tile11.Text == "") && !(Tile12.Text == ""))
                        {
                            if (!(Tile13.Text == "") && !(Tile14.Text == "") && !(Tile15.Text == "") && !(Tile16.Text == ""))
                            {
                                GameOver.Text = "Game Likely Over";
                            }
                            else
                            {
                                GameOver.Text = "";
                            }
                        }
                        else
                        {
                            GameOver.Text = "";
                        }
                    }
                    else
                    {
                        GameOver.Text = "";
                    }
                }
                else
                {
                    GameOver.Text = "";
                }
            }
            else
            {
                GameOver.Text = "";
            }
        }

        public void Compress(string direction)
        {
            TextBlock[,] Tiles;
            Tiles = new TextBlock[,]
            {
                { Tile1, Tile2, Tile3, Tile4 },
                { Tile5, Tile6, Tile7, Tile8 },
                { Tile9, Tile10, Tile11, Tile12 },
                { Tile13, Tile14, Tile15, Tile16 }
            };
            if (direction == "right")
            {
                for (int h = 0; h < 5; h++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (Tiles[i, j + 1].Text == "")
                                {
                                    Tiles[i, j + 1].Text = Tiles[i, j].Text;
                                    Tiles[i, j].Text = "";
                                    boardChanged = true;
                                }
                        }
                    }
                }
            }
            else if (direction == "left")
            {
                for (int h = 0; h < 5; h++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 3; j > 0; j--)
                        {
                            if (Tiles[i, j - 1].Text == "")
                            {
                                Tiles[i, j - 1].Text = Tiles[i, j].Text;
                                Tiles[i, j].Text = "";
                                boardChanged = true;
                            }
                        }
                    }
                }
            }
            else if (direction == "up")
            {
                for (int h = 0; h < 5; h++)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (Tiles[i-1, j].Text == "")
                            {
                                Tiles[i-1, j].Text = Tiles[i, j].Text;
                                Tiles[i, j].Text = "";
                                boardChanged = true;
                            }
                        }
                    }
                }
            }
            else if (direction == "down")
            {
                for (int h = 0; h < 5; h++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (Tiles[i + 1, j].Text == "")
                            {
                                Tiles[i + 1, j].Text = Tiles[i, j].Text;
                                Tiles[i, j].Text = "";
                                boardChanged = true;
                            }
                        }
                    }
                }
            }
        }

        public void Merge(string direction)
        {
            List<string> Numbers = new List<string>
            {
                "2",
                "4",
                "8",
                "16",
                "32",
                "64",
                "128",
                "256",
                "512",
                "1024",
                "2048",
                "4096",
                "8192"
            };
            TextBlock[,] Tiles;
            Tiles = new TextBlock[,]
            {
                { Tile1, Tile2, Tile3, Tile4 },
                { Tile5, Tile6, Tile7, Tile8 },
                { Tile9, Tile10, Tile11, Tile12 },
                { Tile13, Tile14, Tile15, Tile16 }
            };
            if (direction == "right")
            {
                //for (int i = 0; i < 4; i++)
                //{
                //    for (int j = 0; j < 3; j++)
                //    {
                //        if ((Tiles[i, j + 1].Text == Tiles[i, j].Text) && !(Tiles[i,j].Text==""))
                //        {
                //            Tiles[i, j + 1].Text = Numbers[Numbers.FindIndex(x => x == Tiles[i,j+1].Text)+1]; 
                //            Tiles[i, j].Text = "";
                //            boardchanged = true;
                //        }
                //    }
                //}
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 2; j > -1; j--)
                    {
                        if ((Tiles[i, j + 1].Text == Tiles[i, j].Text) && !(Tiles[i, j].Text == ""))
                        {
                            Tiles[i, j + 1].Text = Numbers[Numbers.FindIndex(x => x == Tiles[i, j + 1].Text) + 1]; ;
                            Tiles[i, j].Text = "";
                            boardChanged = true;
                        }
                    }
                }
            }
            else if (direction == "left")
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 3; j > 0; j--)
                    {
                        if ((Tiles[i, j - 1].Text == Tiles[i, j].Text) && !(Tiles[i, j].Text == ""))
                        {
                            Tiles[i, j - 1].Text = Numbers[Numbers.FindIndex(x => x == Tiles[i, j - 1].Text) + 1]; ;
                            Tiles[i, j].Text = "";
                            boardChanged = true;
                        }
                    }
                }
            }
            else if (direction == "up")
            {
                for (int i = 1; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if ((Tiles[i-1, j].Text == Tiles[i, j].Text) && !(Tiles[i, j].Text == ""))
                        {
                            Tiles[i-1, j].Text = Numbers[Numbers.FindIndex(x => x == Tiles[i-1, j].Text) + 1]; ;
                            Tiles[i, j].Text = "";
                            boardChanged = true;
                        }
                    }
                }
            }
            else if (direction == "down")
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if ((Tiles[i + 1, j].Text == Tiles[i, j].Text) && !(Tiles[i, j].Text == ""))
                        {
                            Tiles[i + 1, j].Text = Numbers[Numbers.FindIndex(x => x == Tiles[i + 1, j].Text) + 1]; ;
                            Tiles[i, j].Text = "";
                            boardChanged = true;
                        }
                    }
                }
            }
        }
        
        static bool togglehistory = false;
        private void ShowHideHistory_Click(object sender, RoutedEventArgs e)
        {
            if (togglehistory)
            {
                togglehistory = false;
                ShowHideHistory.Content = "Show History?";
                MoveHistoryList.Items.Clear();
                MoveHistoryList.BorderThickness = new Thickness(0);
            }
            else
            {
                togglehistory = true;
                ShowHideHistory.Content = "Hide History?";
                MoveHistoryList.BorderThickness = new Thickness(3);
                foreach (string move in moveHistory)
                {
                    MoveHistoryList.Items.Add(move);
                }
            }
        }
        
        private List<string> moveHistory = new List<string>();
        public void RecordMove(string move)
        {
            moveHistory.Add(move);
            if (togglehistory == true)
            {
                for (int i = 0; i < moveHistory.Count; i++)
                {
                    MoveHistoryList.Items.Add(moveHistory[i]);
                }
            }
            else
            {
                MoveHistoryList.Items.Clear();
            }
        }
    }
}