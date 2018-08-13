using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pozicija
{
    public partial class Poz : UserControl
    {
        int premuto = 0;
        int premutoOld = 0;

        ArrayList cb = new ArrayList(9);

        public int PREMUTO
        {
            get { return premuto; }
        }

        public delegate void PozicijaChangeHandler(object Poz, int Premuto);
        public event PozicijaChangeHandler PozicijaChange;

        protected void OnPozicijaChange(object Poz, int Premuto)
        {
            // Check if there are any Subscribers
            if (PozicijaChange != null)
            {
                // Call the Event
                PozicijaChange(Poz, premuto);
            }
        }

        public Poz()
        {
            InitializeComponent();

            cb.Add(checkBox9);
            cb.Add(checkBox1);
            cb.Add(checkBox2);
            cb.Add(checkBox3);
            cb.Add(checkBox4);
            cb.Add(checkBox5);
            cb.Add(checkBox6);
            cb.Add(checkBox7); 
            cb.Add(checkBox8);
            cb.Add(checkBox9);
        }

        private void checkBox1_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 1;
            Izvrsi();
        }

        private void checkBox2_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 2;
            Izvrsi();
        }

        private void checkBox3_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 3;
            Izvrsi();
        }

        private void checkBox4_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 4;
            Izvrsi();
        }

        private void checkBox5_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 5;
            Izvrsi();
        }

        private void checkBox6_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 6;
            Izvrsi();
        }

        private void checkBox7_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 7;
            Izvrsi();
        }

        private void checkBox8_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 8;
            Izvrsi();
        }

        private void checkBox9_MouseClick(object sender, EventArgs e)
        {
            premutoOld = premuto;
            premuto = 9;
            Izvrsi();
        }

        private void Izvrsi()
        {
            if (premuto != premutoOld)
            {
                ((CheckBox)cb[premutoOld]).Checked = false;
                //((CheckBox)cb[premutoOld]).CheckedState = System.Windows.Forms.CheckState.Unchecked;
            }

            ((CheckBox)cb[premuto]).Checked = true;
            //((CheckBox)cb[premuto]).CheckedState = System.Windows.Forms.CheckState.Checked;

            OnPozicijaChange(this, premuto);
        }
    }
}
