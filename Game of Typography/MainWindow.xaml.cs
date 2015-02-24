using System;
using System.Collections.Generic;
using System.Linq;
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
using Xceed.Wpf.Toolkit;
using System.ComponentModel;
using System.Windows.Forms;
using TypographyEffects;


namespace Game_of_Typography
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            grdFilePaths.DataContext = fp;
            fp.VideoPath = @"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\sample.avi";
            fp.AudioPath = @"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\Enrique_Iglesias-_Rhythm_Divine.wav";
            fp.SrtPath = @"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\subtitle.srt";
            fp.BmpPath = @"D:\Game of Challenges\AudioToVideo\AudioToVideo\testdata\red.bmp";

            txtVideoStatus.Visibility = Visibility.Collapsed;


        }

        #region IsPlaying(bool)
        private void IsPlaying(bool bValue)
        {
            btnStop.IsEnabled = bValue;
            btnMoveBackward.IsEnabled = bValue;
            btnMoveForward.IsEnabled = bValue;
            btnPlay.IsEnabled = bValue;
        }
        #endregion

        #region Play and Pause
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            IsPlaying(true);
            if (btnPlay.Content.ToString() == "Play")
            {
                MediaEL.Source = new Uri(fp.VideoPath);
                MediaEL.Play();
                btnPlay.Content = "Pause";
            }
            else
            {
                MediaEL.Pause();
                btnPlay.Content = "Play";
            }
        }
        #endregion

        #region Stop
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MediaEL.Stop();
            btnPlay.Content = "Play";
            IsPlaying(false);
            btnPlay.IsEnabled = true;
        }
        #endregion

        #region Back and Forward
        private void btnMoveForward_Click(object sender, RoutedEventArgs e)
        {
            MediaEL.Position = MediaEL.Position + TimeSpan.FromSeconds(10);
        }

        private void btnMoveBackward_Click(object sender, RoutedEventArgs e)
        {
            MediaEL.Position = MediaEL.Position - TimeSpan.FromSeconds(10);
        }
        #endregion

        #region Open Media
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            //ofd.Filter = "Video Files (*.avi)";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MediaEL.Source = new Uri(ofd.FileName);
                btnPlay.IsEnabled = true;
            }
        }
        #endregion

        public AVI avi = new AVI();
        public FilePaths fp = new FilePaths();
        

        private void btnSrtFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fp.SrtPath = openFileDialog1.FileName;
            }
        }

        private void btnBmpFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fp.BmpPath = openFileDialog1.FileName;
            }
        }

        private void btnAudioFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fp.AudioPath = openFileDialog1.FileName;
            }
        }

        private void btnVideoFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fp.VideoPath = openFileDialog1.FileName;
            }
        }

        private void Wizard_PageChanged(object sender, RoutedEventArgs e)
        {
            Wizard p = e.Source as Wizard;

            if (p.CurrentPage.Name == "IntroPage")
            {
                p.NextButtonContent = "File Paths";
            }
            else if (p.CurrentPage.Name == "Page1")
            {
                p.NextButtonContent = "Text Effect";
                p.BackButtonContent = "Intro Page";
            }
            else if (p.CurrentPage.Name == "Page2")
            {
                txtVideoStatus.Visibility = Visibility.Collapsed;
                p.NextButtonContent = "Play Video";
                p.BackButtonContent = "Text Effect";
            }
        }

        private void btnCreateVideo_Click(object sender, RoutedEventArgs e)
        {
            txtVideoStatus.Visibility = Visibility.Visible;
            txtVideoStatus.Text = "Creating Video..........";

            TextEffect te = (TextEffect)cmbTextEffect.SelectedIndex;

            if (te == TextEffect.CurvedTextEffect)
            {
                avi.CreateVideo(fp, 30, curvedTextConfig, te);
                txtVideoStatus.Text = "Video Created..........";
                return;
            }
            else if (te == TextEffect.BouncingTextEffect)
            {
                avi.CreateVideo(fp, 30, bounceTextConfig, te);
                txtVideoStatus.Text = "Video Created..........";
                return;
            }

            avi.CreateVideo(fp, 30, bounceTextConfig, te);
            txtVideoStatus.Text = "Video Created..........";
        }

        private void cmbTextEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            Window window = new Window
            {
                Title = "Text effect",
                Content = new CurvedTextConfig()
            };
            window.Width = 400;
            window.Height = 400;


            TextEffect textEffect = (TextEffect)cmbTextEffect.SelectedIndex;

            if (textEffect == TextEffect.CurvedTextEffect)
            {
                window.Content = new CurvedTextConfig();
                window.ShowDialog();
            }
            else if (textEffect == TextEffect.BouncingTextEffect)
            {
                window.Content = new BounceTextConfig();
                window.ShowDialog();
            }

          

            if (textEffect == TextEffect.CurvedTextEffect)
            {
                CurvedTextConfig c = (CurvedTextConfig)window.Content;
                curvedTextConfig.FontSize = c.TextFontSize;
            }
            else if (textEffect == TextEffect.BouncingTextEffect)
            {
                BounceTextConfig c = (BounceTextConfig)window.Content;
                bounceTextConfig.FontSize = c.TextFontSize;
            }

           
            

        }
        TextEffectConfig config = new TextEffectConfig();
        CurvedTextEffectConfig curvedTextConfig = new CurvedTextEffectConfig();
        BounceTextEffectConfig bounceTextConfig = new BounceTextEffectConfig();
         

         private void btnConfig_Click(object sender, RoutedEventArgs e)
         {
             Window window = new Window
             {
                 Title = "Text effect",
                 Content = new CurvedTextConfig()
             };
             window.Width = 400;
             window.Height = 400;


             TextEffect textEffect = (TextEffect)cmbTextEffect.SelectedIndex;

             if (textEffect == TextEffect.CurvedTextEffect)
             {
                 window.Content = new CurvedTextConfig();
             }
             else if (textEffect == TextEffect.BouncingTextEffect)
             {
                 window.Content = new BounceTextConfig();
             }

             window.ShowDialog();

             if (textEffect == TextEffect.CurvedTextEffect)
             {
                 CurvedTextConfig c = (CurvedTextConfig)window.Content;
                 curvedTextConfig.FontSize = c.TextFontSize;
             }
             else if (textEffect == TextEffect.BouncingTextEffect)
             {
                 BounceTextConfig c = (BounceTextConfig)window.Content;
                 bounceTextConfig.FontSize = c.TextFontSize;
             }

         }
            
    }
}
