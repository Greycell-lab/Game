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
using System.Windows.Threading;

namespace Game
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool moveLeft, moveRight, alienMove;
        int playerSpeed = 10;
        int alienSpeed = 10;
        public MainWindow()
        {
            DispatcherTimer gameTime = new DispatcherTimer();

            InitializeComponent();

            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Tick += gameEngine;
            gameTime.Start();
            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space.png"));
            MyCanvas.Background = bg;
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/spaceship.png"));
            player.Fill = playerImage;
            ImageBrush alienImage = new ImageBrush();
            alienImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/alienship.png"));
            alien.Fill = alienImage;


        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
         if(e.Key == Key.Left)
            {
                moveLeft = true;
            }
         if(e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
        }
        private void gameEngine(object sender, EventArgs e)
        {
            if(moveLeft && Canvas.GetLeft(player)>10)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }            
            if(alienMove == true)
            {
                Canvas.SetLeft(alien, Canvas.GetLeft(alien) + alienSpeed);                
                if(Canvas.GetLeft(alien) > 320)
                {
                    alienMove = false;
                }
            }
            if(alienMove == false)
            {
                Canvas.SetLeft(alien, Canvas.GetLeft(alien) - alienSpeed);
                if(Canvas.GetLeft(alien) < 10)
                {
                    alienMove = true;
                }
            }
        }
    }
}
