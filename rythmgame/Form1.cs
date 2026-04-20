using System.Diagnostics;
using System.Text.Json;

namespace rythmgame
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        int virtualWidth = 800;
        int virtualHeight = 600;

        public static int bpm = 120;
        int msPerBeat = 60000 / bpm;

        List<string> beatmapsPaths = new List<string>();
        float traveltime = 2.0f;

        Stopwatch stopwatch = new Stopwatch();
        float lastTime = 0f;


        float scaleX = 1f;
        float scaleY = 1f;
        int score = 0;

        int spawnY = 0;

        private ListBox listBoxMaps;

        public Form1()
        {
            InitializeComponent();

            listBoxMaps = new ListBox();
            listBoxMaps.Location = new Point(10, 10);
            listBoxMaps.Size = new Size(300, 400);
            this.Controls.Add(listBoxMaps);

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true
                );
            this.UpdateStyles();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string beatmapFolder = "Beatmaps";

            if (!Directory.Exists(beatmapFolder))
                Directory.CreateDirectory(beatmapFolder);

            string[] files =
                Directory.GetFiles(beatmapFolder, "*.json");

            foreach (string file in files)
            {
                beatmapsPaths.Add(file);
                listBoxMaps.Items.Add(file);
            }
            Button playButton = new Button();
            playButton.Location = new Point(50, 50);
            playButton.Size = new Size(100, 30);


        }
        float offsetX = 0;
        float offsetY = 0;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            float scale = Math.Min(
                (float)ClientSize.Width / virtualWidth,
                (float)ClientSize.Height / virtualHeight);

            scaleX = scale;
            scaleY = scale;

            offsetX = (ClientSize.Width - virtualWidth * scaleX) / 2f;
            offsetY = (ClientSize.Height - virtualHeight * scaleY) / 2f;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if(listBoxMaps.SelectedItems.Count == -1)
                return;

            string path =
                beatmapsPaths[listBoxMaps.SelectedIndex];
            Beatmap beatmap = 
                BeatmapLoader.Load(path);
            gameform game =
                new gameform(beatmap);

            game.Show();

            this.Hide();
        }




        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }

    }
}
