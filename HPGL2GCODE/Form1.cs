using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.IO;

namespace AsseTest
{
    public struct Linea
    {
        public Linea(double lx, double ly, double lz, int lsp, bool ldraw)
        {
            x = lx;
            y = ly;
            z = lz;
            sp = lsp;
            draw = ldraw;
        }

        public double x;
        public double y;
        public double z;
        public int sp;
        public bool draw;
    }

    public struct ImpColore
    {
        public ImpColore(CheckBox chb, InputBoxSmall.InputBoxSmall ibs, ComboBox cb)
        {
            Checked = chb;
            Profondita = ibs;
            Tool = cb;
        }

        public CheckBox Checked;
        public InputBoxSmall.InputBoxSmall Profondita;
        public ComboBox Tool;
    }

    public struct ImpUtensile
    {
        public ImpUtensile(InputBoxSmall.InputBoxSmall ibs1, InputBoxSmall.InputBoxSmall ibs2, InputBoxSmall.InputBoxSmall ibs3)
        {
            MaxProfondita = ibs1;
            FVelocita = ibs2;
            SVelocita = ibs3;
        }

        public InputBoxSmall.InputBoxSmall MaxProfondita;
        public InputBoxSmall.InputBoxSmall FVelocita;
        public InputBoxSmall.InputBoxSmall SVelocita;
    }

    public struct Impostazione
    {
        public Impostazione(ImpColore ic, ImpUtensile iu)
        {
            Colore = ic;
            Utensili = iu;
        }

        public ImpColore Colore;
        public ImpUtensile Utensili;
    }

    public class frmQuote : System.Windows.Forms.Form
    {
        private Graphics grfx;
        private Graphics grfxBm;
        private Bitmap bitmap;
        private Bitmap bitmapHPGL;

        private ArrayList alPoints = new ArrayList();
        private ArrayList alPens = new ArrayList();
        private Pen penMov = new Pen(System.Drawing.Color.White);
        private Pen pen = new Pen(System.Drawing.Color.White);
        private Point ptLast;
        private bool crta = false;

        private ArrayList Percorso = new ArrayList();

        private ArrayList DrawingLines;

        private Color col;

        bool programHPGLExist = false;

        private double UkupnaDuzina = 0.0;

        private double ZoomFactor = 1;
        private int shiftX = 0;
        private int shiftY = 0;

        private double zUP = 10.0;
        private double zDW = -1.0;
        private double zPP = 1.0;
        private int zSTP = 1;
        private double sizeX = 1.0;
        private double sizeY = 1.0;

        private int noLine = 0;

        private string FileName = "";

        //private string GC_X = "";
        //private string GC_Y = "";
        //private string GC_Z = "";
        //private string GC_F = "";
        //private string GC_G = "";
        //private string GC_M = "";

        private string GC_X_old = "";
        private string GC_Y_old = "";
        private string GC_Z_old = "";
        private string GC_F_old = "";
        private string GC_G_old = "";
        private string GC_M_old = "";
        private string GC_S_old = "";

        private struct Punto
        {
            public Punto(double x, double y)
            {
                X = x;
                Y = y;
            }
            public double X;
            public double Y;
        }

        private Punto punto = new Punto(0.0, 0.0);
        private Punto punto_old = new Punto(0.0, 0.0);
        private string[] pars = new string[2];
        private int[] VS = new int[256];
        private double[] PW = new double[256];
        private string[] lineDiv = new string[65535];
        private double sizeDraw;
        private double XMax = -9999999.0;
        private double YMax = -9999999.0;
        private double XMin = 9999999.0;
        private double YMin = 9999999.0;
        private double offsetXDraw = 0.0;
        private double offsetYDraw = 0.0;

        private string ProgramInProgramEditor = "";
        private System.Windows.Forms.Button btnApri;

        private System.Windows.Forms.Panel pnlDisegno;
        private Label txtUkupnaDuzina;
        private Label label1;
        private Label txtVisinaHPGL;
        private Label txtSirinaHPGL;
        private int NumeroTotaleLinee = 0;
        private OpenFileDialog openFileDialog1;
        private RichTextBox rtbHPGL;
        private Label label2;
        private Label txtNumeroTotaleLinee;
        private Button btnZoomOUT;
        private Button btnZoomIN;
        private Button btnZoomHOME;
        private Button btnShiftXdx;
        private Button btnShiftXsx;
        private Button btnShiftXdw;
        private Button btnShiftXup;
        private Button btnPAN;
        private Button btnDraw;
        private System.Windows.Forms.Timer timer1;
        private IContainer components;
        private Button btnSaveGCode;
        private RichTextBox rtbGCODE;

        Impostazioni fmImpostazioni = new Impostazioni();

        bool oldDraw = true;
        private TabControl tabTrasformazioni;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button btnApplicaPosizione;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Pozicija.Poz pozicijaCTRL;
        private Label txtYminmax;
        private Label txtXminmax;
        private Label label8;
        private Label label7;
        private Pozicija.Poz pozCTRL;
        private Pozicija.Poz pozPosizione;
        private CheckBox checkBox1;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabControl tabControl2;
        private TabPage tabPage5;
        private CheckBox cbScalaNonProporzionale;
        private Button btnScalaApplica;
        private Pozicija.Poz pozScala;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private CheckBox cbDimensioniNonProporzionale;
        private Button btnApplicaDimensioni;
        private Pozicija.Poz pozDimensioni;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private CheckBox checkBox4;
        private Button btnApplicaRotazione;
        private Pozicija.Poz pozRotazione;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private Button btnScalaVMirror;
        private Button btnScalaHMirror;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label30;
        private GroupBox groupBox1;
        private RichTextBox rtbComands;
        private RichTextBox rtbHPGLHiden;
        private TabPage tabPage6;
        private Label label32;
        private Label label31;
        private RichTextBox rtbFine;
        private RichTextBox rtbInizio;
        private CheckBox rbScriviNumeroRigha;
        private SaveFileDialog saveFileDialog1;
        private Button btnRegenerate;
        private Label label35;
        private Label txtNoLineeGCode;
        private Label label36;
        private CheckBox cbOtimizzaXDistanza;
        private TabPage tabPage7;
        private Label label37;
        private Label label49;
        private Label label48;
        private Label label47;
        private Label label46;
        private Label label50;
        private ComboBox cbTool1;
        private Label label52;
        private Label label51;
        private ComboBox cbTool8;
        private ComboBox cbTool7;
        private ComboBox cbTool6;
        private ComboBox cbTool5;
        private ComboBox cbTool4;
        private ComboBox cbTool3;
        private ComboBox cbTool2;
        private InputBox.InputBox txtPosizioneY;
        private InputBox.InputBox txtPosizioneX;
        private InputBox.InputBox txtScalaY;
        private InputBox.InputBox txtScalaX;
        private InputBox.InputBox txtDimensioniY;
        private InputBox.InputBox txtDimensioniX;
        private InputBox.InputBox txtRotazioneY;
        private InputBox.InputBox txtRotazioneX;
        private InputBox.InputBox txtRotazioneAngolo;
        private InputBox.InputBox txtImpostazioniNumeroPassi;
        private InputBox.InputBox txtImpostazioniZLavoro;
        private InputBox.InputBox txtImpostazioniZSicurezza;
        private InputBox.InputBox txtOtimizzaXDistanza;
        private InputBox.InputBox txtImpostazioniZPerPasso;
        private InputBoxSmall.InputBoxSmall ibColore8;
        private InputBoxSmall.InputBoxSmall ibColore7;
        private InputBoxSmall.InputBoxSmall ibColore6;
        private InputBoxSmall.InputBoxSmall ibColore5;
        private InputBoxSmall.InputBoxSmall ibColore4;
        private InputBoxSmall.InputBoxSmall ibColore3;
        private InputBoxSmall.InputBoxSmall ibColore2;
        private InputBoxSmall.InputBoxSmall ibColore1;
        private Label label54;
        private Label label53;
        private CheckBox cbColore2;
        private CheckBox cbColore1;
        private CheckBox cbColore8;
        private CheckBox cbColore7;
        private CheckBox cbColore6;
        private CheckBox cbColore5;
        private CheckBox cbColore4;
        private CheckBox cbColore3;
        private TabPage tabPage8;
        private Label label38;
        private Label label57;
        private InputBoxSmall.InputBoxSmall ibST1;
        private Label label56;
        private InputBoxSmall.InputBoxSmall ibFT1;
        private Label label55;
        private InputBoxSmall.InputBoxSmall ibMaxT1;
        private Label label45;
        private Label label44;
        private Label label43;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private InputBoxSmall.InputBoxSmall ibST8;
        private InputBoxSmall.InputBoxSmall ibFT8;
        private InputBoxSmall.InputBoxSmall ibMaxT8;
        private InputBoxSmall.InputBoxSmall ibST7;
        private InputBoxSmall.InputBoxSmall ibFT7;
        private InputBoxSmall.InputBoxSmall ibMaxT7;
        private InputBoxSmall.InputBoxSmall ibST6;
        private InputBoxSmall.InputBoxSmall ibFT6;
        private InputBoxSmall.InputBoxSmall ibMaxT6;
        private InputBoxSmall.InputBoxSmall ibST5;
        private InputBoxSmall.InputBoxSmall ibFT5;
        private InputBoxSmall.InputBoxSmall ibMaxT5;
        private InputBoxSmall.InputBoxSmall ibST4;
        private InputBoxSmall.InputBoxSmall ibFT4;
        private InputBoxSmall.InputBoxSmall ibMaxT4;
        private InputBoxSmall.InputBoxSmall ibST3;
        private InputBoxSmall.InputBoxSmall ibFT3;
        private InputBoxSmall.InputBoxSmall ibMaxT3;
        private InputBoxSmall.InputBoxSmall ibST2;
        private InputBoxSmall.InputBoxSmall ibFT2;
        private InputBoxSmall.InputBoxSmall ibMaxT2;
        private CheckBox cbOrdinaXColore;

        ImpColore[] impC = new ImpColore[8];
        ImpUtensile[] impU = new ImpUtensile[8];
        Impostazione[] impo = new Impostazione[8];

        HPGL hpgl = null;

