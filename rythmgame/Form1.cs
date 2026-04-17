namespace rythmgame
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void GameLoop(object sender, EventArgs e)
        {
            // Game logic and rendering code goes here
            Invalidate();
        }
    }
}
