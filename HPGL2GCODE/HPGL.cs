using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace AsseTest
{
	public class HPGL
	{
		private string ProgramHPGL = "";
		private string ProgramMoveTo = "";

		string line;
		int lineCount;

        bool FINE = false;

		private struct Punto
		{
			public Punto(float x, float y)
			{
				X = x;
				Y = y;
			}
			public float X;
			public float Y;
		}

		private Punto punto = new Punto(0f, 0f);

		private ArrayList Lines = new ArrayList();

		private string[] pars = new string[2];
		int[] VS = new int[256];
		float[] PW = new float[256];
		int SP = 0;

		bool draw = false;

		string[] lineDiv = new string[65535];
		const float ptomm = 0.0254005F;

		float XMax = -999999999f;
		float YMax = -999999999f;
		float XMin = 999999999f;
		float YMin = 999999999f;
		float offsetX = 0f;
		float offsetY = 0f;

		public float XMAX
		{
			get{return XMax;}
		}
		public float YMAX
		{
			get{return YMax;}
		}
		public float XMIN
		{
			get{return XMin;}
		}
		public float YMIN
		{
			get{return YMin;}
		}
		public float OFFSETX
		{
			get{return offsetX;}
		}
		public float OFFSETY
		{
			get{return offsetY;}
		}
		public string PROGRAMHPGL
		{
			get{return ProgramHPGL;}	
		}
		public string PROGRAMMoveTo
		{
			get{return ProgramMoveTo;}	
		}
		public ArrayList ALPROGRAMMoveTo
		{
			get{return Lines;}	
            set{Lines = value;}	
		}
		public int LINES
		{
			get{return lineCount;}	
		}

		public HPGL(string progHPGL)
		{
			line = progHPGL;

//			HPGLName = "";
			XMax = -999999;
			YMax = -999999;
			XMin = 999999;
			YMin = 999999;

            //ElaboraHPGL();
		}

        public HPGL()
        {
            XMax = -999999;
            YMax = -999999;
            XMin = 999999;
            YMin = 999999;

            ProgramHPGL = "";
        }

		public void ElaboraHPGL(string line)
		{    
			lineCount = 0;
			string str2 = "";
            //draw = false;

			lineDiv = line.Split(';');

			foreach(string str in lineDiv)
			{
				if (str.Trim() != "")
				{
                    try
                    {
                        switch (str.Substring(0, 2))
                        {
                            case "IN":
                                ProgramWriteLine(str);
                                break;

                            case "VS":
                                ProgramWriteLine(str);
                                break;

                            case "WU":
                                ProgramWriteLine(str);
                                break;

                            case "PW":
                                ProgramWriteLine(str);
                                break;

                            case "SP":
                                ProgramWriteLine(str);

                                str2 = str.TrimStart("SP".ToCharArray());

                                if (str2 == "")
                                {
                                    SP = 1;
                                    break;
                                }

                                SP = Int32.Parse(str2);
                                break;

                            case "PU":
                                ProgramWriteLine(str);

                                str2 = str.TrimStart("PU".ToCharArray());

                                draw = false;

                                AddpXpY(str2);

                                break;

                            case "PD":
                                ProgramWriteLine(str);

                                str2 = str.TrimStart("PD".ToCharArray());

                                draw = true;

                                AddpXpY(str2);

                                break;

                            case "PA":
                                ProgramWriteLine(str);

                                str2 = str.TrimStart("PA".ToCharArray());

                                AddpXpY(str2);

                                break;

                            case "XX":
                                FINE = true;

                                break;
                        }
                    }

                    catch (Exception e)
                    { }
				}
			}
		}

		private void AddpXpY(string str2)
		{
			float pX = 0;
			float pY = 0;

			if (str2 == "")
			{
				return;
			}

			if (str2.IndexOf(' ') == -1)
				pars = str2.Split(',');
			else
				pars = str2.Split(' ');

            pX = (float)Int32.Parse(pars[0]) / 10.0f;
            pY = (float)Int32.Parse(pars[1]) / 10.0f;
							
			if(FINE == false) MinMaxXY(pX, pY);

			Lines.Add(new Linea(pX, pY, 0f, SP, draw));

			punto.X = pX;
			punto.Y = pY;
		}

		private void ProgramWriteLine(string str)
		{
			ProgramHPGL += str.Trim() + ';' + '\n';
			lineCount++;
		}

		private void MinMaxXY(float x, float y)
		{
			if (x > XMax) XMax = x;
			if (x < XMin) XMin = x;
			if (y > YMax) YMax = y;
			if (y < YMin) YMin = y;
		}
	}
}
