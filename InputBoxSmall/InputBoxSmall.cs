using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InputBoxSmall
{
    public partial class InputBoxSmall : UserControl
    {
        string oldText = "";
        double dMin = -9999999.0;
        double dMax = 9999999.0;
        bool dInt = false;
        bool change = true;
        bool gchange = true;

        double value_old = 0.0;

        public double VALUE
        {
            get
            {
                string s = textBox1.Text.Replace('.', ',');

                double d = double.Parse(s);

                return d;
            }

            set
            {
                string s = SrediStr(value.ToString().Replace('.', ','));

                change = true;

                textBox1.Text = s;
            }
        }

        public double OLD_VALUE
        {
            get
            {
                return value_old;
            }
        }

        public string TEXT
        {
            get
            {
                return textBox1.Text;
            }
            set 
            {
                string s = value;

                change = true;

                textBox1.Text = s.Replace('.', ',');
            }
        }

        public double VMIN
        { 
            set
            {
                dMin = value;
            }
        }

        public double VMAX
        {
            set
            {
                dMax = value;
            }
        }

        public bool VINT
        {
            set
            {
                dInt = value;
            }
        }

        public bool CHANGE
        {
            set
            {
                change = value;

                gchange = change;
            }
        }

        public delegate void inputBoxChangeHandler(object InputBox, double VALUE);
        public event inputBoxChangeHandler inputBoxChange;

        protected void OnPozicijaChange(object InputBox, double VALUE)
        {
            // Check if there are any Subscribers
            if (inputBoxChange != null)
            {
                // Call the Event
                inputBoxChange(InputBox, VALUE);
            }
        }

        public InputBoxSmall()
        {
            InitializeComponent();

            oldText = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (change == false || textBox1.Text == "") return;

            try
            {
                string s = textBox1.Text.Replace('.', ',');

                double d = double.Parse(s);

                value_old = d;

                d++;

                string sd = d.ToString();

                if (sd.IndexOf(',') == -1) sd = sd + ",00";

                textBox1.Text = SrediStr(sd);
            }
            catch (Exception ex01)
            {
            }

            try
            {
                string s = textBox1.Text;
                double d = double.Parse(s);

                OnPozicijaChange(this, d);
            }
            catch (Exception ex01)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (change == false || textBox1.Text == "") return;

            try
            {
                string s = textBox1.Text.Replace('.', ',');

                double d = double.Parse(s);

                value_old = d;

                d--;

                if (d < dMin) return;

                string sd = d.ToString();

                if (sd.IndexOf(',') == -1) sd = sd + ",00";

                textBox1.Text = SrediStr(sd);
            }
            catch (Exception ex01)
            {
            }

            try
            {
                string s = textBox1.Text;
                double d = double.Parse(s);

                OnPozicijaChange(this, d);
            }
            catch (Exception ex01)
            {
            }
        }

        private bool InputUToku = false;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool ok = true;

            if (textBox1.Text == "") return;
            if (textBox1.Text == "-") return;

            if (change == false)
            {
                change = true;
                textBox1.Text = SrediStr(oldText);
            }

            string s = textBox1.Text;

            try
            {
                double d = double.Parse(s);

                oldText = s;

                //OnPozicijaChange(this, d);
            }
            catch (Exception ex01)
            {
                ok = false;

                textBox1.Text = SrediStr(oldText);
            }

            change = gchange;
        }

        private void InputBox_Leave(object sender, EventArgs e)
        {

        }

        private string SrediStr(string s)
        {
            if (s.Length == 0) return "";

            if (dInt == true)
            {
                s = s.Substring(0, s.IndexOf(','));
            }
          
            if (s.IndexOf(',') == -1) s = s + ",00";

            string nule = "00";

            s = s + nule;

            s = s.Substring(0, s.LastIndexOf(',') + 1 + nule.Length);

            return s;
        }

        private void InputBox_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (InputUToku == false) return;

            InputUToku = false;

            if (textBox1.Text == "")
                textBox1.Text = SrediStr("0,00");
            else
                textBox1.Text = SrediStr(textBox1.Text);

            try
            {
                string s = textBox1.Text;
                double d = double.Parse(s);

                OnPozicijaChange(this, d);
            }
            catch (Exception ex01)
            {
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            InputUToku = true;

            try
            {
                string s = textBox1.Text;
                double d = double.Parse(s);

                value_old = d;
            }
            catch (Exception ex01)
            {
            }
        }
    }
}
