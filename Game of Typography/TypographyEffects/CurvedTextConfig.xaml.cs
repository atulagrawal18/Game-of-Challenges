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

namespace TypographyEffects
{
    /// <summary>
    /// Interaction logic for CurvedTextConfig.xaml
    /// </summary>
    public partial class CurvedTextConfig : UserControl
    {
        public CurvedTextConfig()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        public int TextFontSize { get {
            if (string.IsNullOrEmpty(txtFontSize.Text)) return 32;
            else return int.Parse(txtFontSize.Text);
        }
        }
        public int DistanceFromCentreToBaseOfText
        {
            get
            {
                if (string.IsNullOrEmpty(txtDistance.Text)) return 32;
            else return int.Parse(txtDistance.Text);} }

        
    }
}
