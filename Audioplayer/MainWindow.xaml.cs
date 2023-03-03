using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using Path = System.IO.Path;

namespace Audioplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<string> text = new List<string>();
        DispatcherTimer timer;
        static bool p;
        static bool l=true;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_tick;
            timer.Start();
        }

       
        private void timer_tick(object sender, EventArgs e)
        {
            Min.Text = media.Position.ToString(@"mm\:ss");
            Tic.Value = media.Position.TotalSeconds;  
        }
        private void Folder(object sender, RoutedEventArgs e)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker=true };
            
            var result = dialog.ShowDialog();
            
           // string ext = Path.GetExtension(dialog.FileName);
            if (result == CommonFileDialogResult.Ok)
            {
                var files = Directory.GetFiles(dialog.FileName);
                foreach (var item in files)
                {
                    if (Path.GetExtension(item) == ".mp3")
                    {
                        text.Add(item);
                    }
                }
                Treck.ItemsSource = text;
                media.Source = new Uri(text[0]);
                media.Play();
                
                
            }
        }
        private void media_MediaEnded(object sender, RoutedEventArgs e)//перемотка вправо
        {
            Per();
            timer.Start();
             if(p==true && Min.Text==Max.Text)
            {
                media.Source = new Uri(text[Treck.SelectedIndex]);
                media.Position = TimeSpan.FromSeconds(1);
                media.Play();
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//перемотка влево
        {
            back();
            timer.Start();
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//играть
        {
            //media.Source = new Uri((string)Treck.SelectedValue);
            media.Play();
            timer.Start();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            media.Pause();
            timer.Stop(); 
           
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            p = true;
        }

        private void RestartOff(object sender, RoutedEventArgs e)
        {
            p = false;
        }

        private void Shuffle(object sender, RoutedEventArgs e)
        {
           Random random = new Random();
           List<string> data = new List<string>();
            
            if (l == true)
            {
                foreach (var s in text)
                {
                    int j = random.Next(data.Count + 1);
                    if (j == data.Count)
                    {
                        data.Add(s);
                    }
                    else
                    {
                        data.Add(data[j]);
                        data[j] = s;
                    }
                }
                Treck.ItemsSource= data;
                media.Source = new Uri(data[random.Next(0,data.Count)]);
                media.Play();
                l = false;
            }
            else
            {
                Treck.ItemsSource = text;
                media.Source = new Uri(text[random.Next(0, text.Count)]);
                media.Play();
                l = true;
            }
        }

        private void Tic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)// слайдер
        {
            media.Pause();
            media.Position = TimeSpan.FromSeconds(Tic.Value);
            media.Play();

        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            media.Stop();
            timer.Stop();
            Tic.Value = 0;
             
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
           Tic.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
           Max.Text = media.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        }

        private void Load_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (media != null)
            {
                media.Volume = Load.Value;
            }
        }
        private void Per()
        {
            media.Stop();
            Treck.SelectedIndex += 1;
            int y = text.Count - 1;
            if (Treck.SelectedIndex == y && Min.Text ==Max.Text) { 
               Treck.SelectedIndex = 0; 
            }
        
            Me();
        }
        private void back()
        {
            media.Stop();
            Treck.SelectedIndex -= 1;
            if (Treck.SelectedIndex == -1) { Treck.SelectedIndex += text.Count; };
            Me();
        }
        private void Me()
        {
            media.Source = new Uri(text[Treck.SelectedIndex]);
            media.Play();
        }

       
    }
}
