/*
 * Aaron Baumgarner
 * Notes: I added a regex to varify the user enters a non-negative and greater than zero number. It was surprisingly easy.
 *          
 */

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GameOfCraps
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand KeyboardCommand = new RoutedCommand();
        private int _betAmount;
        private int _bankAmount;
        private int _numHouseWins;
        private int _numPlayerWins;
        private int _numRoll;
        private int _point;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mm_Game_Start_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            btn_Roll.IsEnabled = true;
            btn_PlayAgain.IsEnabled = false;
            mm_Game_Start.IsEnabled = false;
            EnableBetAmounts();
        }

        private void mm_Game_Exit_Click(object sender, RoutedEventArgs e)
        {
            ClosingGame();
        }

        private void ClosingGame()
        {
            var answer = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.OKCancel);

            if (answer == MessageBoxResult.OK)
                Application.Current.Shutdown();
        }

        private void mm_Help_About_Click(object sender, RoutedEventArgs e)
        {
            ShowAboutMessage();
        }

        private void ShowAboutMessage()
        {
            MessageBox.Show(
                "Developer: Aaron Baumgarner" + Environment.NewLine + "Version: 0.0.01" + Environment.NewLine +
                ".NET 4.5.1" + Environment.NewLine + "64 bit", "About");
        }

        private void mm_Help_Shortcuts_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "CTRL + Q - Exits Program" + Environment.NewLine + "CTRL + A - About" + Environment.NewLine +
                "CTRL + S - Starts the game" + Environment.NewLine + "R - Rolls the die" + Environment.NewLine +
                "P - Play again", "Keybord Shortcuts");
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.Q))
                ClosingGame();
            else if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.S))
                StartGame();
            else if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.A))
                ShowAboutMessage();
        }

        private void mm_Help_Rules_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "A player rolls two dice where each die has six faces in the usual way (values 1 through 6)." +
                Environment.NewLine + "After the dice have come to rest the sum of the two upward faces is calculated." +
                Environment.NewLine + "The first roll:" + Environment.NewLine +
                "   If the sum is 7 or 11 on the first throw the roller/player wins." + Environment.NewLine +
                "   If the sum is 2, 3 or 12 the roller/player loses, that is the house wins." + Environment.NewLine +
                "   If the sum is 4, 5, 6, 8, 9, or 10, that sum becomes the roller/player's \"point\"." +
                Environment.NewLine + "Continue given the player's point:" + Environment.NewLine +
                "   Now the player must roll the \"point\" total before rolling a 7 in order to win." +
                Environment.NewLine +
                "   If they roll a 7 before rolling the point value they got on the first roll the roller/player loses (the 'house' wins).",
                "Rules");
        }

        private void btn_Roll_Click(object sender, RoutedEventArgs e)
        {
            DisableAllBetBtn();
            int dieOneRoll, dieTwoRoll, total;
            var rand = new Random();
            btn_Roll.IsEnabled = false;

            _numRoll++;
            dieOneRoll = rand.Next(1, 6);
            txtBox_DieOne.Text = dieOneRoll.ToString();

            dieTwoRoll = rand.Next(1, 6);
            txtBox_DieTwo.Text = dieTwoRoll.ToString();

            total = dieOneRoll + dieTwoRoll;

            btn_Roll.IsEnabled = true;

            if (_numRoll == 1)
                CheckFirstRollPoints(total);
            else
            {
                CheckContinueRolls(total);
            }

            txtBox_Point.Text = _point.ToString();

            txtBox_Total.Text = total.ToString();
            txtBox_PlayerWins.Text = _numPlayerWins.ToString();
            txtBox_HouseWins.Text = _numHouseWins.ToString();
        }

        private void CheckContinueRolls(int total)
        {
            if (total == 7)
            {
                HouseWins();
            }
            else if (total == _point)
            {
                PlayerWins();
            }
        }

        private void PlayerWins()
        {
            btn_Roll.IsEnabled = false;
            btn_PlayAgain.IsEnabled = true;
            mm_Game_Reset.IsEnabled = true;

            txtBox_WinLose.Text = "Winner!";
            txtBox_WinLose.Visibility = Visibility.Visible;
            _numPlayerWins++;

            txtBox_Point.Text = "";

            _bankAmount += (_betAmount*2);
            txtBox_BankAmount.Text = _bankAmount.ToString();
            _betAmount = 0;
        }

        private void HouseWins()
        {
            _numHouseWins++;
            txtBox_WinLose.Text = "Loser!";
            txtBox_WinLose.Visibility = Visibility.Visible;

            btn_Roll.IsEnabled = false;
            btn_PlayAgain.IsEnabled = true;
            mm_Game_Reset.IsEnabled = true;

            txtBox_Point.Text = "";

            if (_bankAmount == 0)
            {
                MessageBox.Show("You are out of money. The game will now reset.");
                ResetGame();
            }
        }

        private void CheckFirstRollPoints(int total)
        {
            if (total == 11 || total == 7)
            {
                PlayerWins();
            }
            else if (total == 2 || total == 3 || total == 12)
            {
                HouseWins();
            }
            else if (total == 4 || total == 5 || total == 6 || total == 8 || total == 9 || total == 10)
            {
                _point = total;
            }
        }

        private void btn_PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            txtBox_Total.Text = "";
            txtBox_Point.Text = "";
            txtBox_DieOne.Text = "";
            txtBox_DieTwo.Text = "";
            txtBox_BetAmount.Text = "";
            _point = 0;
            _numRoll = 0;

            txtBox_WinLose.Visibility = Visibility.Hidden;
            StartGame();
        }

        private void mm_Game_Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            txtBox_Total.Text = "";
            txtBox_Point.Text = "";
            txtBox_DieOne.Text = "";
            txtBox_DieTwo.Text = "";
            txtBox_BankAmount.Text = "";
            txtBox_BetAmount.Text = 0.ToString();
            
            btn_BankSet.IsEnabled = true;
            btn_PlayAgain.IsEnabled = false;
            txtBox_BankAmount.IsReadOnly = false;
            txtBox_WinLose.Visibility = Visibility.Hidden;

            _numPlayerWins = 0;
            _numHouseWins = 0;
            _point = 0;
            _numRoll = 0;
            _betAmount = 0;

            txtBox_BankAmount.Focus();
        }

        private void btn_Bet_Five_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(5);
        }

        private void AddToBet(int i)
        {
            _betAmount += i;
            _bankAmount -= i;
            txtBox_BetAmount.Text = _betAmount.ToString();
            txtBox_BankAmount.Text = _bankAmount.ToString();

            EnableBetAmounts();
            mm_Game_Start.IsEnabled = true;
        }

        private void btn_Bet_One_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(1);
        }

        private void btn_Bet_Ten_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(10);
        }

        private void btn_Bet_Fifty_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(50);
        }

        private void btn_Bet_Hundred_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(100);
        }

        private void btn_Bet_FiveHundred_Click(object sender, RoutedEventArgs e)
        {
            AddToBet(500);
        }

        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please set your bank amount before playing.");
            txtBox_BankAmount.Focus();
        }

        private void btn_BankSet_Click(object sender, RoutedEventArgs e)
        {
            BankControl();
        }

        private void BankControl()
        {
            string pattern = @"^\-{0}[1-9]{1}[0-9]*";
            string input;
            Match matches;
            
            txtBox_BankAmount.Focus();

            input = txtBox_BankAmount.Text;
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            matches = rgx.Match(input);


            if (!matches.Success)
            {
                MessageBox.Show("Invalid input");
                txtBox_BankAmount.Text = String.Empty;
                txtBox_BankAmount.Focus();
            }
            else
            {
                _bankAmount = Convert.ToInt32(txtBox_BankAmount.Text);
                txtBox_BankAmount.IsReadOnly = true;
                btn_BankSet.IsEnabled = false;
                EnableBetAmounts();
            }
        }

        private void EnableBetAmounts()
        {
            if(_bankAmount >= 1)
                btn_Bet_One.IsEnabled = true;
            else
                btn_Bet_One.IsEnabled = false;
            
            if(_bankAmount >= 5)
                btn_Bet_Five.IsEnabled = true;
            else
                btn_Bet_Five.IsEnabled = false;

            if(_bankAmount >= 10)
                btn_Bet_Ten.IsEnabled = true;
            else
                btn_Bet_Ten.IsEnabled = false;

            if(_bankAmount >= 50)
                btn_Bet_Fifty.IsEnabled = true;
            else
                btn_Bet_Fifty.IsEnabled = false;

            if(_bankAmount >= 100)
                btn_Bet_Hundred.IsEnabled = true;
            else
                btn_Bet_Hundred.IsEnabled = false;

            if(_bankAmount >= 500)
                btn_Bet_FiveHundred.IsEnabled = true;
            else
                btn_Bet_FiveHundred.IsEnabled = false;

            }

        private void DisableAllBetBtn()
        {
            btn_Bet_One.IsEnabled = false;
            btn_Bet_Five.IsEnabled = false;
            btn_Bet_Ten.IsEnabled = false;
            btn_Bet_Fifty.IsEnabled = false;
            btn_Bet_Hundred.IsEnabled = false;
            btn_Bet_FiveHundred.IsEnabled = false;
        }

    }
}