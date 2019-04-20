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
using Examination;
using System.Windows.Threading;

namespace Quiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int incrememt = 1;
        DispatcherTimer dispatcher = new DispatcherTimer();
        public int score = 0;
        private double difference;
        private bool score_changed = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dtTicker(object sender, EventArgs e)
        {
            incrememt++;
            if(incrememt > 10)
            {
                incrememt = 10;
                dispatcher.Stop();
            }
            else
            {
                ChangeDrill drill = new ChangeDrill(tboxUser.Text);
                txtQuestion.Text = $"Question {incrememt}:\nHello {drill.customerName}! If a customer buys goods worth {drill.expense} and gives you {drill.income}, how much change needs to be given?";
                difference = drill.difference;

                score_changed = false;
            }
        }

        private void doDrill()
        {
            dispatcher.Interval = TimeSpan.FromSeconds(45);
            dispatcher.Tick += dtTicker;
            dispatcher.Start(); 
        }

        private void updateScores()
        {
            if (double.Parse(txtAnswer.Text) == difference && score_changed == false)
            {
                score++;
                lblScore.Content = $"{score} out of {incrememt} ({Math.Round((Convert.ToDouble(score) / incrememt * 100), 2)}%)";
                lblCorrectAnswer.Content = "";
                score_changed = true;

            }
            else if (score_changed == true)
            {
                lblCorrectAnswer.Content = $"Score has already been updated!";
            }
            else
            {
                lblScore.Content = $"{score} out of {incrememt} ({Math.Round((Convert.ToDouble(score) / incrememt * 100), 2)}%)";
                lblCorrectAnswer.Content = $"Incorrect.";// The correct answer is {difference}";
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            incrememt = 1;
            score = 0;

            tblockDirections.Text = "";
            lblScore.Content = "...";
            lblCorrectAnswer.Content = "";

            ChangeDrill drill = new ChangeDrill(tboxUser.Text);
            txtQuestion.Text = $"Question {incrememt}:\nHello {drill.customerName}! If a customer buys goods worth {drill.expense} and gives you {drill.income}, how much change needs to be given?";
            difference = drill.difference;

            score_changed = false;

            doDrill();

            txtAnswer.Focus();
            txtAnswer.SelectAll();

            btnStart.IsEnabled = false;
            btnEnd.IsEnabled = true;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            updateScores();

            incrememt++;
            if (incrememt > 10)
            {
                incrememt = 10;
                return;
            }
            else
            {

                ChangeDrill drill = new ChangeDrill(tboxUser.Text);
                txtQuestion.Text = $"Question {incrememt}:\nHello {drill.customerName}! If a customer buys goods worth {drill.expense} and gives you {drill.income}, how much change needs to be given?";
                difference = drill.difference;

                score_changed = false;

                doDrill();
            }
            txtAnswer.Focus();
            txtAnswer.SelectAll();
        }

        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            if (dispatcher.IsEnabled)
            {
                dispatcher.Stop();
            }

            incrememt = 1;
            score = 0;

            tblockDirections.Text = "";
            lblScore.Content = "...";
            lblCorrectAnswer.Content = "";

            btnStart.IsEnabled = true;
            btnEnd.IsEnabled = false;


        }
    }
}
