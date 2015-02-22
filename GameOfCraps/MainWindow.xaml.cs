using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfCraps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand KeyboardCommand = new RoutedCommand();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mm_Game_Start_Click(object sender, RoutedEventArgs e)
        {
            btn_Roll.IsEnabled = true;
        }

        private void mm_Game_Exit_Click(object sender, RoutedEventArgs e)
        {
            ClosingGame();
        }

        private void ClosingGame()
        {
            var answer = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.OKCancel);

            if(answer == MessageBoxResult.OK)
                Application.Current.Shutdown();
        }

        private void mm_Help_About_Click(object sender, RoutedEventArgs e)
        {
            ShowAboutMessage();
        }

        private void ShowAboutMessage()
        {
            MessageBox.Show("Developer: Aaron Baumgarner" + Environment.NewLine + "Version: 0.0.01" + Environment.NewLine + ".NET 4.5.1" + Environment.NewLine + "64 bit", "About");
        }

        private void mm_Help_Shortcuts_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CTRL + Q - Exits Program" + Environment.NewLine + "CTRL + A - About", "Keybord Shortcuts");
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Q)
                ClosingGame();
        }

        private void mm_Help_Rules_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A player rolls two dice where each die has six faces in the usual way (values 1 through 6)." + Environment.NewLine + "After the dice have come to rest the sum of the two upward faces is calculated." + Environment.NewLine + "The first roll:" + Environment.NewLine + "   If the sum is 7 or 11 on the first throw the roller/player wins." + Environment.NewLine + "   If the sum is 2, 3 or 12 the roller/player loses, that is the house wins." + Environment.NewLine + "   If the sum is 4, 5, 6, 8, 9, or 10, that sum becomes the roller/player's \"point\"." + Environment.NewLine + "Continue given the player's point:" + Environment.NewLine + "   Now the player must roll the \"point\" total before rolling a 7 in order to win." + Environment.NewLine + "   If they roll a 7 before rolling the point value they got on the first roll the roller/player loses (the 'house' wins).", "Rules");
        }

        private void btn_Roll_Click(object sender, RoutedEventArgs e)
        {
            int dieOneRoll, dieTwoRoll, total;
            Random rand = new Random();

            dieOneRoll = rand.Next(1, 6);
            txtBox_DieOne.Text = dieOneRoll.ToString();

            dieTwoRoll = rand.Next(1, 6);
            txtBox_DieTwo.Text = dieTwoRoll.ToString();

            total = dieOneRoll + dieTwoRoll;

            txtBox_Total.Text = total.ToString();
        }

    }
}