        public frmQuote()
        {
            InitializeComponent();

            //this.pozicijaCTRL. += new System.Windows.Forms.MouseEventHandler(this.userControl11_MouseClick);

            grfx = pnlDisegno.CreateGraphics();
            grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;

            sizeDraw = (double)grfx.VisibleClipBounds.Width / 300f;

            Size size = pnlDisegno.Size;
            bitmap = new Bitmap(size.Width, size.Height);
            grfxBm = Graphics.FromImage(bitmap);
            grfxBm.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grfxBm.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            grfxBm.Clear(System.Drawing.Color.DarkKhaki);

            hpgl = new HPGL();

            pozPosizione.PozicijaChange += new Pozicija.Poz.PozicijaChangeHandler(poz1_PozicijaChange);
            txtPosizioneX.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(inputBox1_inputBoxChange);

            txtPosizioneX.TEXT = ToFloatStr(0.0);
            txtPosizioneY.TEXT = ToFloatStr(0.0);

            txtScalaX.TEXT = ToFloatStr(100.0);
            txtScalaY.TEXT = ToFloatStr(100.0);

            txtDimensioniX.TEXT = ToFloatStr(0.0);
            txtDimensioniY.TEXT = ToFloatStr(0.0);

            txtRotazioneX.TEXT = ToFloatStr(0.0);
            txtRotazioneY.TEXT = ToFloatStr(0.0);
            txtRotazioneAngolo.TEXT = ToFloatStr(0.0);

            //txtImpostazioniZSicurezza.TEXT = ToFloatStr(10.0);
            //txtImpostazioniZLavoro.TEXT = ToFloatStr(-1.0);
            //txtImpostazioniNumeroPassi.TEXT = ToFloatStr(1.0);
            //txtImpostazioniNumeroPassi.VMIN = 1.0;
            //txtImpostazioniNumeroPassi.VINT = true;

            //txtImpostazioniZPerPasso.TEXT = ToFloatStr(1.0);
            //txtImpostazioniZPerPasso.CHANGE = false;

            //txtOtimizzaXDistanza.VMIN = 0.0;

            UcitajImpostazioni();            

            impC[0] = new ImpColore(cbColore1, ibColore1, cbTool1);
            impC[1] = new ImpColore(cbColore2, ibColore2, cbTool2);
            impC[2] = new ImpColore(cbColore3, ibColore3, cbTool3);
            impC[3] = new ImpColore(cbColore4, ibColore4, cbTool4);
            impC[4] = new ImpColore(cbColore5, ibColore5, cbTool5);
            impC[5] = new ImpColore(cbColore6, ibColore6, cbTool6);
            impC[6] = new ImpColore(cbColore7, ibColore7, cbTool7);
            impC[7] = new ImpColore(cbColore8, ibColore8, cbTool8);            

            impU[0] = new ImpUtensile(ibMaxT1, ibFT1, ibST1);
            impU[1] = new ImpUtensile(ibMaxT2, ibFT2, ibST2);
            impU[2] = new ImpUtensile(ibMaxT3, ibFT3, ibST3);
            impU[3] = new ImpUtensile(ibMaxT4, ibFT4, ibST4);
            impU[4] = new ImpUtensile(ibMaxT5, ibFT5, ibST5);
            impU[5] = new ImpUtensile(ibMaxT6, ibFT6, ibST6);
            impU[6] = new ImpUtensile(ibMaxT7, ibFT7, ibST7);
            impU[7] = new ImpUtensile(ibMaxT8, ibFT8, ibST8);            

            impo[0] = new Impostazione(impC[0], impU[0]);
            impo[1] = new Impostazione(impC[1], impU[1]);
            impo[2] = new Impostazione(impC[2], impU[2]);
            impo[3] = new Impostazione(impC[3], impU[3]);
            impo[4] = new Impostazione(impC[4], impU[4]);
            impo[5] = new Impostazione(impC[5], impU[5]);
            impo[6] = new Impostazione(impC[6], impU[6]);
            impo[7] = new Impostazione(impC[7], impU[7]);
        }

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuote));
            this.pnlDisegno = new System.Windows.Forms.Panel();
            this.btnPAN = new System.Windows.Forms.Button();
            this.btnShiftXdx = new System.Windows.Forms.Button();
            this.btnShiftXsx = new System.Windows.Forms.Button();
            this.btnShiftXdw = new System.Windows.Forms.Button();
            this.btnShiftXup = new System.Windows.Forms.Button();
            this.btnZoomHOME = new System.Windows.Forms.Button();
            this.btnZoomOUT = new System.Windows.Forms.Button();
            this.btnZoomIN = new System.Windows.Forms.Button();
            this.txtVisinaHPGL = new System.Windows.Forms.Label();
            this.txtSirinaHPGL = new System.Windows.Forms.Label();
            this.txtYminmax = new System.Windows.Forms.Label();
            this.txtXminmax = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSaveGCode = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.txtNumeroTotaleLinee = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUkupnaDuzina = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnApri = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.rtbHPGL = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rtbGCODE = new System.Windows.Forms.RichTextBox();
            this.tabTrasformazioni = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtPosizioneY = new InputBox.InputBox();
            this.txtPosizioneX = new InputBox.InputBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnApplicaPosizione = new System.Windows.Forms.Button();
            this.pozPosizione = new Pozicija.Poz();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtScalaY = new InputBox.InputBox();
            this.txtScalaX = new InputBox.InputBox();
            this.label23 = new System.Windows.Forms.Label();
            this.btnScalaVMirror = new System.Windows.Forms.Button();
            this.btnScalaHMirror = new System.Windows.Forms.Button();
            this.cbScalaNonProporzionale = new System.Windows.Forms.CheckBox();
            this.btnScalaApplica = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pozScala = new Pozicija.Poz();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtDimensioniY = new InputBox.InputBox();
            this.txtDimensioniX = new InputBox.InputBox();
            this.cbDimensioniNonProporzionale = new System.Windows.Forms.CheckBox();
            this.btnApplicaDimensioni = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.pozDimensioni = new Pozicija.Poz();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtRotazioneY = new InputBox.InputBox();
            this.txtRotazioneX = new InputBox.InputBox();
            this.txtRotazioneAngolo = new InputBox.InputBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.btnApplicaRotazione = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.pozRotazione = new Pozicija.Poz();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtOtimizzaXDistanza = new InputBox.InputBox();
            this.txtImpostazioniZPerPasso = new InputBox.InputBox();
            this.txtImpostazioniNumeroPassi = new InputBox.InputBox();
            this.txtImpostazioniZLavoro = new InputBox.InputBox();
            this.txtImpostazioniZSicurezza = new InputBox.InputBox();
            this.label36 = new System.Windows.Forms.Label();
            this.cbOtimizzaXDistanza = new System.Windows.Forms.CheckBox();
            this.rbScriviNumeroRigha = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.rtbFine = new System.Windows.Forms.RichTextBox();
            this.rtbInizio = new System.Windows.Forms.RichTextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.cbColore8 = new System.Windows.Forms.CheckBox();
            this.cbColore7 = new System.Windows.Forms.CheckBox();
            this.cbColore6 = new System.Windows.Forms.CheckBox();
            this.cbColore5 = new System.Windows.Forms.CheckBox();
            this.cbColore4 = new System.Windows.Forms.CheckBox();
            this.cbColore3 = new System.Windows.Forms.CheckBox();
            this.cbColore2 = new System.Windows.Forms.CheckBox();
            this.cbColore1 = new System.Windows.Forms.CheckBox();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.ibColore8 = new InputBoxSmall.InputBoxSmall();
            this.ibColore7 = new InputBoxSmall.InputBoxSmall();
            this.ibColore6 = new InputBoxSmall.InputBoxSmall();
            this.ibColore5 = new InputBoxSmall.InputBoxSmall();
            this.ibColore4 = new InputBoxSmall.InputBoxSmall();
            this.ibColore3 = new InputBoxSmall.InputBoxSmall();
            this.ibColore2 = new InputBoxSmall.InputBoxSmall();
            this.ibColore1 = new InputBoxSmall.InputBoxSmall();
            this.cbTool8 = new System.Windows.Forms.ComboBox();
            this.cbTool7 = new System.Windows.Forms.ComboBox();
            this.cbTool6 = new System.Windows.Forms.ComboBox();
            this.cbTool5 = new System.Windows.Forms.ComboBox();
            this.cbTool4 = new System.Windows.Forms.ComboBox();
            this.cbTool3 = new System.Windows.Forms.ComboBox();
            this.cbTool2 = new System.Windows.Forms.ComboBox();
            this.cbTool1 = new System.Windows.Forms.ComboBox();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.ibST8 = new InputBoxSmall.InputBoxSmall();
            this.ibFT8 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT8 = new InputBoxSmall.InputBoxSmall();
            this.ibST7 = new InputBoxSmall.InputBoxSmall();
            this.ibFT7 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT7 = new InputBoxSmall.InputBoxSmall();
            this.ibST6 = new InputBoxSmall.InputBoxSmall();
            this.ibFT6 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT6 = new InputBoxSmall.InputBoxSmall();
            this.ibST5 = new InputBoxSmall.InputBoxSmall();
            this.ibFT5 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT5 = new InputBoxSmall.InputBoxSmall();
            this.ibST4 = new InputBoxSmall.InputBoxSmall();
            this.ibFT4 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT4 = new InputBoxSmall.InputBoxSmall();
            this.ibST3 = new InputBoxSmall.InputBoxSmall();
            this.ibFT3 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT3 = new InputBoxSmall.InputBoxSmall();
            this.ibST2 = new InputBoxSmall.InputBoxSmall();
            this.ibFT2 = new InputBoxSmall.InputBoxSmall();
            this.ibMaxT2 = new InputBoxSmall.InputBoxSmall();
            this.label57 = new System.Windows.Forms.Label();
            this.ibST1 = new InputBoxSmall.InputBoxSmall();
            this.label56 = new System.Windows.Forms.Label();
            this.ibFT1 = new InputBoxSmall.InputBoxSmall();
            this.label55 = new System.Windows.Forms.Label();
            this.ibMaxT1 = new InputBoxSmall.InputBoxSmall();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txtNoLineeGCode = new System.Windows.Forms.Label();
            this.rtbComands = new System.Windows.Forms.RichTextBox();
            this.rtbHPGLHiden = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnRegenerate = new System.Windows.Forms.Button();
            this.cbOrdinaXColore = new System.Windows.Forms.CheckBox();
            this.pnlDisegno.SuspendLayout();
            this.tabTrasformazioni.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDisegno
            // 
            this.pnlDisegno.BackColor = System.Drawing.Color.DarkKhaki;
            this.pnlDisegno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDisegno.Controls.Add(this.btnPAN);
            this.pnlDisegno.Controls.Add(this.btnShiftXdx);
            this.pnlDisegno.Controls.Add(this.btnShiftXsx);
            this.pnlDisegno.Controls.Add(this.btnShiftXdw);
            this.pnlDisegno.Controls.Add(this.btnShiftXup);
            this.pnlDisegno.Controls.Add(this.btnZoomHOME);
            this.pnlDisegno.Controls.Add(this.btnZoomOUT);
            this.pnlDisegno.Controls.Add(this.btnZoomIN);
            this.pnlDisegno.Controls.Add(this.txtVisinaHPGL);
            this.pnlDisegno.Controls.Add(this.txtSirinaHPGL);
            this.pnlDisegno.Location = new System.Drawing.Point(9, 13);
            this.pnlDisegno.Name = "pnlDisegno";
            this.pnlDisegno.Size = new System.Drawing.Size(543, 501);
            this.pnlDisegno.TabIndex = 26;
            this.pnlDisegno.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlDisegno_MouseDown);
            this.pnlDisegno.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlDisegno_MouseUp);
            // 
            // btnPAN
            // 
            this.btnPAN.BackColor = System.Drawing.Color.Transparent;
            this.btnPAN.ForeColor = System.Drawing.Color.Black;
            this.btnPAN.Location = new System.Drawing.Point(477, 24);
            this.btnPAN.Name = "btnPAN";
            this.btnPAN.Size = new System.Drawing.Size(31, 23);
            this.btnPAN.TabIndex = 13;
            this.btnPAN.Text = "P";
            this.btnPAN.UseVisualStyleBackColor = false;
            this.btnPAN.Click += new System.EventHandler(this.btnPAN_Click);
            // 
            // btnShiftXdx
            // 
            this.btnShiftXdx.Location = new System.Drawing.Point(507, 24);
            this.btnShiftXdx.Name = "btnShiftXdx";
            this.btnShiftXdx.Size = new System.Drawing.Size(31, 23);
            this.btnShiftXdx.TabIndex = 12;
            this.btnShiftXdx.Text = ">";
            this.btnShiftXdx.UseVisualStyleBackColor = true;
            this.btnShiftXdx.Click += new System.EventHandler(this.btnShiftXdx_Click);
            // 
            // btnShiftXsx
            // 
            this.btnShiftXsx.Location = new System.Drawing.Point(447, 24);
            this.btnShiftXsx.Name = "btnShiftXsx";
            this.btnShiftXsx.Size = new System.Drawing.Size(31, 23);
            this.btnShiftXsx.TabIndex = 11;
            this.btnShiftXsx.Text = "<";
            this.btnShiftXsx.UseVisualStyleBackColor = true;
            this.btnShiftXsx.Click += new System.EventHandler(this.btnShiftXsx_Click);
            // 
            // btnShiftXdw
            // 
            this.btnShiftXdw.Location = new System.Drawing.Point(477, 46);
            this.btnShiftXdw.Name = "btnShiftXdw";
            this.btnShiftXdw.Size = new System.Drawing.Size(31, 23);
            this.btnShiftXdw.TabIndex = 10;
            this.btnShiftXdw.Text = "v";
            this.btnShiftXdw.UseVisualStyleBackColor = true;
            this.btnShiftXdw.Click += new System.EventHandler(this.btnShiftXdw_Click);
            // 
            // btnShiftXup
            // 
            this.btnShiftXup.Location = new System.Drawing.Point(477, 3);
            this.btnShiftXup.Name = "btnShiftXup";
            this.btnShiftXup.Size = new System.Drawing.Size(31, 23);
            this.btnShiftXup.TabIndex = 9;
            this.btnShiftXup.Text = "^";
            this.btnShiftXup.UseVisualStyleBackColor = true;
            this.btnShiftXup.Click += new System.EventHandler(this.btnShiftXup_Click);
            // 
            // btnZoomHOME
            // 
            this.btnZoomHOME.Location = new System.Drawing.Point(3, 3);
            this.btnZoomHOME.Name = "btnZoomHOME";
            this.btnZoomHOME.Size = new System.Drawing.Size(49, 23);
            this.btnZoomHOME.TabIndex = 8;
            this.btnZoomHOME.Text = "Home";
            this.btnZoomHOME.UseVisualStyleBackColor = true;
            this.btnZoomHOME.Click += new System.EventHandler(this.btnZoomHOME_Click);
            // 
            // btnZoomOUT
            // 
            this.btnZoomOUT.Location = new System.Drawing.Point(81, 3);
            this.btnZoomOUT.Name = "btnZoomOUT";
            this.btnZoomOUT.Size = new System.Drawing.Size(31, 23);
            this.btnZoomOUT.TabIndex = 7;
            this.btnZoomOUT.Text = "Z-";
            this.btnZoomOUT.UseVisualStyleBackColor = true;
            this.btnZoomOUT.Click += new System.EventHandler(this.btnZoomOUT_Click);
            // 
            // btnZoomIN
            // 
            this.btnZoomIN.Location = new System.Drawing.Point(51, 3);
            this.btnZoomIN.Name = "btnZoomIN";
            this.btnZoomIN.Size = new System.Drawing.Size(31, 23);
            this.btnZoomIN.TabIndex = 6;
            this.btnZoomIN.Text = "Z+";
            this.btnZoomIN.UseVisualStyleBackColor = true;
            this.btnZoomIN.Click += new System.EventHandler(this.btnZoomIN_Click);
            // 
            // txtVisinaHPGL
            // 
            this.txtVisinaHPGL.AutoSize = true;
            this.txtVisinaHPGL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVisinaHPGL.ForeColor = System.Drawing.Color.White;
            this.txtVisinaHPGL.Location = new System.Drawing.Point(486, 228);
            this.txtVisinaHPGL.Name = "txtVisinaHPGL";
            this.txtVisinaHPGL.Size = new System.Drawing.Size(50, 13);
            this.txtVisinaHPGL.TabIndex = 3;
            this.txtVisinaHPGL.Text = "0.00mm";
            this.txtVisinaHPGL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.txtVisinaHPGL.Visible = false;
            // 
            // txtSirinaHPGL
            // 
            this.txtSirinaHPGL.AutoSize = true;
            this.txtSirinaHPGL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSirinaHPGL.ForeColor = System.Drawing.Color.White;
            this.txtSirinaHPGL.Location = new System.Drawing.Point(240, 481);
            this.txtSirinaHPGL.Name = "txtSirinaHPGL";
            this.txtSirinaHPGL.Size = new System.Drawing.Size(50, 13);
            this.txtSirinaHPGL.TabIndex = 2;
            this.txtSirinaHPGL.Text = "0.00mm";
            this.txtSirinaHPGL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.txtSirinaHPGL.Visible = false;
            // 
            // txtYminmax
            // 
            this.txtYminmax.AutoSize = true;
            this.txtYminmax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYminmax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtYminmax.Location = new System.Drawing.Point(29, 64);
            this.txtYminmax.Name = "txtYminmax";
            this.txtYminmax.Size = new System.Drawing.Size(46, 13);
            this.txtYminmax.TabIndex = 19;
            this.txtYminmax.Text = "0.0 - 0.0";
            // 
            // txtXminmax
            // 
            this.txtXminmax.AutoSize = true;
            this.txtXminmax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXminmax.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtXminmax.Location = new System.Drawing.Point(29, 51);
            this.txtXminmax.Name = "txtXminmax";
            this.txtXminmax.Size = new System.Drawing.Size(46, 13);
            this.txtXminmax.TabIndex = 18;
            this.txtXminmax.Text = "0.0 - 0.0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(6, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(6, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "X:";
            // 
            // btnSaveGCode
            // 
            this.btnSaveGCode.Enabled = false;
            this.btnSaveGCode.Location = new System.Drawing.Point(763, 429);
            this.btnSaveGCode.Name = "btnSaveGCode";
            this.btnSaveGCode.Size = new System.Drawing.Size(114, 23);
            this.btnSaveGCode.TabIndex = 15;
            this.btnSaveGCode.Text = "Save Gcode";
            this.btnSaveGCode.UseVisualStyleBackColor = true;
            this.btnSaveGCode.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnDraw
            // 
            this.btnDraw.Enabled = false;
            this.btnDraw.Location = new System.Drawing.Point(763, 459);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(114, 23);
            this.btnDraw.TabIndex = 14;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // txtNumeroTotaleLinee
            // 
            this.txtNumeroTotaleLinee.AutoSize = true;
            this.txtNumeroTotaleLinee.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroTotaleLinee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtNumeroTotaleLinee.Location = new System.Drawing.Point(113, 35);
            this.txtNumeroTotaleLinee.Name = "txtNumeroTotaleLinee";
            this.txtNumeroTotaleLinee.Size = new System.Drawing.Size(13, 13);
            this.txtNumeroTotaleLinee.TabIndex = 5;
            this.txtNumeroTotaleLinee.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Numero totale linee:";
            // 
            // txtUkupnaDuzina
            // 
            this.txtUkupnaDuzina.AutoSize = true;
            this.txtUkupnaDuzina.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUkupnaDuzina.Location = new System.Drawing.Point(113, 18);
            this.txtUkupnaDuzina.Name = "txtUkupnaDuzina";
            this.txtUkupnaDuzina.Size = new System.Drawing.Size(50, 13);
            this.txtUkupnaDuzina.TabIndex = 1;
            this.txtUkupnaDuzina.Text = "0.00mm";
            this.txtUkupnaDuzina.Click += new System.EventHandler(this.txtUkupnaDuzina_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lunghezza totale:";
            // 
            // btnApri
            // 
            this.btnApri.BackColor = System.Drawing.SystemColors.Control;
            this.btnApri.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApri.Location = new System.Drawing.Point(763, 399);
            this.btnApri.Name = "btnApri";
            this.btnApri.Size = new System.Drawing.Size(114, 23);
            this.btnApri.TabIndex = 27;
            this.btnApri.Text = "Open HPGL";
            this.btnApri.UseVisualStyleBackColor = true;
            this.btnApri.Click += new System.EventHandler(this.btnApri_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // rtbHPGL
            // 
            this.rtbHPGL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHPGL.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHPGL.Location = new System.Drawing.Point(558, 13);
            this.rtbHPGL.Name = "rtbHPGL";
            this.rtbHPGL.Size = new System.Drawing.Size(132, 272);
            this.rtbHPGL.TabIndex = 28;
            this.rtbHPGL.Text = "";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rtbGCODE
            // 
            this.rtbGCODE.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbGCODE.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbGCODE.Location = new System.Drawing.Point(696, 13);
            this.rtbGCODE.Name = "rtbGCODE";
            this.rtbGCODE.Size = new System.Drawing.Size(181, 366);
            this.rtbGCODE.TabIndex = 30;
            this.rtbGCODE.Text = "";
            this.rtbGCODE.TextChanged += new System.EventHandler(this.rtbGCODE_TextChanged);
            // 
            // tabTrasformazioni
            // 
            this.tabTrasformazioni.Controls.Add(this.tabPage1);
            this.tabTrasformazioni.Controls.Add(this.tabPage2);
            this.tabTrasformazioni.Controls.Add(this.tabPage3);
            this.tabTrasformazioni.Controls.Add(this.tabPage4);
            this.tabTrasformazioni.Location = new System.Drawing.Point(883, 13);
            this.tabTrasformazioni.Multiline = true;
            this.tabTrasformazioni.Name = "tabTrasformazioni";
            this.tabTrasformazioni.SelectedIndex = 0;
            this.tabTrasformazioni.Size = new System.Drawing.Size(303, 235);
            this.tabTrasformazioni.TabIndex = 31;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage1.Controls.Add(this.txtPosizioneY);
            this.tabPage1.Controls.Add(this.txtPosizioneX);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.btnApplicaPosizione);
            this.tabPage1.Controls.Add(this.pozPosizione);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(295, 209);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Posizione";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtPosizioneY
            // 
            this.txtPosizioneY.BackColor = System.Drawing.Color.Transparent;
            this.txtPosizioneY.Location = new System.Drawing.Point(44, 45);
            this.txtPosizioneY.Name = "txtPosizioneY";
            this.txtPosizioneY.Size = new System.Drawing.Size(121, 32);
            this.txtPosizioneY.TabIndex = 35;
            this.txtPosizioneY.TEXT = "0,00";
            this.txtPosizioneY.VALUE = 0;
            // 
            // txtPosizioneX
            // 
            this.txtPosizioneX.BackColor = System.Drawing.Color.Transparent;
            this.txtPosizioneX.Location = new System.Drawing.Point(44, 15);
            this.txtPosizioneX.Name = "txtPosizioneX";
            this.txtPosizioneX.Size = new System.Drawing.Size(121, 32);
            this.txtPosizioneX.TabIndex = 34;
            this.txtPosizioneX.TEXT = "0,00";
            this.txtPosizioneX.VALUE = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(26, 89);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 33;
            this.checkBox1.Text = "Posizione relativa";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // btnApplicaPosizione
            // 
            this.btnApplicaPosizione.Location = new System.Drawing.Point(25, 178);
            this.btnApplicaPosizione.Name = "btnApplicaPosizione";
            this.btnApplicaPosizione.Size = new System.Drawing.Size(248, 23);
            this.btnApplicaPosizione.TabIndex = 6;
            this.btnApplicaPosizione.Text = "Applica";
            this.btnApplicaPosizione.UseVisualStyleBackColor = true;
            this.btnApplicaPosizione.Click += new System.EventHandler(this.btnApplicaPosizione_Click);
            // 
            // pozPosizione
            // 
            this.pozPosizione.BackColor = System.Drawing.Color.Transparent;
            this.pozPosizione.Location = new System.Drawing.Point(21, 111);
            this.pozPosizione.Name = "pozPosizione";
            this.pozPosizione.Size = new System.Drawing.Size(58, 60);
            this.pozPosizione.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(168, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(168, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "mm";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage2.Controls.Add(this.txtScalaY);
            this.tabPage2.Controls.Add(this.txtScalaX);
            this.tabPage2.Controls.Add(this.label23);
            this.tabPage2.Controls.Add(this.btnScalaVMirror);
            this.tabPage2.Controls.Add(this.btnScalaHMirror);
            this.tabPage2.Controls.Add(this.cbScalaNonProporzionale);
            this.tabPage2.Controls.Add(this.btnScalaApplica);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.pozScala);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(295, 209);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Scala";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtScalaY
            // 
            this.txtScalaY.BackColor = System.Drawing.Color.Transparent;
            this.txtScalaY.Location = new System.Drawing.Point(44, 45);
            this.txtScalaY.Name = "txtScalaY";
            this.txtScalaY.Size = new System.Drawing.Size(121, 32);
            this.txtScalaY.TabIndex = 49;
            this.txtScalaY.TEXT = "0,00";
            this.txtScalaY.VALUE = 0;
            this.txtScalaY.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtScalaY_inputBoxChange);
            // 
            // txtScalaX
            // 
            this.txtScalaX.BackColor = System.Drawing.Color.Transparent;
            this.txtScalaX.Location = new System.Drawing.Point(44, 15);
            this.txtScalaX.Name = "txtScalaX";
            this.txtScalaX.Size = new System.Drawing.Size(121, 32);
            this.txtScalaX.TabIndex = 48;
            this.txtScalaX.TEXT = "0,00";
            this.txtScalaX.VALUE = 0;
            this.txtScalaX.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtScalaX_inputBoxChange);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(192, 3);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(39, 13);
            this.label23.TabIndex = 47;
            this.label23.Text = "Rifletti:";
            // 
            // btnScalaVMirror
            // 
            this.btnScalaVMirror.BackColor = System.Drawing.Color.Tan;
            this.btnScalaVMirror.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnScalaVMirror.Location = new System.Drawing.Point(208, 52);
            this.btnScalaVMirror.Name = "btnScalaVMirror";
            this.btnScalaVMirror.Size = new System.Drawing.Size(40, 25);
            this.btnScalaVMirror.TabIndex = 46;
            this.btnScalaVMirror.Text = "V";
            this.btnScalaVMirror.UseVisualStyleBackColor = false;
            this.btnScalaVMirror.Click += new System.EventHandler(this.btnScalaVMirror_Click);
            // 
            // btnScalaHMirror
            // 
            this.btnScalaHMirror.Location = new System.Drawing.Point(208, 19);
            this.btnScalaHMirror.Name = "btnScalaHMirror";
            this.btnScalaHMirror.Size = new System.Drawing.Size(40, 25);
            this.btnScalaHMirror.TabIndex = 20;
            this.btnScalaHMirror.Text = "H";
            this.btnScalaHMirror.UseVisualStyleBackColor = true;
            this.btnScalaHMirror.Click += new System.EventHandler(this.btnScalaHMirror_Click);
            // 
            // cbScalaNonProporzionale
            // 
            this.cbScalaNonProporzionale.AutoSize = true;
            this.cbScalaNonProporzionale.BackColor = System.Drawing.Color.Transparent;
            this.cbScalaNonProporzionale.Checked = true;
            this.cbScalaNonProporzionale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbScalaNonProporzionale.Location = new System.Drawing.Point(26, 89);
            this.cbScalaNonProporzionale.Name = "cbScalaNonProporzionale";
            this.cbScalaNonProporzionale.Size = new System.Drawing.Size(112, 17);
            this.cbScalaNonProporzionale.TabIndex = 43;
            this.cbScalaNonProporzionale.Text = "Non proporzionale";
            this.cbScalaNonProporzionale.UseVisualStyleBackColor = false;
            this.cbScalaNonProporzionale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cbScalaNonProporzionale_MouseDown);
            this.cbScalaNonProporzionale.CheckedChanged += new System.EventHandler(this.cbScalaNonProporzionale_CheckedChanged);
            // 
            // btnScalaApplica
            // 
            this.btnScalaApplica.Location = new System.Drawing.Point(25, 178);
            this.btnScalaApplica.Name = "btnScalaApplica";
            this.btnScalaApplica.Size = new System.Drawing.Size(248, 23);
            this.btnScalaApplica.TabIndex = 40;
            this.btnScalaApplica.Text = "Applica";
            this.btnScalaApplica.UseVisualStyleBackColor = true;
            this.btnScalaApplica.Click += new System.EventHandler(this.btnScalaApplica_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(24, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 36;
            this.label9.Text = "X";
            this.label9.DoubleClick += new System.EventHandler(this.label9_DoubleClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(163, 56);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 16);
            this.label10.TabIndex = 39;
            this.label10.Text = "%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(25, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 16);
            this.label11.TabIndex = 37;
            this.label11.Text = "Y";
            this.label11.DoubleClick += new System.EventHandler(this.label11_DoubleClick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(163, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 16);
            this.label12.TabIndex = 38;
            this.label12.Text = "%";
            // 
            // pozScala
            // 
            this.pozScala.BackColor = System.Drawing.Color.Transparent;
            this.pozScala.Location = new System.Drawing.Point(21, 111);
            this.pozScala.Name = "pozScala";
            this.pozScala.Size = new System.Drawing.Size(58, 60);
            this.pozScala.TabIndex = 42;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage3.Controls.Add(this.txtDimensioniY);
            this.tabPage3.Controls.Add(this.txtDimensioniX);
            this.tabPage3.Controls.Add(this.cbDimensioniNonProporzionale);
            this.tabPage3.Controls.Add(this.btnApplicaDimensioni);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.pozDimensioni);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(295, 209);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Dimensioni";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtDimensioniY
            // 
            this.txtDimensioniY.BackColor = System.Drawing.Color.Transparent;
            this.txtDimensioniY.Location = new System.Drawing.Point(44, 45);
            this.txtDimensioniY.Name = "txtDimensioniY";
            this.txtDimensioniY.Size = new System.Drawing.Size(121, 32);
            this.txtDimensioniY.TabIndex = 45;
            this.txtDimensioniY.TEXT = "0,00";
            this.txtDimensioniY.VALUE = 0;
            this.txtDimensioniY.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtDimensioniY_inputBoxChange);
            // 
            // txtDimensioniX
            // 
            this.txtDimensioniX.BackColor = System.Drawing.Color.Transparent;
            this.txtDimensioniX.Location = new System.Drawing.Point(44, 15);
            this.txtDimensioniX.Name = "txtDimensioniX";
            this.txtDimensioniX.Size = new System.Drawing.Size(121, 32);
            this.txtDimensioniX.TabIndex = 44;
            this.txtDimensioniX.TEXT = "0,00";
            this.txtDimensioniX.VALUE = 0;
            this.txtDimensioniX.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtDimensioniX_inputBoxChange);
            // 
            // cbDimensioniNonProporzionale
            // 
            this.cbDimensioniNonProporzionale.AutoSize = true;
            this.cbDimensioniNonProporzionale.BackColor = System.Drawing.Color.Transparent;
            this.cbDimensioniNonProporzionale.Checked = true;
            this.cbDimensioniNonProporzionale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDimensioniNonProporzionale.Location = new System.Drawing.Point(26, 89);
            this.cbDimensioniNonProporzionale.Name = "cbDimensioniNonProporzionale";
            this.cbDimensioniNonProporzionale.Size = new System.Drawing.Size(112, 17);
            this.cbDimensioniNonProporzionale.TabIndex = 43;
            this.cbDimensioniNonProporzionale.Text = "Non proporzionale";
            this.cbDimensioniNonProporzionale.UseVisualStyleBackColor = false;
            this.cbDimensioniNonProporzionale.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cbDimensioniNonProporzionale_MouseDown);
            this.cbDimensioniNonProporzionale.CheckedChanged += new System.EventHandler(this.cbDimensioniNonProporzionale_CheckedChanged);
            // 
            // btnApplicaDimensioni
            // 
            this.btnApplicaDimensioni.Location = new System.Drawing.Point(25, 178);
            this.btnApplicaDimensioni.Name = "btnApplicaDimensioni";
            this.btnApplicaDimensioni.Size = new System.Drawing.Size(248, 23);
            this.btnApplicaDimensioni.TabIndex = 40;
            this.btnApplicaDimensioni.Text = "Applica";
            this.btnApplicaDimensioni.UseVisualStyleBackColor = true;
            this.btnApplicaDimensioni.Click += new System.EventHandler(this.btnApplicaDimensioni_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(24, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 16);
            this.label13.TabIndex = 36;
            this.label13.Text = "X";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(168, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 16);
            this.label14.TabIndex = 39;
            this.label14.Text = "mm";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(25, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 16);
            this.label15.TabIndex = 37;
            this.label15.Text = "Y";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(168, 24);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 16);
            this.label16.TabIndex = 38;
            this.label16.Text = "mm";
            // 
            // pozDimensioni
            // 
            this.pozDimensioni.BackColor = System.Drawing.Color.Transparent;
            this.pozDimensioni.Location = new System.Drawing.Point(21, 111);
            this.pozDimensioni.Name = "pozDimensioni";
            this.pozDimensioni.Size = new System.Drawing.Size(58, 60);
            this.pozDimensioni.TabIndex = 42;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage4.Controls.Add(this.txtRotazioneY);
            this.tabPage4.Controls.Add(this.txtRotazioneX);
            this.tabPage4.Controls.Add(this.txtRotazioneAngolo);
            this.tabPage4.Controls.Add(this.label21);
            this.tabPage4.Controls.Add(this.label22);
            this.tabPage4.Controls.Add(this.checkBox4);
            this.tabPage4.Controls.Add(this.btnApplicaRotazione);
            this.tabPage4.Controls.Add(this.label17);
            this.tabPage4.Controls.Add(this.label18);
            this.tabPage4.Controls.Add(this.label19);
            this.tabPage4.Controls.Add(this.label20);
            this.tabPage4.Controls.Add(this.pozRotazione);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(295, 209);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Rotazione";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtRotazioneY
            // 
            this.txtRotazioneY.BackColor = System.Drawing.Color.Transparent;
            this.txtRotazioneY.Location = new System.Drawing.Point(44, 75);
            this.txtRotazioneY.Name = "txtRotazioneY";
            this.txtRotazioneY.Size = new System.Drawing.Size(121, 32);
            this.txtRotazioneY.TabIndex = 50;
            this.txtRotazioneY.TEXT = "0,00";
            this.txtRotazioneY.VALUE = 0;
            // 
            // txtRotazioneX
            // 
            this.txtRotazioneX.BackColor = System.Drawing.Color.Transparent;
            this.txtRotazioneX.Location = new System.Drawing.Point(44, 45);
            this.txtRotazioneX.Name = "txtRotazioneX";
            this.txtRotazioneX.Size = new System.Drawing.Size(121, 32);
            this.txtRotazioneX.TabIndex = 49;
            this.txtRotazioneX.TEXT = "0,00";
            this.txtRotazioneX.VALUE = 0;
            // 
            // txtRotazioneAngolo
            // 
            this.txtRotazioneAngolo.BackColor = System.Drawing.Color.Transparent;
            this.txtRotazioneAngolo.Location = new System.Drawing.Point(44, 15);
            this.txtRotazioneAngolo.Name = "txtRotazioneAngolo";
            this.txtRotazioneAngolo.Size = new System.Drawing.Size(121, 32);
            this.txtRotazioneAngolo.TabIndex = 48;
            this.txtRotazioneAngolo.TEXT = "0,00";
            this.txtRotazioneAngolo.VALUE = 0;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(5, 17);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(40, 13);
            this.label21.TabIndex = 46;
            this.label21.Text = "Angolo";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(168, 16);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(39, 16);
            this.label22.TabIndex = 47;
            this.label22.Text = "gradi";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.BackColor = System.Drawing.Color.Transparent;
            this.checkBox4.Location = new System.Drawing.Point(84, 150);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(108, 17);
            this.checkBox4.TabIndex = 43;
            this.checkBox4.Text = "Posizione relativa";
            this.checkBox4.UseVisualStyleBackColor = false;
            // 
            // btnApplicaRotazione
            // 
            this.btnApplicaRotazione.Location = new System.Drawing.Point(25, 178);
            this.btnApplicaRotazione.Name = "btnApplicaRotazione";
            this.btnApplicaRotazione.Size = new System.Drawing.Size(248, 23);
            this.btnApplicaRotazione.TabIndex = 40;
            this.btnApplicaRotazione.Text = "Applica";
            this.btnApplicaRotazione.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(24, 54);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 16);
            this.label17.TabIndex = 36;
            this.label17.Text = "X";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(168, 86);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(30, 16);
            this.label18.TabIndex = 39;
            this.label18.Text = "mm";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(25, 86);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 16);
            this.label19.TabIndex = 37;
            this.label19.Text = "Y";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(168, 54);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(30, 16);
            this.label20.TabIndex = 38;
            this.label20.Text = "mm";
            // 
            // pozRotazione
            // 
            this.pozRotazione.BackColor = System.Drawing.Color.Transparent;
            this.pozRotazione.Location = new System.Drawing.Point(21, 111);
            this.pozRotazione.Name = "pozRotazione";
            this.pozRotazione.Size = new System.Drawing.Size(58, 60);
            this.pozRotazione.TabIndex = 42;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Location = new System.Drawing.Point(883, 254);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(303, 260);
            this.tabControl2.TabIndex = 35;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage5.Controls.Add(this.cbOrdinaXColore);
            this.tabPage5.Controls.Add(this.txtOtimizzaXDistanza);
            this.tabPage5.Controls.Add(this.txtImpostazioniZPerPasso);
            this.tabPage5.Controls.Add(this.txtImpostazioniNumeroPassi);
            this.tabPage5.Controls.Add(this.txtImpostazioniZLavoro);
            this.tabPage5.Controls.Add(this.txtImpostazioniZSicurezza);
            this.tabPage5.Controls.Add(this.label36);
            this.tabPage5.Controls.Add(this.cbOtimizzaXDistanza);
            this.tabPage5.Controls.Add(this.rbScriviNumeroRigha);
            this.tabPage5.Controls.Add(this.label29);
            this.tabPage5.Controls.Add(this.label30);
            this.tabPage5.Controls.Add(this.label28);
            this.tabPage5.Controls.Add(this.label26);
            this.tabPage5.Controls.Add(this.label27);
            this.tabPage5.Controls.Add(this.label24);
            this.tabPage5.Controls.Add(this.label25);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(295, 234);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Impostazioni";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtOtimizzaXDistanza
            // 
            this.txtOtimizzaXDistanza.BackColor = System.Drawing.Color.Transparent;
            this.txtOtimizzaXDistanza.Enabled = false;
            this.txtOtimizzaXDistanza.Location = new System.Drawing.Point(137, 155);
            this.txtOtimizzaXDistanza.Name = "txtOtimizzaXDistanza";
            this.txtOtimizzaXDistanza.Size = new System.Drawing.Size(121, 32);
            this.txtOtimizzaXDistanza.TabIndex = 58;
            this.txtOtimizzaXDistanza.TEXT = "0,00";
            this.txtOtimizzaXDistanza.VALUE = 0;
            // 
            // txtImpostazioniZPerPasso
            // 
            this.txtImpostazioniZPerPasso.BackColor = System.Drawing.Color.Transparent;
            this.txtImpostazioniZPerPasso.Location = new System.Drawing.Point(135, 94);
            this.txtImpostazioniZPerPasso.Name = "txtImpostazioniZPerPasso";
            this.txtImpostazioniZPerPasso.Size = new System.Drawing.Size(121, 32);
            this.txtImpostazioniZPerPasso.TabIndex = 57;
            this.txtImpostazioniZPerPasso.TEXT = "-1,00";
            this.txtImpostazioniZPerPasso.VALUE = -1;
            // 
            // txtImpostazioniNumeroPassi
            // 
            this.txtImpostazioniNumeroPassi.BackColor = System.Drawing.Color.Transparent;
            this.txtImpostazioniNumeroPassi.Location = new System.Drawing.Point(135, 65);
            this.txtImpostazioniNumeroPassi.Name = "txtImpostazioniNumeroPassi";
            this.txtImpostazioniNumeroPassi.Size = new System.Drawing.Size(121, 32);
            this.txtImpostazioniNumeroPassi.TabIndex = 56;
            this.txtImpostazioniNumeroPassi.TEXT = "1,00";
            this.txtImpostazioniNumeroPassi.VALUE = 1;
            this.txtImpostazioniNumeroPassi.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtImpostazioniNumeroPassi_inputBoxChange);
            // 
            // txtImpostazioniZLavoro
            // 
            this.txtImpostazioniZLavoro.BackColor = System.Drawing.Color.Transparent;
            this.txtImpostazioniZLavoro.Location = new System.Drawing.Point(135, 35);
            this.txtImpostazioniZLavoro.Name = "txtImpostazioniZLavoro";
            this.txtImpostazioniZLavoro.Size = new System.Drawing.Size(121, 32);
            this.txtImpostazioniZLavoro.TabIndex = 55;
            this.txtImpostazioniZLavoro.TEXT = "-1,00";
            this.txtImpostazioniZLavoro.VALUE = -1;
            this.txtImpostazioniZLavoro.inputBoxChange += new InputBox.InputBox.inputBoxChangeHandler(this.txtImpostazioniZLavoro_inputBoxChange);
            // 
            // txtImpostazioniZSicurezza
            // 
            this.txtImpostazioniZSicurezza.BackColor = System.Drawing.Color.Transparent;
            this.txtImpostazioniZSicurezza.Location = new System.Drawing.Point(135, 6);
            this.txtImpostazioniZSicurezza.Name = "txtImpostazioniZSicurezza";
            this.txtImpostazioniZSicurezza.Size = new System.Drawing.Size(121, 32);
            this.txtImpostazioniZSicurezza.TabIndex = 54;
            this.txtImpostazioniZSicurezza.TEXT = "10,00";
            this.txtImpostazioniZSicurezza.VALUE = 10;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(262, 161);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(30, 16);
            this.label36.TabIndex = 53;
            this.label36.Text = "mm";
            // 
            // cbOtimizzaXDistanza
            // 
            this.cbOtimizzaXDistanza.AutoSize = true;
            this.cbOtimizzaXDistanza.BackColor = System.Drawing.Color.Transparent;
            this.cbOtimizzaXDistanza.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbOtimizzaXDistanza.Location = new System.Drawing.Point(13, 163);
            this.cbOtimizzaXDistanza.Name = "cbOtimizzaXDistanza";
            this.cbOtimizzaXDistanza.Size = new System.Drawing.Size(118, 17);
            this.cbOtimizzaXDistanza.TabIndex = 52;
            this.cbOtimizzaXDistanza.Text = "Otimizza x distanza:";
            this.cbOtimizzaXDistanza.UseVisualStyleBackColor = false;
            this.cbOtimizzaXDistanza.CheckedChanged += new System.EventHandler(this.cbOtimizzaXDistanza_CheckedChanged);
            // 
            // rbScriviNumeroRigha
            // 
            this.rbScriviNumeroRigha.AutoSize = true;
            this.rbScriviNumeroRigha.BackColor = System.Drawing.Color.Transparent;
            this.rbScriviNumeroRigha.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbScriviNumeroRigha.Location = new System.Drawing.Point(13, 132);
            this.rbScriviNumeroRigha.Name = "rbScriviNumeroRigha";
            this.rbScriviNumeroRigha.Size = new System.Drawing.Size(140, 17);
            this.rbScriviNumeroRigha.TabIndex = 51;
            this.rbScriviNumeroRigha.Text = "Scrivi numero righa:       ";
            this.rbScriviNumeroRigha.UseVisualStyleBackColor = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(13, 106);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(119, 13);
            this.label29.TabIndex = 45;
            this.label29.Text = "Profondità max x passo:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(262, 104);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(30, 16);
            this.label30.TabIndex = 44;
            this.label30.Text = "mm";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(13, 75);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(122, 13);
            this.label28.TabIndex = 41;
            this.label28.Text = "No passi per Z di lavoro:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(13, 45);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(116, 13);
            this.label26.TabIndex = 38;
            this.label26.Text = "Z di lavoro (profondità):";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(262, 45);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(30, 16);
            this.label27.TabIndex = 39;
            this.label27.Text = "mm";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(13, 15);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(75, 13);
            this.label24.TabIndex = 35;
            this.label24.Text = "Z di sicurezza:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(262, 15);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(30, 16);
            this.label25.TabIndex = 36;
            this.label25.Text = "mm";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.label32);
            this.tabPage6.Controls.Add(this.label31);
            this.tabPage6.Controls.Add(this.rtbFine);
            this.tabPage6.Controls.Add(this.rtbInizio);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(295, 234);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Testa/Coda";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(158, 8);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(46, 13);
            this.label32.TabIndex = 41;
            this.label32.Text = "Fine file:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(10, 8);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(50, 13);
            this.label31.TabIndex = 40;
            this.label31.Text = "Inizio file:";
            // 
            // rtbFine
            // 
            this.rtbFine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rtbFine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbFine.Location = new System.Drawing.Point(161, 27);
            this.rtbFine.Name = "rtbFine";
            this.rtbFine.Size = new System.Drawing.Size(122, 155);
            this.rtbFine.TabIndex = 39;
            this.rtbFine.Text = "G00 Z50.00\nG00 X0.00 Y0.00\nM02";
            // 
            // rtbInizio
            // 
            this.rtbInizio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rtbInizio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInizio.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbInizio.Location = new System.Drawing.Point(12, 27);
            this.rtbInizio.Name = "rtbInizio";
            this.rtbInizio.Size = new System.Drawing.Size(122, 155);
            this.rtbInizio.TabIndex = 38;
            this.rtbInizio.Text = "G21\nG00 Z50.00\n";
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.cbColore8);
            this.tabPage7.Controls.Add(this.cbColore7);
            this.tabPage7.Controls.Add(this.cbColore6);
            this.tabPage7.Controls.Add(this.cbColore5);
            this.tabPage7.Controls.Add(this.cbColore4);
            this.tabPage7.Controls.Add(this.cbColore3);
            this.tabPage7.Controls.Add(this.cbColore2);
            this.tabPage7.Controls.Add(this.cbColore1);
            this.tabPage7.Controls.Add(this.label54);
            this.tabPage7.Controls.Add(this.label53);
            this.tabPage7.Controls.Add(this.ibColore8);
            this.tabPage7.Controls.Add(this.ibColore7);
            this.tabPage7.Controls.Add(this.ibColore6);
            this.tabPage7.Controls.Add(this.ibColore5);
            this.tabPage7.Controls.Add(this.ibColore4);
            this.tabPage7.Controls.Add(this.ibColore3);
            this.tabPage7.Controls.Add(this.ibColore2);
            this.tabPage7.Controls.Add(this.ibColore1);
            this.tabPage7.Controls.Add(this.cbTool8);
            this.tabPage7.Controls.Add(this.cbTool7);
            this.tabPage7.Controls.Add(this.cbTool6);
            this.tabPage7.Controls.Add(this.cbTool5);
            this.tabPage7.Controls.Add(this.cbTool4);
            this.tabPage7.Controls.Add(this.cbTool3);
            this.tabPage7.Controls.Add(this.cbTool2);
            this.tabPage7.Controls.Add(this.cbTool1);
            this.tabPage7.Controls.Add(this.label52);
            this.tabPage7.Controls.Add(this.label51);
            this.tabPage7.Controls.Add(this.label50);
            this.tabPage7.Controls.Add(this.label49);
            this.tabPage7.Controls.Add(this.label48);
            this.tabPage7.Controls.Add(this.label47);
            this.tabPage7.Controls.Add(this.label46);
            this.tabPage7.Controls.Add(this.label37);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(295, 234);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Colore";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // cbColore8
            // 
            this.cbColore8.AutoSize = true;
            this.cbColore8.Checked = true;
            this.cbColore8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore8.Location = new System.Drawing.Point(10, 205);
            this.cbColore8.Name = "cbColore8";
            this.cbColore8.Size = new System.Drawing.Size(74, 17);
            this.cbColore8.TabIndex = 83;
            this.cbColore8.Text = "8 Marrone";
            this.cbColore8.UseVisualStyleBackColor = true;
            this.cbColore8.CheckedChanged += new System.EventHandler(this.cbColore8_CheckedChanged);
            // 
            // cbColore7
            // 
            this.cbColore7.AutoSize = true;
            this.cbColore7.Checked = true;
            this.cbColore7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore7.Location = new System.Drawing.Point(10, 180);
            this.cbColore7.Name = "cbColore7";
            this.cbColore7.Size = new System.Drawing.Size(62, 17);
            this.cbColore7.TabIndex = 82;
            this.cbColore7.Text = "7 Ciano";
            this.cbColore7.UseVisualStyleBackColor = true;
            this.cbColore7.CheckedChanged += new System.EventHandler(this.cbColore7_CheckedChanged);
            // 
            // cbColore6
            // 
            this.cbColore6.AutoSize = true;
            this.cbColore6.Checked = true;
            this.cbColore6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore6.Location = new System.Drawing.Point(10, 155);
            this.cbColore6.Name = "cbColore6";
            this.cbColore6.Size = new System.Drawing.Size(61, 17);
            this.cbColore6.TabIndex = 81;
            this.cbColore6.Text = "6 Giallo";
            this.cbColore6.UseVisualStyleBackColor = true;
            this.cbColore6.CheckedChanged += new System.EventHandler(this.cbColore6_CheckedChanged);
            // 
            // cbColore5
            // 
            this.cbColore5.AutoSize = true;
            this.cbColore5.Checked = true;
            this.cbColore5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore5.Location = new System.Drawing.Point(10, 130);
            this.cbColore5.Name = "cbColore5";
            this.cbColore5.Size = new System.Drawing.Size(77, 17);
            this.cbColore5.TabIndex = 80;
            this.cbColore5.Text = "5 Magenta";
            this.cbColore5.UseVisualStyleBackColor = true;
            this.cbColore5.CheckedChanged += new System.EventHandler(this.cbColore5_CheckedChanged);
            // 
            // cbColore4
            // 
            this.cbColore4.AutoSize = true;
            this.cbColore4.Checked = true;
            this.cbColore4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore4.Location = new System.Drawing.Point(10, 105);
            this.cbColore4.Name = "cbColore4";
            this.cbColore4.Size = new System.Drawing.Size(63, 17);
            this.cbColore4.TabIndex = 79;
            this.cbColore4.Text = "4 Verde";
            this.cbColore4.UseVisualStyleBackColor = true;
            this.cbColore4.CheckedChanged += new System.EventHandler(this.cbColore4_CheckedChanged);
            // 
            // cbColore3
            // 
            this.cbColore3.AutoSize = true;
            this.cbColore3.Checked = true;
            this.cbColore3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore3.Location = new System.Drawing.Point(10, 80);
            this.cbColore3.Name = "cbColore3";
            this.cbColore3.Size = new System.Drawing.Size(65, 17);
            this.cbColore3.TabIndex = 78;
            this.cbColore3.Text = "3 Rosso";
            this.cbColore3.UseVisualStyleBackColor = true;
            this.cbColore3.CheckedChanged += new System.EventHandler(this.cbColore3_CheckedChanged);
            // 
            // cbColore2
            // 
            this.cbColore2.AutoSize = true;
            this.cbColore2.Checked = true;
            this.cbColore2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore2.Location = new System.Drawing.Point(10, 55);
            this.cbColore2.Name = "cbColore2";
            this.cbColore2.Size = new System.Drawing.Size(50, 17);
            this.cbColore2.TabIndex = 77;
            this.cbColore2.Text = "2 Blu";
            this.cbColore2.UseVisualStyleBackColor = true;
            this.cbColore2.CheckedChanged += new System.EventHandler(this.cbColore2_CheckedChanged);
            // 
            // cbColore1
            // 
            this.cbColore1.AutoSize = true;
            this.cbColore1.Checked = true;
            this.cbColore1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColore1.Location = new System.Drawing.Point(10, 30);
            this.cbColore1.Name = "cbColore1";
            this.cbColore1.Size = new System.Drawing.Size(58, 17);
            this.cbColore1.TabIndex = 36;
            this.cbColore1.Text = "1 Nero";
            this.cbColore1.UseVisualStyleBackColor = true;
            this.cbColore1.CheckedChanged += new System.EventHandler(this.cbColore1_CheckedChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.Location = new System.Drawing.Point(216, 9);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(48, 13);
            this.label54.TabIndex = 76;
            this.label54.Text = "Utensile:";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.BackColor = System.Drawing.Color.Transparent;
            this.label53.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label53.Location = new System.Drawing.Point(86, 8);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(58, 13);
            this.label53.TabIndex = 75;
            this.label53.Text = "Profondità:";
            // 
            // ibColore8
            // 
            this.ibColore8.BackColor = System.Drawing.Color.Transparent;
            this.ibColore8.Location = new System.Drawing.Point(86, 200);
            this.ibColore8.Name = "ibColore8";
            this.ibColore8.Size = new System.Drawing.Size(83, 28);
            this.ibColore8.TabIndex = 74;
            this.ibColore8.TEXT = "0,00";
            this.ibColore8.VALUE = 0;
            // 
            // ibColore7
            // 
            this.ibColore7.BackColor = System.Drawing.Color.Transparent;
            this.ibColore7.Location = new System.Drawing.Point(86, 175);
            this.ibColore7.Name = "ibColore7";
            this.ibColore7.Size = new System.Drawing.Size(83, 28);
            this.ibColore7.TabIndex = 73;
            this.ibColore7.TEXT = "0,00";
            this.ibColore7.VALUE = 0;
            // 
            // ibColore6
            // 
            this.ibColore6.BackColor = System.Drawing.Color.Transparent;
            this.ibColore6.Location = new System.Drawing.Point(86, 150);
            this.ibColore6.Name = "ibColore6";
            this.ibColore6.Size = new System.Drawing.Size(83, 28);
            this.ibColore6.TabIndex = 72;
            this.ibColore6.TEXT = "0,00";
            this.ibColore6.VALUE = 0;
            // 
            // ibColore5
            // 
            this.ibColore5.BackColor = System.Drawing.Color.Transparent;
            this.ibColore5.Location = new System.Drawing.Point(86, 125);
            this.ibColore5.Name = "ibColore5";
            this.ibColore5.Size = new System.Drawing.Size(83, 28);
            this.ibColore5.TabIndex = 71;
            this.ibColore5.TEXT = "0,00";
            this.ibColore5.VALUE = 0;
            // 
            // ibColore4
            // 
            this.ibColore4.BackColor = System.Drawing.Color.Transparent;
            this.ibColore4.Location = new System.Drawing.Point(86, 100);
            this.ibColore4.Name = "ibColore4";
            this.ibColore4.Size = new System.Drawing.Size(83, 28);
            this.ibColore4.TabIndex = 70;
            this.ibColore4.TEXT = "0,00";
            this.ibColore4.VALUE = 0;
            // 
            // ibColore3
            // 
            this.ibColore3.BackColor = System.Drawing.Color.Transparent;
            this.ibColore3.Location = new System.Drawing.Point(86, 75);
            this.ibColore3.Name = "ibColore3";
            this.ibColore3.Size = new System.Drawing.Size(83, 28);
            this.ibColore3.TabIndex = 69;
            this.ibColore3.TEXT = "0,00";
            this.ibColore3.VALUE = 0;
            // 
            // ibColore2
            // 
            this.ibColore2.BackColor = System.Drawing.Color.Transparent;
            this.ibColore2.Location = new System.Drawing.Point(86, 50);
            this.ibColore2.Name = "ibColore2";
            this.ibColore2.Size = new System.Drawing.Size(83, 28);
            this.ibColore2.TabIndex = 68;
            this.ibColore2.TEXT = "0,00";
            this.ibColore2.VALUE = 0;
            // 
            // ibColore1
            // 
            this.ibColore1.BackColor = System.Drawing.Color.Transparent;
            this.ibColore1.Location = new System.Drawing.Point(86, 25);
            this.ibColore1.Name = "ibColore1";
            this.ibColore1.Size = new System.Drawing.Size(83, 28);
            this.ibColore1.TabIndex = 40;
            this.ibColore1.TEXT = "0,00";
            this.ibColore1.VALUE = 0;
            // 
            // cbTool8
            // 
            this.cbTool8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool8.FormattingEnabled = true;
            this.cbTool8.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool8.Location = new System.Drawing.Point(219, 203);
            this.cbTool8.Name = "cbTool8";
            this.cbTool8.Size = new System.Drawing.Size(60, 21);
            this.cbTool8.TabIndex = 67;
            this.cbTool8.Text = "---";
            // 
            // cbTool7
            // 
            this.cbTool7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool7.FormattingEnabled = true;
            this.cbTool7.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool7.Location = new System.Drawing.Point(219, 178);
            this.cbTool7.Name = "cbTool7";
            this.cbTool7.Size = new System.Drawing.Size(60, 21);
            this.cbTool7.TabIndex = 66;
            this.cbTool7.Text = "---";
            // 
            // cbTool6
            // 
            this.cbTool6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool6.FormattingEnabled = true;
            this.cbTool6.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool6.Location = new System.Drawing.Point(219, 153);
            this.cbTool6.Name = "cbTool6";
            this.cbTool6.Size = new System.Drawing.Size(60, 21);
            this.cbTool6.TabIndex = 65;
            this.cbTool6.Text = "---";
            // 
            // cbTool5
            // 
            this.cbTool5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool5.FormattingEnabled = true;
            this.cbTool5.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool5.Location = new System.Drawing.Point(219, 128);
            this.cbTool5.Name = "cbTool5";
            this.cbTool5.Size = new System.Drawing.Size(60, 21);
            this.cbTool5.TabIndex = 64;
            this.cbTool5.Text = "---";
            // 
            // cbTool4
            // 
            this.cbTool4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool4.FormattingEnabled = true;
            this.cbTool4.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool4.Location = new System.Drawing.Point(219, 103);
            this.cbTool4.Name = "cbTool4";
            this.cbTool4.Size = new System.Drawing.Size(60, 21);
            this.cbTool4.TabIndex = 63;
            this.cbTool4.Text = "---";
            // 
            // cbTool3
            // 
            this.cbTool3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool3.FormattingEnabled = true;
            this.cbTool3.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool3.Location = new System.Drawing.Point(219, 78);
            this.cbTool3.Name = "cbTool3";
            this.cbTool3.Size = new System.Drawing.Size(60, 21);
            this.cbTool3.TabIndex = 62;
            this.cbTool3.Text = "---";
            // 
            // cbTool2
            // 
            this.cbTool2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool2.FormattingEnabled = true;
            this.cbTool2.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool2.Location = new System.Drawing.Point(219, 53);
            this.cbTool2.Name = "cbTool2";
            this.cbTool2.Size = new System.Drawing.Size(60, 21);
            this.cbTool2.TabIndex = 61;
            this.cbTool2.Text = "---";
            // 
            // cbTool1
            // 
            this.cbTool1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTool1.FormattingEnabled = true;
            this.cbTool1.Items.AddRange(new object[] {
            "---",
            "T1",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7",
            "T8"});
            this.cbTool1.Location = new System.Drawing.Point(219, 28);
            this.cbTool1.Name = "cbTool1";
            this.cbTool1.Size = new System.Drawing.Size(60, 21);
            this.cbTool1.TabIndex = 60;
            this.cbTool1.Text = "---";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.BackColor = System.Drawing.Color.Transparent;
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.Location = new System.Drawing.Point(168, 207);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(30, 16);
            this.label52.TabIndex = 59;
            this.label52.Text = "mm";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.BackColor = System.Drawing.Color.Transparent;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.Location = new System.Drawing.Point(168, 182);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(30, 16);
            this.label51.TabIndex = 58;
            this.label51.Text = "mm";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.BackColor = System.Drawing.Color.Transparent;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(168, 157);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(30, 16);
            this.label50.TabIndex = 54;
            this.label50.Text = "mm";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.BackColor = System.Drawing.Color.Transparent;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.Location = new System.Drawing.Point(168, 132);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(30, 16);
            this.label49.TabIndex = 52;
            this.label49.Text = "mm";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(168, 107);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(30, 16);
            this.label48.TabIndex = 50;
            this.label48.Text = "mm";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.BackColor = System.Drawing.Color.Transparent;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.Location = new System.Drawing.Point(168, 82);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(30, 16);
            this.label47.TabIndex = 48;
            this.label47.Text = "mm";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.Color.Transparent;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(168, 57);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(30, 16);
            this.label46.TabIndex = 46;
            this.label46.Text = "mm";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(168, 32);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(30, 16);
            this.label37.TabIndex = 37;
            this.label37.Text = "mm";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.ibST8);
            this.tabPage8.Controls.Add(this.ibFT8);
            this.tabPage8.Controls.Add(this.ibMaxT8);
            this.tabPage8.Controls.Add(this.ibST7);
            this.tabPage8.Controls.Add(this.ibFT7);
            this.tabPage8.Controls.Add(this.ibMaxT7);
            this.tabPage8.Controls.Add(this.ibST6);
            this.tabPage8.Controls.Add(this.ibFT6);
            this.tabPage8.Controls.Add(this.ibMaxT6);
            this.tabPage8.Controls.Add(this.ibST5);
            this.tabPage8.Controls.Add(this.ibFT5);
            this.tabPage8.Controls.Add(this.ibMaxT5);
            this.tabPage8.Controls.Add(this.ibST4);
            this.tabPage8.Controls.Add(this.ibFT4);
            this.tabPage8.Controls.Add(this.ibMaxT4);
            this.tabPage8.Controls.Add(this.ibST3);
            this.tabPage8.Controls.Add(this.ibFT3);
            this.tabPage8.Controls.Add(this.ibMaxT3);
            this.tabPage8.Controls.Add(this.ibST2);
            this.tabPage8.Controls.Add(this.ibFT2);
            this.tabPage8.Controls.Add(this.ibMaxT2);
            this.tabPage8.Controls.Add(this.label57);
            this.tabPage8.Controls.Add(this.ibST1);
            this.tabPage8.Controls.Add(this.label56);
            this.tabPage8.Controls.Add(this.ibFT1);
            this.tabPage8.Controls.Add(this.label55);
            this.tabPage8.Controls.Add(this.ibMaxT1);
            this.tabPage8.Controls.Add(this.label45);
            this.tabPage8.Controls.Add(this.label44);
            this.tabPage8.Controls.Add(this.label43);
            this.tabPage8.Controls.Add(this.label42);
            this.tabPage8.Controls.Add(this.label41);
            this.tabPage8.Controls.Add(this.label40);
            this.tabPage8.Controls.Add(this.label39);
            this.tabPage8.Controls.Add(this.label38);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(295, 234);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "Utensili";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // ibST8
            // 
            this.ibST8.BackColor = System.Drawing.Color.Transparent;
            this.ibST8.Location = new System.Drawing.Point(204, 201);
            this.ibST8.Name = "ibST8";
            this.ibST8.Size = new System.Drawing.Size(83, 28);
            this.ibST8.TabIndex = 34;
            this.ibST8.TEXT = "0,00";
            this.ibST8.VALUE = 0;
            // 
            // ibFT8
            // 
            this.ibFT8.BackColor = System.Drawing.Color.Transparent;
            this.ibFT8.Location = new System.Drawing.Point(117, 201);
            this.ibFT8.Name = "ibFT8";
            this.ibFT8.Size = new System.Drawing.Size(83, 28);
            this.ibFT8.TabIndex = 33;
            this.ibFT8.TEXT = "0,00";
            this.ibFT8.VALUE = 0;
            // 
            // ibMaxT8
            // 
            this.ibMaxT8.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT8.Location = new System.Drawing.Point(30, 201);
            this.ibMaxT8.Name = "ibMaxT8";
            this.ibMaxT8.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT8.TabIndex = 32;
            this.ibMaxT8.TEXT = "0,00";
            this.ibMaxT8.VALUE = 0;
            // 
            // ibST7
            // 
            this.ibST7.BackColor = System.Drawing.Color.Transparent;
            this.ibST7.Location = new System.Drawing.Point(204, 176);
            this.ibST7.Name = "ibST7";
            this.ibST7.Size = new System.Drawing.Size(83, 28);
            this.ibST7.TabIndex = 31;
            this.ibST7.TEXT = "0,00";
            this.ibST7.VALUE = 0;
            // 
            // ibFT7
            // 
            this.ibFT7.BackColor = System.Drawing.Color.Transparent;
            this.ibFT7.Location = new System.Drawing.Point(117, 176);
            this.ibFT7.Name = "ibFT7";
            this.ibFT7.Size = new System.Drawing.Size(83, 28);
            this.ibFT7.TabIndex = 30;
            this.ibFT7.TEXT = "0,00";
            this.ibFT7.VALUE = 0;
            // 
            // ibMaxT7
            // 
            this.ibMaxT7.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT7.Location = new System.Drawing.Point(30, 176);
            this.ibMaxT7.Name = "ibMaxT7";
            this.ibMaxT7.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT7.TabIndex = 29;
            this.ibMaxT7.TEXT = "0,00";
            this.ibMaxT7.VALUE = 0;
            // 
            // ibST6
            // 
            this.ibST6.BackColor = System.Drawing.Color.Transparent;
            this.ibST6.Location = new System.Drawing.Point(204, 151);
            this.ibST6.Name = "ibST6";
            this.ibST6.Size = new System.Drawing.Size(83, 28);
            this.ibST6.TabIndex = 28;
            this.ibST6.TEXT = "0,00";
            this.ibST6.VALUE = 0;
            // 
            // ibFT6
            // 
            this.ibFT6.BackColor = System.Drawing.Color.Transparent;
            this.ibFT6.Location = new System.Drawing.Point(117, 151);
            this.ibFT6.Name = "ibFT6";
            this.ibFT6.Size = new System.Drawing.Size(83, 28);
            this.ibFT6.TabIndex = 27;
            this.ibFT6.TEXT = "0,00";
            this.ibFT6.VALUE = 0;
            // 
            // ibMaxT6
            // 
            this.ibMaxT6.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT6.Location = new System.Drawing.Point(30, 151);
            this.ibMaxT6.Name = "ibMaxT6";
            this.ibMaxT6.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT6.TabIndex = 26;
            this.ibMaxT6.TEXT = "0,00";
            this.ibMaxT6.VALUE = 0;
            // 
            // ibST5
            // 
            this.ibST5.BackColor = System.Drawing.Color.Transparent;
            this.ibST5.Location = new System.Drawing.Point(204, 126);
            this.ibST5.Name = "ibST5";
            this.ibST5.Size = new System.Drawing.Size(83, 28);
            this.ibST5.TabIndex = 25;
            this.ibST5.TEXT = "0,00";
            this.ibST5.VALUE = 0;
            // 
            // ibFT5
            // 
            this.ibFT5.BackColor = System.Drawing.Color.Transparent;
            this.ibFT5.Location = new System.Drawing.Point(117, 126);
            this.ibFT5.Name = "ibFT5";
            this.ibFT5.Size = new System.Drawing.Size(83, 28);
            this.ibFT5.TabIndex = 24;
            this.ibFT5.TEXT = "0,00";
            this.ibFT5.VALUE = 0;
            // 
            // ibMaxT5
            // 
            this.ibMaxT5.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT5.Location = new System.Drawing.Point(30, 126);
            this.ibMaxT5.Name = "ibMaxT5";
            this.ibMaxT5.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT5.TabIndex = 23;
            this.ibMaxT5.TEXT = "0,00";
            this.ibMaxT5.VALUE = 0;
            // 
            // ibST4
            // 
            this.ibST4.BackColor = System.Drawing.Color.Transparent;
            this.ibST4.Location = new System.Drawing.Point(204, 101);
            this.ibST4.Name = "ibST4";
            this.ibST4.Size = new System.Drawing.Size(83, 28);
            this.ibST4.TabIndex = 22;
            this.ibST4.TEXT = "0,00";
            this.ibST4.VALUE = 0;
            // 
            // ibFT4
            // 
            this.ibFT4.BackColor = System.Drawing.Color.Transparent;
            this.ibFT4.Location = new System.Drawing.Point(117, 101);
            this.ibFT4.Name = "ibFT4";
            this.ibFT4.Size = new System.Drawing.Size(83, 28);
            this.ibFT4.TabIndex = 21;
            this.ibFT4.TEXT = "0,00";
            this.ibFT4.VALUE = 0;
            // 
            // ibMaxT4
            // 
            this.ibMaxT4.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT4.Location = new System.Drawing.Point(30, 101);
            this.ibMaxT4.Name = "ibMaxT4";
            this.ibMaxT4.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT4.TabIndex = 20;
            this.ibMaxT4.TEXT = "0,00";
            this.ibMaxT4.VALUE = 0;
            // 
            // ibST3
            // 
            this.ibST3.BackColor = System.Drawing.Color.Transparent;
            this.ibST3.Location = new System.Drawing.Point(204, 76);
            this.ibST3.Name = "ibST3";
            this.ibST3.Size = new System.Drawing.Size(83, 28);
            this.ibST3.TabIndex = 19;
            this.ibST3.TEXT = "0,00";
            this.ibST3.VALUE = 0;
            // 
            // ibFT3
            // 
            this.ibFT3.BackColor = System.Drawing.Color.Transparent;
            this.ibFT3.Location = new System.Drawing.Point(117, 76);
            this.ibFT3.Name = "ibFT3";
            this.ibFT3.Size = new System.Drawing.Size(83, 28);
            this.ibFT3.TabIndex = 18;
            this.ibFT3.TEXT = "0,00";
            this.ibFT3.VALUE = 0;
            // 
            // ibMaxT3
            // 
            this.ibMaxT3.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT3.Location = new System.Drawing.Point(30, 76);
            this.ibMaxT3.Name = "ibMaxT3";
            this.ibMaxT3.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT3.TabIndex = 17;
            this.ibMaxT3.TEXT = "0,00";
            this.ibMaxT3.VALUE = 0;
            // 
            // ibST2
            // 
            this.ibST2.BackColor = System.Drawing.Color.Transparent;
            this.ibST2.Location = new System.Drawing.Point(204, 51);
            this.ibST2.Name = "ibST2";
            this.ibST2.Size = new System.Drawing.Size(83, 28);
            this.ibST2.TabIndex = 16;
            this.ibST2.TEXT = "0,00";
            this.ibST2.VALUE = 0;
            // 
            // ibFT2
            // 
            this.ibFT2.BackColor = System.Drawing.Color.Transparent;
            this.ibFT2.Location = new System.Drawing.Point(117, 51);
            this.ibFT2.Name = "ibFT2";
            this.ibFT2.Size = new System.Drawing.Size(83, 28);
            this.ibFT2.TabIndex = 15;
            this.ibFT2.TEXT = "0,00";
            this.ibFT2.VALUE = 0;
            // 
            // ibMaxT2
            // 
            this.ibMaxT2.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT2.Location = new System.Drawing.Point(30, 51);
            this.ibMaxT2.Name = "ibMaxT2";
            this.ibMaxT2.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT2.TabIndex = 14;
            this.ibMaxT2.TEXT = "0,00";
            this.ibMaxT2.VALUE = 0;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.Location = new System.Drawing.Point(17, 6);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(96, 13);
            this.label57.TabIndex = 13;
            this.label57.Text = "Max. prof. x passo:";
            // 
            // ibST1
            // 
            this.ibST1.BackColor = System.Drawing.Color.Transparent;
            this.ibST1.Location = new System.Drawing.Point(204, 26);
            this.ibST1.Name = "ibST1";
            this.ibST1.Size = new System.Drawing.Size(83, 28);
            this.ibST1.TabIndex = 12;
            this.ibST1.TEXT = "0,00";
            this.ibST1.VALUE = 0;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label56.Location = new System.Drawing.Point(227, 6);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(17, 13);
            this.label56.TabIndex = 11;
            this.label56.Text = "S:";
            // 
            // ibFT1
            // 
            this.ibFT1.BackColor = System.Drawing.Color.Transparent;
            this.ibFT1.Location = new System.Drawing.Point(117, 26);
            this.ibFT1.Name = "ibFT1";
            this.ibFT1.Size = new System.Drawing.Size(83, 28);
            this.ibFT1.TabIndex = 10;
            this.ibFT1.TEXT = "0,00";
            this.ibFT1.VALUE = 0;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.Location = new System.Drawing.Point(140, 6);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(16, 13);
            this.label55.TabIndex = 9;
            this.label55.Text = "F:";
            // 
            // ibMaxT1
            // 
            this.ibMaxT1.BackColor = System.Drawing.Color.Transparent;
            this.ibMaxT1.Location = new System.Drawing.Point(30, 26);
            this.ibMaxT1.Name = "ibMaxT1";
            this.ibMaxT1.Size = new System.Drawing.Size(83, 28);
            this.ibMaxT1.TabIndex = 8;
            this.ibMaxT1.TEXT = "0,00";
            this.ibMaxT1.VALUE = 0;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(3, 184);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(22, 13);
            this.label45.TabIndex = 7;
            this.label45.Text = "T7";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(3, 209);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(22, 13);
            this.label44.TabIndex = 6;
            this.label44.Text = "T8";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(3, 159);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(22, 13);
            this.label43.TabIndex = 5;
            this.label43.Text = "T6";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(3, 134);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(22, 13);
            this.label42.TabIndex = 4;
            this.label42.Text = "T5";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(3, 109);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(22, 13);
            this.label41.TabIndex = 3;
            this.label41.Text = "T4";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(3, 84);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(22, 13);
            this.label40.TabIndex = 2;
            this.label40.Text = "T3";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(3, 59);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(22, 13);
            this.label39.TabIndex = 1;
            this.label39.Text = "T2";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(3, 34);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(22, 13);
            this.label38.TabIndex = 0;
            this.label38.Text = "T1";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.txtNoLineeGCode);
            this.groupBox1.Controls.Add(this.txtYminmax);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtXminmax);
            this.groupBox1.Controls.Add(this.txtUkupnaDuzina);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtNumeroTotaleLinee);
            this.groupBox1.Location = new System.Drawing.Point(558, 392);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 121);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informazioni";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label35.Location = new System.Drawing.Point(6, 92);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(82, 13);
            this.label35.TabIndex = 20;
            this.label35.Text = "N. linee GCode:";
            // 
            // txtNoLineeGCode
            // 
            this.txtNoLineeGCode.AutoSize = true;
            this.txtNoLineeGCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoLineeGCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtNoLineeGCode.Location = new System.Drawing.Point(113, 92);
            this.txtNoLineeGCode.Name = "txtNoLineeGCode";
            this.txtNoLineeGCode.Size = new System.Drawing.Size(13, 13);
            this.txtNoLineeGCode.TabIndex = 21;
            this.txtNoLineeGCode.Text = "0";
            // 
            // rtbComands
            // 
            this.rtbComands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbComands.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbComands.Location = new System.Drawing.Point(558, 291);
            this.rtbComands.Name = "rtbComands";
            this.rtbComands.Size = new System.Drawing.Size(132, 88);
            this.rtbComands.TabIndex = 37;
            this.rtbComands.Text = "";
            // 
            // rtbHPGLHiden
            // 
            this.rtbHPGLHiden.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHPGLHiden.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHPGLHiden.Location = new System.Drawing.Point(558, 13);
            this.rtbHPGLHiden.Name = "rtbHPGLHiden";
            this.rtbHPGLHiden.Size = new System.Drawing.Size(132, 272);
            this.rtbHPGLHiden.TabIndex = 38;
            this.rtbHPGLHiden.Text = "";
            this.rtbHPGLHiden.Visible = false;
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.Enabled = false;
            this.btnRegenerate.Location = new System.Drawing.Point(763, 489);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(114, 23);
            this.btnRegenerate.TabIndex = 39;
            this.btnRegenerate.Text = "Regenerate GC";
            this.btnRegenerate.UseVisualStyleBackColor = true;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // cbOrdinaXColore
            // 
            this.cbOrdinaXColore.AutoSize = true;
            this.cbOrdinaXColore.BackColor = System.Drawing.Color.Transparent;
            this.cbOrdinaXColore.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbOrdinaXColore.Location = new System.Drawing.Point(13, 197);
            this.cbOrdinaXColore.Name = "cbOrdinaXColore";
            this.cbOrdinaXColore.Size = new System.Drawing.Size(140, 17);
            this.cbOrdinaXColore.TabIndex = 59;
            this.cbOrdinaXColore.Text = "Ordina x Colore             :";
            this.cbOrdinaXColore.UseVisualStyleBackColor = false;
            // 
            // frmQuote
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1192, 524);
            this.Controls.Add(this.btnRegenerate);
            this.Controls.Add(this.rtbComands);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.btnSaveGCode);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnApri);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabTrasformazioni);
            this.Controls.Add(this.rtbGCODE);
            this.Controls.Add(this.rtbHPGL);
            this.Controls.Add(this.pnlDisegno);
            this.Controls.Add(this.rtbHPGLHiden);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 558);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 558);
            this.Name = "frmQuote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hpgl2GCode         by SD 2008";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmQuote_FormClosed);
            this.pnlDisegno.ResumeLayout(false);
            this.pnlDisegno.PerformLayout();
            this.tabTrasformazioni.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Il punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();

            Application.Run(new frmQuote());
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            Graphics grfx = pnlDisegno.CreateGraphics();
            grfx.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
        }

        private int ToDecimal(string binval)
        {
            int val = 0;

            for (int f = 0; f < binval.Length; f++)
            {
                if (binval[f] == '1')
                    val = val + (int)Math.Pow(2.0, (double)(binval.Length - (f + 1)));
            }

            return val;
        }

        private double OrigDimX = 0.0;
        private double OrigDimY = 0.0;

        private void ProgramHPGL()
        {
            XMax = hpgl.XMAX;
            YMax = hpgl.YMAX;
            XMin = hpgl.XMIN;
            YMin = hpgl.YMIN;
            offsetXDraw = hpgl.OFFSETX;
            offsetYDraw = hpgl.OFFSETY;

            if (ApriPRG == true)
            {
                OrigDimX = XMax - XMin;
                OrigDimY = YMax - YMin;

                ApriPRG = false;
            }

            txtXminmax.Text = ToFloatStr(XMin) + " - " + ToFloatStr(XMax);
            txtYminmax.Text = ToFloatStr(YMin) + " - " + ToFloatStr(YMax);

            DrawingLines = (ArrayList)(hpgl.ALPROGRAMMoveTo).Clone();

            TrovaProporzioniHPGL();

            DisegnaProgram(1);

            IspisiDimenzije(1);
        }

        private int nLineGcode = 0;
        private string snLineGcode = "";
        bool ZVrti = false;

        private void DisegnaProgram(int i)
        {
            if (programHPGLExist == false) return;

            if (i == 1) rtbGCODE.Text = "";

            UkupnaDuzina = 0.0f;
            NumeroTotaleLinee = 0;

            punto.X = 0;
            punto.Y = 0;

            punto_old.X = 0;
            punto_old.Y = 0;

            //label37.Text = "0";
            //preskocene = 0;

            Graphics grfx = pnlDisegno.CreateGraphics();
            grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            grfx.Clear(System.Drawing.Color.DarkKhaki);
            grfx.Dispose();

            grfxBm.Clear(System.Drawing.Color.DarkKhaki);

            noLine = 1;

            nLineGcode = rtbGCODE.Lines.Length;

            if (nLineGcode > 0)
            {
                nLineGcode = nLineGcode.ToString().Length;
            }


            if (i == 1)
            {
                foreach (string s in rtbInizio.Lines)
                {
                    if (s.Trim() != "" && s != "\n")
                        WriteGCode(s.Trim());
                }
            }

            double zDWTmp = 0.0;

            if (cbOrdinaXColore.Checked == true)
            {
                for (int col = 1; col <= 8; col++)
                {
                    if (impo[col - 1].Colore.Checked.Checked == false) continue;

                    zDWTmp = 0.0;
                    zDW = 0.0;
                    int prvi = 0;

                    if (Math.Abs(impo[col - 1].Colore.Profondita.VALUE) > Math.Abs(impo[col - 1].Utensili.MaxProfondita.VALUE))
                    {
                        zDW = impo[col - 1].Utensili.MaxProfondita.VALUE;
                        ZVrti = true;
                    }
                    else
                    {
                        zDW = impo[col - 1].Colore.Profondita.VALUE;
                        prvi = 2;
                        ZVrti = false;
                    }
                    
                    do
                    {
                        foreach (Linea line in DrawingLines)
                        {
                            if (line.sp == col) DoProgramLineHPGL(line, i);
                        }

                        zDW += impo[col - 1].Utensili.MaxProfondita.VALUE;

                        if (Math.Abs(zDW) > Math.Abs(impo[col - 1].Colore.Profondita.VALUE))
                        {
                            zDW = impo[col - 1].Colore.Profondita.VALUE;
                            prvi++;
                        }

                    }while (Math.Abs(zDW) <= Math.Abs(impo[col - 1].Colore.Profondita.VALUE) && prvi < 2);                  
                }
            }
            else
            {
                foreach (Linea line in DrawingLines)
                {
                    DoProgramLineHPGL(line, i);
                }
            }

            grfxBm.DrawLine(new Pen(Color.Yellow), ToScreen(new PointF((float)(-2), (float)(0))), ToScreen(new PointF((float)(2), (float)(0))));
            grfxBm.DrawLine(new Pen(Color.Yellow), ToScreen(new PointF((float)(0), (float)(-2))), ToScreen(new PointF((float)(0), (float)(2))));

            //grfxBm.DrawLine(new Pen(Color.Yellow), (new PointF((float)(-20), (float)(0))), (new PointF((float)(20), (float)(0))));
            //grfxBm.DrawLine(new Pen(Color.Yellow), (new PointF((float)(0), (float)(-20))), (new PointF((float)(0), (float)(20))));

            if (i == 1 && noLine > 0)
            {
                foreach (string s in rtbFine.Lines)
                {
                    if (s.Trim() != "" && s != "\n")
                        WriteGCode(s.Trim());
                }
            }

            if (bitmapHPGL != null)
            {
                Graphics g = Graphics.FromImage(bitmapHPGL);
                g.Clear(System.Drawing.Color.DarkKhaki);
                g.Dispose();
            }

            bitmapHPGL = new Bitmap(bitmap);

            txtNoLineeGCode.Text = (rtbGCODE.Lines.Length - 1).ToString();

            Invalidate();
        }

        private void TrovaProporzioniHPGL()
        {
            double size1 = (double)(grfx.VisibleClipBounds.Width) / (XMax - XMin);
            double size2 = (double)(grfx.VisibleClipBounds.Height) / (YMax - YMin);

            if ((int)(YMax * size1) > grfx.VisibleClipBounds.Height)
                sizeDraw = size2;
            else
                sizeDraw = size1;

            sizeDraw = sizeDraw * (double)0.80;

            int sX = (XMax < 0 && XMin < 0) ? -1 : 1;
            int sY = (YMax < 0 && YMin < 0) ? -1 : 1;

            offsetXDraw = (int)((grfx.VisibleClipBounds.Width - ((XMax - XMin) * sizeDraw * sX)) / 2);
            offsetYDraw = (int)((grfx.VisibleClipBounds.Height - ((YMax - YMin) * sizeDraw * sY)) / 2);

            col = Color.Black;
        }

        private void IspisiDimenzije(int i)
        {
            if (ZoomFactor == 1.0f)
            {
                if (MoveH == true)
                {
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 - (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95)), new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 + (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95)));
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 - (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95 - 5.0)), new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 - (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95 + 5.0)));
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 + (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95 - 5.0)), new PointF((float)(grfxBm.VisibleClipBounds.Width / 2.0 + (XMax - XMin) * sizeDraw / 2.0), (float)(grfxBm.VisibleClipBounds.Height * 0.95 + 5.0)));

                    txtSirinaHPGL.Visible = true;
                    txtSirinaHPGL.Text = ToFloatStr(XMax - XMin) + "mm";

                    txtDimensioniX.TEXT = ToFloatStr(XMax - XMin);
                }
                else
                    txtSirinaHPGL.Visible = false;

                if (MoveV == true)
                {
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95), (float)(grfxBm.VisibleClipBounds.Height / 2.0 - (YMax - YMin) * sizeDraw / 2.0)), new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95), (float)(grfxBm.VisibleClipBounds.Height / 2.0 + (YMax - YMin) * sizeDraw / 2.0)));
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95 - 5.0), (float)(grfxBm.VisibleClipBounds.Height / 2.0 - (YMax - YMin) * sizeDraw / 2.0)), new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95 + 5.0), (float)(grfxBm.VisibleClipBounds.Height / 2.0 - (YMax - YMin) * sizeDraw / 2.0)));
                    grfxBm.DrawLine(pen, new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95 - 5.0), (float)(grfxBm.VisibleClipBounds.Height / 2.0 + (YMax - YMin) * sizeDraw / 2.0)), new PointF((float)(grfxBm.VisibleClipBounds.Width * 0.95 + 5.0), (float)(grfxBm.VisibleClipBounds.Height / 2.0 + (YMax - YMin) * sizeDraw / 2.0)));

                    txtVisinaHPGL.Visible = true;
                    txtVisinaHPGL.Text = ToFloatStr(YMax - YMin) + "mm";

                    txtDimensioniY.TEXT = ToFloatStr(YMax - YMin);
                }
                else
                    txtVisinaHPGL.Visible = false;
            }
            else
            {
                txtSirinaHPGL.Visible = false;
                txtVisinaHPGL.Visible = false;
            }

            if(i == 1) txtUkupnaDuzina.Text = ToFloatStr(UkupnaDuzina) + "mm";
            txtNumeroTotaleLinee.Text = NumeroTotaleLinee.ToString();
        }

        int preskocene = 0;

        private void DoProgramLineHPGL(Linea l, int i)
        {
            double x = l.x;
            double y = l.y;
            double z = l.z;
            int SP = l.sp;
            bool draw = l.draw;
            double deep = 0.0;
            string tool = "--";
            bool dozvoljeno = false;

            double F = 0.0;
            double S = 0.0;

            switch (SP)
            #region colors
            {
                case 1:
                    col = Color.Black;
                    break;
                case 2:
                    col = Color.Blue;
                    break;
                case 3:
                    col = Color.Red;
                    break;
                case 4:
                    col = Color.Green;
                    break;
                case 5:
                    col = Color.Magenta;
                    break;
                case 6:
                    col = Color.Yellow;
                    break;
                case 7:
                    col = Color.Cyan;
                    break;
                case 8:
                    col = Color.Brown;
                    break;
            }
            #endregion

            tool = impo[SP - 1].Colore.Tool.Text;
            dozvoljeno = impo[SP - 1].Colore.Checked.Checked;
            deep = zDW;

            F = impo[SP - 1].Utensili.FVelocita.VALUE;
            S = impo[SP - 1].Utensili.SVelocita.VALUE;
            double MaxDeepXPasso = impo[SP - 1].Utensili.MaxProfondita.VALUE;

            //double MaxDeepXPasso = 0.0;

            //switch (tool)
            //{
            //    case "T1":
            //        F = ibFT1.VALUE;
            //        S = ibST1.VALUE;
            //        MaxDeepXPasso = ibMaxT1.VALUE;
            //        break;
            //    case "T2":
            //        F = ibFT2.VALUE;
            //        S = ibST2.VALUE;
            //        MaxDeepXPasso = ibMaxT2.VALUE;
            //        break;
            //    case "T3":
            //        F = ibFT3.VALUE;
            //        S = ibST3.VALUE;
            //        MaxDeepXPasso = ibMaxT3.VALUE;
            //        break;
            //    case "T4":
            //        F = ibFT4.VALUE;
            //        S = ibST4.VALUE;
            //        MaxDeepXPasso = ibMaxT4.VALUE;
            //        break;
            //    case "T5":
            //        F = ibFT5.VALUE;
            //        S = ibST5.VALUE;
            //        MaxDeepXPasso = ibMaxT5.VALUE;
            //        break;
            //    case "T6":
            //        F = ibFT6.VALUE;
            //        S = ibST6.VALUE;
            //        MaxDeepXPasso = ibMaxT6.VALUE;
            //        break;
            //    case "T7":
            //        F = ibFT7.VALUE;
            //        S = ibST7.VALUE;
            //        MaxDeepXPasso = ibMaxT7.VALUE;
            //        break;
            //    case "T8":
            //        F = ibFT8.VALUE;
            //        S = ibST8.VALUE;
            //        MaxDeepXPasso = ibMaxT8.VALUE;
            //        break;
            //}

            if (dozvoljeno == false) return;

            //MessageBox.Show(deep.ToString() + "  " + tool);

            bool linijaKoristena = false;

            if (draw == true)
            {
                Pen pen = new Pen(col);

                double difX1 = punto.X - x;
                double difY1 = punto.Y - y;

                difX1 = Math.Pow((difX1), 2);
                difY1 = Math.Pow((difY1), 2);

                difX1 = Math.Abs(difX1);
                difY1 = Math.Abs(difY1);

                double duzinaTmp1 = Math.Sqrt(difX1 + difY1);

                double difX2 = punto_old.X - x;
                double difY2 = punto_old.Y - y;

                difX2 = Math.Pow((difX2), 2);
                difY2 = Math.Pow((difY2), 2);

                difX2 = Math.Abs(difX2);
                difY2 = Math.Abs(difY2);

                double duzinaTmp2 = Math.Sqrt(difX2 + difY2);

                if (duzinaTmp1 * sizeDraw * ZoomFactor >= 0.5)
                    grfxBm.DrawLine(pen, ToScreen(new PointF((float)(punto.X), (float)(punto.Y))), ToScreen(new PointF((float)(x), (float)(y))));
                else
                {
                    //preskocene++;
                    //label37.Text = preskocene.ToString();
                }

                if (cbOtimizzaXDistanza.Checked == true)
                {
                    if (i == 1)
                    {
                        if (oldDraw == false)
                        {
                            WriteGCode("G01", "X", "Y", "Z" + ToFloatStr(deep), "M", "F" + ToFloatStr(F), tool, "S" + ToFloatStr(S));  //WriteGCode("G01", "X", "Y", "Z" + ToFloatStr(zDW), "M", "F" + F, tool, "S");
                        }
                    }

                    if (duzinaTmp2 >= txtOtimizzaXDistanza.VALUE)
                    {
                        if (i == 1)
                        {
                            string sX = ToFloatStr(x);
                            string sY = ToFloatStr(y);

                            WriteGCode("G01", "X" + sX, "Y" + sY, "Z", "M", "F" + ToFloatStr(F), "---", "S");

                            punto_old.X = x;
                            punto_old.Y = y;

                            UkupnaDuzina += duzinaTmp2;
                        }
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        if (oldDraw == false)
                        {
                            WriteGCode("G01", "X", "Y", "Z" + ToFloatStr(deep), "M", "F" + ToFloatStr(F), tool, "S" + ToFloatStr(S));  //WriteGCode("G01", "X", "Y", "Z" + ToFloatStr(zDW), "M", "F" + F, tool, "S");
                        }

                        string sX = ToFloatStr(x);
                        string sY = ToFloatStr(y);

                        WriteGCode("G01", "X" + sX, "Y" + sY, "Z", "M", "F" + ToFloatStr(F), "---", "S");

                        UkupnaDuzina += duzinaTmp2;

                        punto_old.X = x;
                        punto_old.Y = y;
                    }
                }

                NumeroTotaleLinee++;
            }
            else //if (ZVrti == false)
            {
                if (i == 1)
                {
                    if (oldDraw == true)
                    {
                        WriteGCode("G00", "X", "Y", "Z" + ToFloatStr(zUP), "M", "F", "---", "S");
                    }

                    string sX = ToFloatStr(x);
                    string sY = ToFloatStr(y);

                    WriteGCode("G00", "X" + sX, "Y" + sY, "Z", "M", "F", "---", "S");

                    punto_old.X = x;
                    punto_old.Y = y;
                }
            }

            punto.X = x;
            punto.Y = y;

            oldDraw = draw;
        }

        private void WriteGCode(string s)
        {
            string n = "N" + noLine.ToString() + " ";

            if (rbScriviNumeroRigha.Checked == false) n = "";

            if (s != "")
            {
                noLine++;
                rtbGCODE.AppendText(n + s + '\n');
            }
        }

        private void WriteGCode(string G, string X, string Y, string Z, string M, string F, string T, string S)
        {
            string line = "";

            string n = "N" + noLine.ToString() + " ";

            if (rbScriviNumeroRigha.Checked == false) n = "";

            if (G != "G")
            {
                if (X != "X" || Y != "Y")
                {
                    if ((X == GC_X_old && Y == GC_Y_old) == false)
                    {
                        line = line + G;

                        line = line + " " + X + " " + Y;
                    }
                }

                if (Z != "Z")
                {
                    line = line + G;

                    if (Z != "Z") line = line + " " + Z;
                }

                if (G == "G01" && GC_G_old == "G00" && F != "F") line = line + " " + F;

                if ( T!= "---")
                {
                    line = line + " " + T;
                }

                if (S != "S")
                {
                    line = line + " " + S;
                }
            }

            if (line != "")
            {
                noLine++;
                rtbGCODE.AppendText(n + line + '\n');
            }

            GC_X_old = X;
            GC_Y_old = Y;
            GC_Z_old = Z;
            GC_G_old = G;
            GC_M_old = M;
            GC_F_old = F;
        }

        private string ToFloatStr(double f)
        {
            double m = 0.005;
            if (f < 0.0) m = m * -1.0;

            f = (f + m) * 100.0;

            f = ((int)f) / 100.0;

            string s = f.ToString().Replace(',', '.');

            if (s.LastIndexOf('.') == -1) s = s + ".";

            string nule = "00";

            s = s + nule;

            s = s.Substring(0, s.LastIndexOf('.') + 1 + nule.Length);

            return (s);
        }

        private string ToDecStr(double f)
        {
            f = (f + 0.5) * 10.0;

            int i = (int)(f / 10.0);

            return (i.ToString());
        }

        private Point ToScreen(PointF p)
        {
            return new Point((int)((double)(shiftX + offsetXDraw + (p.X - XMin) * sizeDraw * ZoomFactor)), (int)grfx.VisibleClipBounds.Height - (int)((double)(shiftY + offsetYDraw + (p.Y - YMin) * sizeDraw * ZoomFactor)));
        }

        private Point ToScreenOrig(PointF p)
        {
            return new Point((int)((double)(shiftX + offsetXDraw + (p.X - XMin) * sizeDraw)), (int)grfx.VisibleClipBounds.Height - (int)((double)(shiftY + offsetYDraw + (p.Y - YMin)) * sizeDraw));
        }

        private bool ApriPRG = false;

        private void btnApri_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            UkupnaDuzina = 0.0;

            ApriDialog();

            if (programHPGLExist == true)
            {
                ApriPRG = true;

                ProgramHPGL();
            }

            HOME();
        }

        private void ApriDialog()
        {
            string line = "";

            hpgl = new HPGL();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"\hpgl";
            openFileDialog1.Filter = "HPGL files (*.plt)|*.plt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Nome del HPGL file";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DisabilitaColori();

                programHPGLExist = false;
                rtbHPGL.Text = "";
                rtbGCODE.Text = "";
                rtbHPGLHiden.Text = "";
                rtbComands.Text = "";

                NumeroTotaleLinee = 0;

                ZoomFactor = 1.0;
                MoveH = true;
                MoveV = true;
                shiftX = 0;
                shiftY = 0;

                brojacLinija = 0;
                punto.X = 0;
                punto.Y = 0;

                string strTmp = "";

                try
                {
                    strTmp = openFileDialog1.FileName;

                    strTmp = strTmp.Substring(strTmp.LastIndexOf('\\') + 1, strTmp.Length - strTmp.LastIndexOf('\\') - 1);
                }
                catch (Exception e)
                { }

                this.Text = strTmp + " - " + "HPGL2GCode          by SD 2008";

                FileInfo fi = new FileInfo(FileName = openFileDialog1.FileName);

                if (fi == null)
                {
                    FileName = "";
                    return;
                }

                //
                zDW = double.Parse(txtImpostazioniZLavoro.TEXT); //.Replace(',', '.'));
                zUP = double.Parse(txtImpostazioniZSicurezza.TEXT); //.Replace(',', '.'));
                zSTP = (int)double.Parse(txtImpostazioniNumeroPassi.TEXT);
                zPP = zDW / zSTP;
                //

                StreamReader sr = fi.OpenText();

                while ((line = sr.ReadLine()) != null)
                {
                    AbilitaColori(line);

                    rtbHPGL.AppendText(line.Trim() + '\n');

                    hpgl.ElaboraHPGL(line.Trim());

                    ProgramInProgramEditor = hpgl.PROGRAMHPGL;

                    programHPGLExist = true;

                    btnRegenerate.Enabled = true;
                    btnDraw.Enabled = true;
                }

                rtbComands.AppendText("[OPEN]" + '\n');
                rtbComands.AppendText("File=" + FileName + '\n');
                rtbComands.AppendText('\n'.ToString());

                //line = "XX;SP2;PU0 0;PD-10 0;PD10 0;PU0 -10;PD0 10;";

                //richTextBox1.AppendText(line.Trim() + '\n');

                //hpgl.ElaboraHPGL(line.Trim());

                sr.Close();
            }
            else
            {
                this.Text = "HPGL2GCode          by SD 2008";

                //btnImpostazioni.Enabled = false;
                btnDraw.Enabled = false;

                Invalidate();
            }
        }

        private void DisabilitaColori()
        {
            cbColore1.Checked = false;
            cbColore2.Checked = false;
            cbColore3.Checked = false;
            cbColore4.Checked = false;
            cbColore5.Checked = false;
            cbColore6.Checked = false;
            cbColore7.Checked = false;
            cbColore8.Checked = false;

            cbColore1.Enabled = false;
            cbColore2.Enabled = false;
            cbColore3.Enabled = false;
            cbColore4.Enabled = false;
            cbColore5.Enabled = false;
            cbColore6.Enabled = false;
            cbColore7.Enabled = false;
            cbColore8.Enabled = false;
        }

        private void AbilitaColori(string s)
        {
            s = s.ToUpper();

            if (s.IndexOf("SP1") != -1) { cbColore1.Enabled = true; cbColore1.Checked = true; }
            if (s.IndexOf("SP2") != -1) { cbColore2.Enabled = true; cbColore2.Checked = true; }
            if (s.IndexOf("SP3") != -1) { cbColore3.Enabled = true; cbColore3.Checked = true; }
            if (s.IndexOf("SP4") != -1) { cbColore4.Enabled = true; cbColore4.Checked = true; }
            if (s.IndexOf("SP5") != -1) { cbColore5.Enabled = true; cbColore5.Checked = true; }
            if (s.IndexOf("SP6") != -1) { cbColore6.Enabled = true; cbColore6.Checked = true; }
            if (s.IndexOf("SP7") != -1) { cbColore7.Enabled = true; cbColore7.Checked = true; }
            if (s.IndexOf("SP8") != -1) { cbColore8.Enabled = true; cbColore8.Checked = true; }
        }

        private void btnZoomOUT_Click(object sender, EventArgs e)
        {
            if (ZoomFactor <= 0.1f)
            {
                ZoomFactor = 0.1f;
            }
            else
            {
                ZoomFactor -= 0.1f;

                shiftX += 22;
                shiftY += 20;
            }

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private void btnZoomIN_Click(object sender, EventArgs e)
        {
            ZoomFactor += 0.1f;

            shiftX -= 22;
            shiftY -= 20;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private void btnZoomHOME_Click(object sender, EventArgs e)
        {
            HOME();
        }

        private void HOME()
        {
            timer1.Stop();

            ZoomFactor = 1.0;
            MoveH = true;
            MoveV = true;
            shiftX = 0;
            shiftY = 0;

            PAN = false;
            btnPAN.BackColor = Color.Transparent;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private void btnShiftXdw_Click(object sender, EventArgs e)
        {
            MoveV = false;

            shiftY += 10;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private void btnShiftXup_Click(object sender, EventArgs e)
        {
            MoveV = false;

            shiftY -= 10;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private bool MoveH = true;
        private bool MoveV = true;

        private void btnShiftXdx_Click(object sender, EventArgs e)
        {
            MoveH = false;

            shiftX -= 10;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private void btnShiftXsx_Click(object sender, EventArgs e)
        {
            MoveH = false;

            shiftX += 10;

            DisegnaProgram(2);
            IspisiDimenzije(2);
        }

        private int panXDW = 0;
        private int panXUP = 0;
        private int panYDW = 0;
        private int panYUP = 0;

        private bool MousePAN = false;

        private void pnlDisegno_MouseDown(object sender, MouseEventArgs e)
        {
            if (PAN == true)
            {
                MousePAN = true;

                panXDW = e.X;
                panYDW = e.Y;
            }
        }

        private void pnlDisegno_MouseUp(object sender, MouseEventArgs e)
        {
            if (PAN == true && MousePAN == true)
            {
                MousePAN = false;

                panXUP = e.X;
                panYUP = e.Y;

                shiftX += (panXUP - panXDW);
                shiftY += (panYDW - panYUP);

                MoveH = false;
                MoveV = false;

                DisegnaProgram(2);
                IspisiDimenzije(2);
            }
        }

        private bool PAN = false;

        private void btnPAN_Click(object sender, EventArgs e)
        {
            PAN = !PAN;

            if (PAN == true)
            {
                btnPAN.BackColor = Color.Red;
            }
            else
            {
                btnPAN.BackColor = Color.Transparent;
            }
        }

        private int brojacLinija = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (brojacLinija >= DrawingLines.Count)
            {
                return;
            }

            Linea l = (AsseTest.Linea)DrawingLines[brojacLinija];

            double x = l.x;
            double y = l.y;
            double z = l.z;
            int SP = l.sp;
            bool draw = l.draw;

            Pen pen = new Pen(Color.White);

            grfxBm.DrawLine(pen, ToScreen(new PointF((float)(punto.X), (float)(punto.Y))), ToScreen(new PointF((float)(x), (float)(y))));

            if (bitmapHPGL != null)
            {
                Graphics g = Graphics.FromImage(bitmapHPGL);
                g.Clear(System.Drawing.Color.DarkKhaki);
                g.Dispose();
            }

            bitmapHPGL = new Bitmap(bitmap);

            Invalidate();

            punto.X = x;
            punto.Y = y;

            brojacLinija++;

            timer1.Start();
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            brojacLinija = 0;

            if (programHPGLExist == false) return;

            punto.X = 0;
            punto.Y = 0;

            timer1.Enabled = true;
            timer1.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (rtbGCODE.Text != "")
            {
                saveFileDialog1.FileName = FileName.Substring(0, FileName.Length - 4);

                string sT = saveFileDialog1.FileName;

                sT = sT.Substring(sT.LastIndexOf("\\") + 1, sT.Length - sT.LastIndexOf("\\") - 1);

                saveFileDialog1.DefaultExt = "ngc";
                saveFileDialog1.AddExtension = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;

                    if (filename.IndexOf('.') == -1) filename = filename + ".ngc";

                    SaveGCode(filename);
                    SaveGCode(@"C:\Program Files\CncSimulator\ncfiles\" + sT + ".nc");
                }
            }
        }

        private void SaveGCode(string path)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(path);

                if (sw == null) return;

                string[] s = rtbGCODE.Text.Split('\n');

                foreach (string str in s)
                {
                    sw.WriteLine(str);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }

        public int premuto = 0;

        private void pozicijaCTRL_MouseClick(object sender, MouseEventArgs e)
        {
            //double xmi 

        }

        public void poz1_PozicijaChange(object Poz, int Premuto)
        {
            premuto = Premuto;

            switch (premuto)
            {
                case 1:
                    txtPosizioneX.TEXT = ToFloatStr(-XMin);
                    txtPosizioneY.TEXT = ToFloatStr(-YMax);
                    break;
                case 2:
                    txtPosizioneX.TEXT = ToFloatStr(0.0);
                    txtPosizioneY.TEXT = ToFloatStr(-YMax);
                    break;
                case 3:
                    txtPosizioneX.TEXT = ToFloatStr(-XMax);
                    txtPosizioneY.TEXT = ToFloatStr(-YMax);
                    break;
                case 4:
                    txtPosizioneX.TEXT = ToFloatStr(-XMax);
                    txtPosizioneY.TEXT = ToFloatStr(0.0);
                    break;
                case 5:
                    txtPosizioneX.TEXT = ToFloatStr(-XMax);
                    txtPosizioneY.TEXT = ToFloatStr(-YMin);
                    break;
                case 6:
                    txtPosizioneX.TEXT = ToFloatStr(0.0);
                    txtPosizioneY.TEXT = ToFloatStr(-YMin);
                    break;
                case 7:
                    txtPosizioneX.TEXT = ToFloatStr(-XMin);
                    txtPosizioneY.TEXT = ToFloatStr(-YMin);
                    break;
                case 8:
                    txtPosizioneX.TEXT = ToFloatStr(-XMin);
                    txtPosizioneY.TEXT = ToFloatStr(0.0);
                    break;
                case 9:
                    txtPosizioneX.TEXT = ToFloatStr(-XMin - (XMax - XMin) / 2.0);
                    txtPosizioneY.TEXT = ToFloatStr(-YMin - (YMax - YMin) / 2.0);
                    break;
            }
        }

        private double zMinimalna = 0.0;

        private void NadjiZMinimalna()
        {
            zMinimalna = 9999999.0;

            if (ibColore1.VALUE < zMinimalna) zMinimalna = ibColore1.VALUE;
            if (ibColore2.VALUE < zMinimalna) zMinimalna = ibColore2.VALUE;
            if (ibColore3.VALUE < zMinimalna) zMinimalna = ibColore3.VALUE;
            if (ibColore4.VALUE < zMinimalna) zMinimalna = ibColore4.VALUE;
            if (ibColore5.VALUE < zMinimalna) zMinimalna = ibColore5.VALUE;
            if (ibColore6.VALUE < zMinimalna) zMinimalna = ibColore6.VALUE;
            if (ibColore7.VALUE < zMinimalna) zMinimalna = ibColore7.VALUE;
            if (ibColore8.VALUE < zMinimalna) zMinimalna = ibColore8.VALUE;
        }

        private void Izracujaj(int vrsta)
        {
            if (programHPGLExist == true)
            {
                rtbGCODE.Text = "";

                programHPGLExist = false;
                NumeroTotaleLinee = 0;

                ZoomFactor = 1.0;
                MoveH = true;
                MoveV = true;
                shiftX = 0;
                shiftY = 0;

                brojacLinija = 0;
                punto.X = 0;
                punto.Y = 0;

                UkupnaDuzina = 0.0;

                string nome;

                string strTmp = "";

                double offX = 0.0;
                double offY = 0.0;

                NadjiZMinimalna();

                if (vrsta == 1 || vrsta == 2)
                {
                    try
                    {
                        offX = (double)txtPosizioneX.VALUE;
                        offY = (double)txtPosizioneY.VALUE;
                    }
                    catch (Exception e_6)
                    { }
                }

                sizeX = 1.0f;
                sizeY = 1.0f;

                if (vrsta == 3)
                {
                    try
                    {
                        sizeX = (double)txtScalaX.VALUE / 100.0;
                        sizeY = (double)txtScalaY.VALUE / 100.0;

                        if (HBtnPress == true) sizeX = sizeX * -1.0;
                        if (VBtnPress == true) sizeY = sizeY * -1.0;
                    }
                    catch (Exception e_6)
                    { }
                }

                if (vrsta == 5)
                {
                    try
                    {
                        sizeX =  txtDimensioniX.VALUE / (double)(XMax - XMin);
                        sizeY =  txtDimensioniY.VALUE / (double)(YMax - YMin);
                    }
                    catch (Exception e_6)
                    { }
                }

                string[] strLines = rtbHPGL.Text.Split(';');

                if (vrsta != 2) // || vrsta != 4)
                {
                    rtbHPGL.Text = ""; //rtbHPGLHiden.Text = "";
                    hpgl = new HPGL();
                }

                foreach (string s in strLines)
                {
                    string st = s.ToUpper();
                    st = st.Replace("\n", "");

                    if (st.StartsWith("PU") || st.StartsWith("PD") || st.StartsWith("PA"))
                    {
                        try
                        {
                            if (st.Length > 2)
                            {
                                double pX = 0.0;
                                double pY = 0.0;

                                string begin = st.Substring(0, 2);
                                string sep = " ";

                                st = st.Substring(2, st.Length - 2);

                                if (st.IndexOf(' ') == -1)
                                {
                                    pars = st.Split(',');
                                    sep = ",";
                                }
                                else
                                {
                                    pars = st.Split(' ');
                                    sep = " ";
                                }

                                pX = (double)Int32.Parse(pars[0]) / 10.0;
                                pY = (double)Int32.Parse(pars[1]) / 10.0;

                                //-----------------------SIZE
                                pX = (pX) * sizeX + offX;
                                pY = (pY) * sizeY + offY;
                                //---------------------------

                                pX *= 10.0;
                                pY *= 10.0;

                                st = begin + ToDecStr(pX) + sep + ToDecStr(pY);
                            }
                        }

                        catch (Exception e_5)
                        {

                        }
                    }

                    if (st.Trim(' ').TrimEnd('\n') != "")
                    {
                        st = st + ';';

                        rtbHPGL.AppendText(st + '\n'); //rtbHPGLHiden.AppendText(st + '\n');

                        hpgl.ElaboraHPGL(st.Trim());

                        ProgramInProgramEditor = hpgl.PROGRAMHPGL;

                        programHPGLExist = true;

                        //btnImpostazioni.Enabled = true;
                        btnDraw.Enabled = true;
                    }
                }

                ProgramHPGL();

                HOME();               
            }
        }

        private void inputBox1_inputBoxChange(object InputBox, double VALUE)
        {
        }

        bool HBtnPress = false;
        bool VBtnPress = false;

        private void btnScalaHMirror_Click(object sender, EventArgs e)
        {
            if (HBtnPress == true)
            {
                btnScalaHMirror.FlatStyle = System.Windows.Forms.FlatStyle.System;
                btnScalaHMirror.UseVisualStyleBackColor = true;

                HBtnPress = false;
            }
            else
            {
                btnScalaHMirror.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                btnScalaHMirror.UseVisualStyleBackColor = false;
                btnScalaHMirror.BackColor = System.Drawing.Color.Tan;

                HBtnPress = true;
            }
        }

        private void btnScalaVMirror_Click(object sender, EventArgs e)
        {
            if (VBtnPress == true)
            {
                btnScalaVMirror.FlatStyle = System.Windows.Forms.FlatStyle.System;
                btnScalaVMirror.UseVisualStyleBackColor = true;

                VBtnPress = false;
            }
            else
            {
                btnScalaVMirror.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                btnScalaVMirror.UseVisualStyleBackColor = false;
                btnScalaVMirror.BackColor = System.Drawing.Color.Tan;

                VBtnPress = true;
            }
        }

        private void txtImpostazioniZLavoro_inputBoxChange(object InputBox, double VALUE)
        {
            txtImpostazioniZPerPasso.VALUE = txtImpostazioniZLavoro.VALUE / txtImpostazioniNumeroPassi.VALUE;

            try
            {
                zDW = double.Parse(txtImpostazioniZLavoro.TEXT); //.Replace(',', '.'));
            }
            catch (Exception excImpostazioni)
            { }
        }

        private void txtImpostazioniNumeroPassi_inputBoxChange(object InputBox, double VALUE)
        {
            txtImpostazioniZPerPasso.VALUE = txtImpostazioniZLavoro.VALUE / txtImpostazioniNumeroPassi.VALUE;
            zSTP = (int)double.Parse(txtImpostazioniNumeroPassi.TEXT);
            zPP = zDW / zSTP;

            try
            {
                zDW = double.Parse(txtImpostazioniZLavoro.TEXT); //.Replace(',', '.'));
            }
            catch (Exception excImpostazioni)
            { }
        }

        private void txtImpostazioniZSicurezza_inputBoxChange(object InputBox, double VALUE)
        {
            try
            {
                zUP = double.Parse(txtImpostazioniZSicurezza.TEXT); //.Replace(',', '.'));
            }
            catch (Exception excImpostazioni)
            { }
        }

        private void btnApplicaPosizione_Click(object sender, EventArgs e)
        {
            rtbComands.AppendText("[MOVE]" + '\n');
            rtbComands.AppendText("X=" + txtPosizioneX.TEXT + '\n');
            rtbComands.AppendText("Y=" + txtPosizioneY.TEXT + '\n');
            rtbComands.AppendText("ZS=" + txtImpostazioniZSicurezza.TEXT + '\n');
            rtbComands.AppendText("NP=" + txtImpostazioniNumeroPassi.TEXT + '\n');
            rtbComands.AppendText("ZP=" + txtImpostazioniZPerPasso.TEXT + '\n');
            rtbComands.AppendText('\n'.ToString());

            Izracujaj(1);
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    rtbComands.AppendText(@"[MOVE COPY]" + '\n');
        //    rtbComands.AppendText("X=" + txtPosizioneX.TEXT + '\n');
        //    rtbComands.AppendText("Y=" + txtPosizioneY.TEXT + '\n');
        //    rtbComands.AppendText("ZS=" + txtImpostazioniZSicurezza.TEXT + '\n');
        //    rtbComands.AppendText("NP=" + txtImpostazioniNumeroPassi.TEXT + '\n');
        //    rtbComands.AppendText("ZP=" + txtImpostazioniZPerPasso.TEXT + '\n');
        //    rtbComands.AppendText('\n'.ToString());

        //    Izracujaj(2);
        //}

        private void btnScalaApplica_Click(object sender, EventArgs e)
        {
            rtbComands.AppendText(@"[SCALE]" + '\n');
            rtbComands.AppendText("X=" + txtPosizioneX.TEXT + '\n');
            rtbComands.AppendText("Y=" + txtPosizioneY.TEXT + '\n');
            rtbComands.AppendText("ZS=" + txtImpostazioniZSicurezza.TEXT + '\n');
            rtbComands.AppendText("NP=" + txtImpostazioniNumeroPassi.TEXT + '\n');
            rtbComands.AppendText("ZP=" + txtImpostazioniZPerPasso.TEXT + '\n');
            rtbComands.AppendText('\n'.ToString());

            Izracujaj(3);
        }

        private void btnApplicaDimensioni_Click(object sender, EventArgs e)
        {
            rtbComands.AppendText(@"[SIZE]" + '\n');
            rtbComands.AppendText("X=" + txtPosizioneX.TEXT + '\n');
            rtbComands.AppendText("Y=" + txtPosizioneY.TEXT + '\n');
            rtbComands.AppendText("ZS=" + txtImpostazioniZSicurezza.TEXT + '\n');
            rtbComands.AppendText("NP=" + txtImpostazioniNumeroPassi.TEXT + '\n');
            rtbComands.AppendText("ZP=" + txtImpostazioniZPerPasso.TEXT + '\n');
            rtbComands.AppendText('\n'.ToString());

            Izracujaj(5);
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    rtbComands.AppendText(@"[ZOOM COPY]" + '\n');
        //    rtbComands.AppendText("X=" + txtPosizioneX.TEXT + '\n');
        //    rtbComands.AppendText("Y=" + txtPosizioneY.TEXT + '\n');
        //    rtbComands.AppendText("ZS=" + txtImpostazioniZSicurezza.TEXT + '\n');
        //    rtbComands.AppendText("NP=" + txtImpostazioniNumeroPassi.TEXT + '\n');
        //    rtbComands.AppendText("ZP=" + txtImpostazioniZPerPasso.TEXT + '\n');
        //    rtbComands.AppendText('\n'.ToString());

        //    Izracujaj(4);
        //}

        private void btnRegenerate_Click(object sender, EventArgs e)
        {
            Izracujaj(0);
        }

        private void rtbGCODE_TextChanged(object sender, EventArgs e)
        {
            if (rtbHPGL.Text != "")
            {
                btnSaveGCode.Enabled = true;
                btnRegenerate.Enabled = true;
            }
            else
            {
                btnSaveGCode.Enabled = false;
                btnRegenerate.Enabled = false;
            }
        }

        private void cbOtimizzaXDistanza_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOtimizzaXDistanza.Checked == true)
                txtOtimizzaXDistanza.Enabled = true;
            else
                txtOtimizzaXDistanza.Enabled = false;
        }

        private void txtUkupnaDuzina_Click(object sender, EventArgs e)
        {

        }

        private int DimensioniKoMijenja = 0;

        private void txtDimensioniX_inputBoxChange(object InputBox, double VALUE)
        {
            //if (DimensioniKoMijenja == 3) { DimensioniKoMijenja = 0; return; }
            if (DimensioniKoMijenja == 0) DimensioniKoMijenja = 1;
            if (DimensioniKoMijenja == 2) return;

            if (cbDimensioniNonProporzionale.Checked == false && DimensioniKoMijenja == 1)
                txtDimensioniY.VALUE = txtDimensioniY.VALUE + (txtDimensioniX.VALUE - txtDimensioniX.OLD_VALUE) * (1.0 / fattoreDimensioni);

            DimensioniKoMijenja = 0;
        }

        private void txtDimensioniY_inputBoxChange(object InputBox, double VALUE)
        {
            //if (DimensioniKoMijenja == 3) { DimensioniKoMijenja = 0; return; }
            if (DimensioniKoMijenja == 0) DimensioniKoMijenja = 2;
            if (DimensioniKoMijenja == 1) return;

            if (cbDimensioniNonProporzionale.Checked == false && DimensioniKoMijenja == 2)
                txtDimensioniX.VALUE = txtDimensioniX.VALUE + (txtDimensioniY.VALUE - txtDimensioniY.OLD_VALUE) * fattoreDimensioni;

            DimensioniKoMijenja = 0;
        }

        private void cbDimensioniNonProporzionale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDimensioniNonProporzionale.Checked == false)
            {
                fattoreDimensioni = txtDimensioniX.VALUE / txtDimensioniY.VALUE;
            }

            DimensioniKoMijenja = 0;
        }

        private int ScalaKoMijenja = 0;

        private void txtScalaX_inputBoxChange(object InputBox, double VALUE)
        {
            //if (ScalaKoMijenja == 3) { ScalaKoMijenja = 0; return; }
            if (ScalaKoMijenja == 0) ScalaKoMijenja = 1;
            if (ScalaKoMijenja == 2) return;

            if (cbScalaNonProporzionale.Checked == false && ScalaKoMijenja == 1)
                txtScalaY.VALUE = txtScalaY.VALUE + (txtScalaX.VALUE - txtScalaX.OLD_VALUE) * (1.0 / fattoreScala);

            ScalaKoMijenja = 0;
        }

        private void txtScalaY_inputBoxChange(object InputBox, double VALUE)
        {
            //if (ScalaKoMijenja == 3) { ScalaKoMijenja = 0; return; }
            if (ScalaKoMijenja == 0) ScalaKoMijenja = 2;
            if (ScalaKoMijenja == 1) return;

            if (cbScalaNonProporzionale.Checked == false && ScalaKoMijenja == 2)
                txtScalaX.VALUE = txtScalaX.VALUE + (txtScalaY.VALUE - txtScalaY.OLD_VALUE) * fattoreScala;

            ScalaKoMijenja = 0;
        }

        private double fattoreScala = 1.0;
        private double fattoreDimensioni = 1.0;

        private void cbScalaNonProporzionale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbScalaNonProporzionale.Checked == false)
            {
                fattoreScala = txtScalaX.VALUE / txtScalaY.VALUE;
            }

            ScalaKoMijenja = 0;
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            txtScalaX.VALUE = 100.0;
            fattoreScala = txtScalaX.VALUE / txtScalaY.VALUE;
            ScalaKoMijenja = 0;
        }

        private void label11_DoubleClick(object sender, EventArgs e)
        {
            txtScalaY.VALUE = 100.0;
            fattoreScala = txtScalaX.VALUE / txtScalaY.VALUE;
            ScalaKoMijenja = 0;
        }

        private void cbScalaNonProporzionale_MouseDown(object sender, MouseEventArgs e)
        {
            ScalaKoMijenja = 3;
            fattoreScala = 1;
        }

        private void cbDimensioniNonProporzionale_MouseDown(object sender, MouseEventArgs e)
        {
            DimensioniKoMijenja = 3;
            fattoreDimensioni = 1;
        }

        private void label13_Click(object sender, EventArgs e)
        {
            txtDimensioniX.TEXT = ToFloatStr(OrigDimX);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            txtDimensioniY.TEXT = ToFloatStr(OrigDimY);
        }

        private void cbColore1_CheckedChanged(object sender, EventArgs e)
        {
            ibColore1.Enabled = cbColore1.Checked;
            cbTool1.Enabled = cbColore1.Checked;
        }

        private void cbColore2_CheckedChanged(object sender, EventArgs e)
        {
            ibColore2.Enabled = cbColore2.Checked;
            cbTool2.Enabled = cbColore2.Checked;
        }

        private void cbColore3_CheckedChanged(object sender, EventArgs e)
        {
            ibColore3.Enabled = cbColore3.Checked;
            cbTool3.Enabled = cbColore3.Checked;
        }

        private void cbColore4_CheckedChanged(object sender, EventArgs e)
        {
            ibColore4.Enabled = cbColore4.Checked;
            cbTool4.Enabled = cbColore4.Checked;
        }

        private void cbColore5_CheckedChanged(object sender, EventArgs e)
        {
            ibColore5.Enabled = cbColore5.Checked;
            cbTool5.Enabled = cbColore5.Checked;
        }

        private void cbColore6_CheckedChanged(object sender, EventArgs e)
        {
            ibColore6.Enabled = cbColore6.Checked;
            cbTool6.Enabled = cbColore6.Checked;
        }

        private void cbColore7_CheckedChanged(object sender, EventArgs e)
        {
            ibColore7.Enabled = cbColore7.Checked;
            cbTool7.Enabled = cbColore7.Checked;
        }

        private void cbColore8_CheckedChanged(object sender, EventArgs e)
        {
            ibColore8.Enabled = cbColore8.Checked;
            cbTool8.Enabled = cbColore8.Checked;
        }

        private void frmQuote_FormClosed(object sender, FormClosedEventArgs e)
        {
            StreamWriter sw = null;

            try
            {
                if (File.Exists("Impostazioni.ini") == true) File.Delete("Impostazioni.ini");
            }
            catch (Exception exc)
            { 
            
            }

            try
            {
                sw = new StreamWriter("Impostazioni.ini");

                sw.WriteLine("[Impostazioni]");
                sw.WriteLine(txtImpostazioniZSicurezza.TEXT.Replace(',', '.'));
                sw.WriteLine(txtImpostazioniZLavoro.TEXT.Replace(',', '.'));
                sw.WriteLine(txtImpostazioniNumeroPassi.TEXT.Replace(',', '.'));
                sw.WriteLine(txtImpostazioniZPerPasso.TEXT.Replace(',', '.'));
                if(rbScriviNumeroRigha.Checked == true)
                    sw.WriteLine("1");
                else
                    sw.WriteLine("0");
                if (cbOtimizzaXDistanza.Checked == true)
                    sw.WriteLine("1");
                else
                    sw.WriteLine("0");
                sw.WriteLine(txtOtimizzaXDistanza.TEXT.Replace(',', '.'));
                if (cbOrdinaXColore.Checked == true)
                    sw.WriteLine("1");
                else
                    sw.WriteLine("0");
                sw.WriteLine("");
                sw.WriteLine("[Testa]");
                sw.WriteLine(rtbInizio.Text);
                //sw.WriteLine("");
                sw.WriteLine("[Coda]");
                sw.WriteLine(rtbFine.Text);
                //sw.WriteLine("");
                sw.WriteLine("[Colori]");
                sw.WriteLine(ibColore1.TEXT.Replace(',', '.') + ";" + cbTool1.Text);
                sw.WriteLine(ibColore2.TEXT.Replace(',', '.') + ";" + cbTool2.Text);
                sw.WriteLine(ibColore3.TEXT.Replace(',', '.') + ";" + cbTool3.Text);
                sw.WriteLine(ibColore4.TEXT.Replace(',', '.') + ";" + cbTool4.Text);
                sw.WriteLine(ibColore5.TEXT.Replace(',', '.') + ";" + cbTool5.Text);
                sw.WriteLine(ibColore6.TEXT.Replace(',', '.') + ";" + cbTool6.Text);
                sw.WriteLine(ibColore7.TEXT.Replace(',', '.') + ";" + cbTool7.Text);
                sw.WriteLine(ibColore8.TEXT.Replace(',', '.') + ";" + cbTool8.Text);
                sw.WriteLine("");
                sw.WriteLine("[Utensili]");
                sw.WriteLine(ibMaxT1.TEXT.Replace(',', '.') + ";" + ibFT1.TEXT.Replace(',', '.') + ";" + ibST1.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT2.TEXT.Replace(',', '.') + ";" + ibFT2.TEXT.Replace(',', '.') + ";" + ibST2.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT3.TEXT.Replace(',', '.') + ";" + ibFT3.TEXT.Replace(',', '.') + ";" + ibST3.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT4.TEXT.Replace(',', '.') + ";" + ibFT4.TEXT.Replace(',', '.') + ";" + ibST4.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT5.TEXT.Replace(',', '.') + ";" + ibFT5.TEXT.Replace(',', '.') + ";" + ibST5.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT6.TEXT.Replace(',', '.') + ";" + ibFT6.TEXT.Replace(',', '.') + ";" + ibST6.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT7.TEXT.Replace(',', '.') + ";" + ibFT7.TEXT.Replace(',', '.') + ";" + ibST7.TEXT.Replace(',', '.'));
                sw.WriteLine(ibMaxT8.TEXT.Replace(',', '.') + ";" + ibFT8.TEXT.Replace(',', '.') + ";" + ibST8.TEXT.Replace(',', '.'));
            }
            catch (Exception exc)
            { }
            finally 
            {
                if (sw != null) sw.Close();
            }
        }

        private void UcitajImpostazioni()
        {
            StreamReader sr = null;
            FileInfo fi = null;

            string line = "";

            if (File.Exists("Impostazioni.ini") == true)
            {
                try
                {
                    fi = new FileInfo("Impostazioni.ini");
                    sr = fi.OpenText();

                    while ((line = sr.ReadLine().Replace("\n","")) != null)
                    {
                        if (line == "[Impostazioni]")
                        {
                            txtImpostazioniZSicurezza.TEXT = sr.ReadLine().Replace("\n", "");
                            txtImpostazioniZLavoro.TEXT = sr.ReadLine().Replace("\n", "");
                            txtImpostazioniNumeroPassi.TEXT = sr.ReadLine().Replace("\n", "");
                            txtImpostazioniZPerPasso.TEXT = sr.ReadLine().Replace("\n", "");
                            //txtVelocitaDiLavorazione.TEXT = sr.ReadLine().Replace("\n", "");
                            if (sr.ReadLine().Replace("\n", "").IndexOf("1") != -1)
                                rbScriviNumeroRigha.Checked = true;
                            else
                                rbScriviNumeroRigha.Checked = false;
                            if (sr.ReadLine().Replace("\n", "").IndexOf("1") != -1)
                                cbOtimizzaXDistanza.Checked = true;
                            else
                                cbOtimizzaXDistanza.Checked = false;
                            txtOtimizzaXDistanza.TEXT = sr.ReadLine().Replace("\n", "");
                            if (sr.ReadLine().Replace("\n", "").IndexOf("1") != -1)
                                cbOrdinaXColore.Checked = true;
                            else
                                cbOrdinaXColore.Checked = false;
                        }
                        if (line == "[Testa]")
                        {
                            rtbInizio.Text = "";

                            while ((line = sr.ReadLine().Replace("\n", "")) != null && line.IndexOf("[") == -1)
                            {
                                if(line != "") rtbInizio.AppendText(line + '\n');
                            }
                        }
                        if (line == "[Coda]")
                        {
                            rtbFine.Text = "";

                            while ((line = sr.ReadLine().Replace("\n", "")) != null && line.IndexOf("[") == -1)
                            {
                                if (line != "") rtbFine.AppendText(line + '\n');
                            }
                        }
                        if (line == "[Colori]")
                        {
                            for (int i = 1; i <= 8; i++)
                            {
                                line = sr.ReadLine().Replace("\n", "");
                                string[] s = line.Split(';');

                                if (i == 1) { ibColore1.TEXT = s[0]; cbTool1.Text = s[1]; }
                                if (i == 2) { ibColore2.TEXT = s[0]; cbTool2.Text = s[1]; }
                                if (i == 3) { ibColore3.TEXT = s[0]; cbTool3.Text = s[1]; }
                                if (i == 4) { ibColore4.TEXT = s[0]; cbTool4.Text = s[1]; }
                                if (i == 5) { ibColore5.TEXT = s[0]; cbTool5.Text = s[1]; }
                                if (i == 6) { ibColore6.TEXT = s[0]; cbTool6.Text = s[1]; }
                                if (i == 7) { ibColore7.TEXT = s[0]; cbTool7.Text = s[1]; }
                                if (i == 8) { ibColore8.TEXT = s[0]; cbTool8.Text = s[1]; }
                            }
                        }
                        if (line == "[Utensili]")
                        {
                            for (int i = 1; i <= 8; i++)
                            {
                                line = sr.ReadLine().Replace("\n", "");
                                string[] s = line.Split(';');

                                if (i == 1) { ibMaxT1.TEXT = s[0]; ibFT1.TEXT = s[1]; ; ibST1.TEXT = s[2]; }
                                if (i == 2) { ibMaxT2.TEXT = s[0]; ibFT2.TEXT = s[1]; ; ibST2.TEXT = s[2]; }
                                if (i == 3) { ibMaxT3.TEXT = s[0]; ibFT3.TEXT = s[1]; ; ibST3.TEXT = s[2]; }
                                if (i == 4) { ibMaxT4.TEXT = s[0]; ibFT4.TEXT = s[1]; ; ibST4.TEXT = s[2]; }
                                if (i == 5) { ibMaxT5.TEXT = s[0]; ibFT5.TEXT = s[1]; ; ibST5.TEXT = s[2]; }
                                if (i == 6) { ibMaxT6.TEXT = s[0]; ibFT6.TEXT = s[1]; ; ibST6.TEXT = s[2]; }
                                if (i == 7) { ibMaxT7.TEXT = s[0]; ibFT7.TEXT = s[1]; ; ibST7.TEXT = s[2]; }
                                if (i == 8) { ibMaxT8.TEXT = s[0]; ibFT8.TEXT = s[1]; ; ibST8.TEXT = s[2]; }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                
                }
                finally
                {
                    if (sr != null) sr.Close();    
                }
            }
        }
    }
}
