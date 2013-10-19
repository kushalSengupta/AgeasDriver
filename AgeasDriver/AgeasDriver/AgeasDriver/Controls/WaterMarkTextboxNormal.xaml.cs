using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TestSilverlightApp
{
    public partial class WaterMarkTextboxNormal : UserControl
    {
        private System.Windows.Input.InputScope myinputScope;
        public System.Windows.Input.InputScope MyInputScope
        {
            set
            {
                myinputScope = value;
            }
            get
            {
                return myinputScope;
            }
        }
        private string watermark;
        public string WaterMark
        {
            set
            {
                watermark = value;
            }

            get
            {
                return watermark;
            }
        }

        public string Text
        {
            get
            {
                if (textBox1.Text == this.watermark)
                {
                    return "";
                }
                else
                {
                    return textBox1.Text;
                }
            }
            set
            {
                DisableWaterMark();
                textBox1.Text = value;
            }
        }

        public Double FontHight
        {
            get
            {
                return textBox1.FontSize;
            }
            set
            {
                textBox1.FontSize = value;
            }
        }

        //public VerticalAlignment VerticalContentAlignment
        //{
        //    get 
        //    {
        //        return textBox1.VerticalContentAlignment;
        //    }
        //    set 
        //    {
        //        textBox1.VerticalContentAlignment = value;
        //    }
        //}

        public WaterMarkTextboxNormal()
        {
            InitializeComponent();
        }

        private void SetWaterMark()
        {
            textBox1.Foreground = new SolidColorBrush(Color.FromArgb(130,130,130,130));
            textBox1.Text = this.watermark;
        }

        void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                SetWaterMark();
                textBox1.GotFocus += new RoutedEventHandler(textBox1_GotFocus);
            }
        }

        private void DisableWaterMark()
        {
            textBox1.Text = "";
            textBox1.GotFocus -= textBox1_GotFocus;
            textBox1.Foreground = new SolidColorBrush(Colors.Black);
        }

        void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            DisableWaterMark();
        }

        private void textBox1_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.InputScope = myinputScope;

            if (!string.IsNullOrEmpty(this.watermark))
            {
                textBox1.GotFocus += new RoutedEventHandler(textBox1_GotFocus);
                textBox1.LostFocus += new RoutedEventHandler(textBox1_LostFocus);
                SetWaterMark();
            }
        }



    }
}
