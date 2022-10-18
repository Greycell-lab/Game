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
using System.Media;

namespace Game
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool moveLeft, moveRight, moveUp, moveDown, alienMove, ashoot;
        int playerSpeed = 5;
        int alienSpeed = 7;
        int ufoLife = 100;
        int playerLife = 5;
        int alienshoottick = 0;

        SoundPlayer playeraudio = new SoundPlayer(Game.Properties.Resources.playershoot);
        SoundPlayer alienaudio = new SoundPlayer(Game.Properties.Resources.ufoshoot);
        MediaPlayer backgroundplayer = new MediaPlayer();       
        List<Rectangle> itemstoremove = new List<Rectangle>();
        public MainWindow()
        {
            DispatcherTimer gameTime = new DispatcherTimer();
            
            InitializeComponent();

            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Tick += gameEngine;
            gameTime.Start();            
            backgroundplayer.Open(new Uri("pack://application:,,,/Resources/background.mp3"));
            backgroundplayer.Play();
            MyCanvas.Focus();
            backgroundplayer.Open(new Uri(@"C:\Users\Administrator\Source\Repos\Game\Game\Resources\background.mp3"));
            backgroundplayer.Play();
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
        if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
         if(e.Key == Key.Right)
            {
                moveRight = true;
            }
         if(e.Key == Key.Up)
            {
                moveUp = true;
            }
         if(e.Key == Key.Down)
            {
                moveDown = true;
            }
         if(e.Key == Key.Space)
            {
                Rectangle bullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black
                };
                Canvas.SetTop(bullet, Canvas.GetTop(player) - bullet.Height);
                Canvas.SetLeft(bullet, Canvas.GetLeft(player) + player.Width / 2);
                MyCanvas.Children.Add(bullet);
                playeraudio.Play();
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
            if(e.Key == Key.Up)
            {
                moveUp = false;
            }
            if(e.Key == Key.Down)
            {
                moveDown = false;
            }
        }
        private void gameEngine(object sender, EventArgs e)
        {
            
            Task alienshootaudio = new Task(() =>
            alienaudio.Play()
            );
            alienshoottick++;
            ufolifelabel.Content = "Ufo Life: " + ufoLife;
            playerlifelabel.Content = "Player Life: " + playerLife;

            Rect ralien = new Rect(Canvas.GetLeft(alien), Canvas.GetTop(alien), alien.Width, alien.Height);
            Rect playerhitbox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            if(moveLeft && Canvas.GetLeft(player)>10)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }
            if(moveUp && Canvas.GetTop(player) > 125)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - playerSpeed);
            }
            if(moveDown && Canvas.GetTop(player) + 100 < Application.Current.MainWindow.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + playerSpeed);
            }
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if(x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    if(Canvas.GetTop(x) < 10)
                    {
                        itemstoremove.Add(x);
                    }
                    Rect rbullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if(rbullet.IntersectsWith(ralien))
                    {
                        ufoLife -= 1;
                        itemstoremove.Add(x);
                        if(ufoLife == 0)
                        {
                            MessageBox.Show("You WIN!");
                        }

                    }
                }
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
            Rectangle alienshoot = new Rectangle
            {
                Tag = "alienshoot",
                Height = 20,
                Width = 10,
                Fill = Brushes.LightGreen,
                Stroke = Brushes.Black
            };
            Canvas.SetTop(alienshoot, Canvas.GetTop(alien) + alienshoot.Height*2);
            Canvas.SetLeft(alienshoot, Canvas.GetLeft(alien) + alien.Width / 2);
            if (alienshoottick == 20)
            {
                MyCanvas.Children.Add(alienshoot);
                alienaudio.Play();
                alienshoottick = 0;
            }
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if(x is Rectangle && (string)x.Tag == "alienshoot" )
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);
                    if(Canvas.GetTop(x) > 580)
                    {
                        itemstoremove.Add(x);                        
                    }
                    Rect ahitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if(ahitbox.IntersectsWith(playerhitbox))
                    {
                        playerLife -= 1;                        
                        itemstoremove.Add(x);
                    }
                }
            }
            if (playerLife == 0)
            {
                MessageBox.Show("Game Over! You LOST");
                Environment.Exit(0);
            }
            if (ufoLife == 0)
            {
                MessageBox.Show("You WIN! You saved the Planet!");
                Environment.Exit(0);
            }

            foreach (var x in itemstoremove)
            {
                MyCanvas.Children.Remove(x);
            }

            
        }
    }
}
