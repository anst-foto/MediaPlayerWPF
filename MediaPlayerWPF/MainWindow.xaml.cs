using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace MediaPlayerWPF;

public partial class MainWindow : Window
{
    
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Video Files (*.mp4)|*.mp4|MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"
        };
        
        if (dialog.ShowDialog() == true)
        {
            Player.Source = new Uri(dialog.FileName, UriKind.Absolute);
        }

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        
        Player.Play();
        Thread.Sleep(500);
        Player.Stop();
        
        Position.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
        
        timer.Tick += (_, _) =>
        {
            LabelStatus.Content = Player.Source is not null 
                ? $@"{Player.Position:hh\:mm\:ss} / {Player.NaturalDuration.TimeSpan:hh\:mm\:ss}" 
                : "Нет файла...";
        };
        timer.Start();
    }

    private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
    {
        Player.Play();
    }

    private void ButtonPause_OnClick(object sender, RoutedEventArgs e)
    {
        Player.Pause();
    }

    private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
    {
        Player.Stop();
    }

    private void Volume_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.Volume = (sender as Slider)!.Value;
    }

    private void Position_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Player.Pause();
        Player.Position = new TimeSpan(0, 0, 0, (int)(sender as Slider)!.Value);
        Player.Play();
    }
}