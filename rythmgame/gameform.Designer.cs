using System.Diagnostics;
using System.Text.Json;

namespace rythmgame
{
    public partial class gameForm : Form
    {
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
        List<Beatnote> beatmap = new List<Beatnote>();
        List<Note> notes = new List<Note>();
        int virtualWidth = 800;
        int virtualHeight = 600;
        public Beatmap currentBeatmap;

        public static int bpm = 120;
        int msPerBeat = 60000 / bpm;

        float traveltime = 2.0f;

        Stopwatch stopwatch = new Stopwatch();
        float lastTime = 0f;
        float scaleX = 1f;
        float scaleY = 1f;
        int score = 0;
        int spawnY = 0;

        public gameForm(Beatmap beatmapData)
        {
            InitializeComponent();


            currentBeatmap = beatmapData;

            beatmap = new List<Beatnote>(currentBeatmap.notes);




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

            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds / 1000f;
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "gameForm";
            this.Text = "gameForm";

            this.ResumeLayout(false);
        }


        void GameLoop(object sender, EventArgs e)
        {
            // Game logic and rendering code goes here
            Input();

            float currentTime = stopwatch.ElapsedMilliseconds / 1000f;
            float deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            spawnNotes(currentTime);

            foreach (var note in notes)
            {
                note.y += note.speed * deltaTime;
            }
            CheckMissedNotes();
            Invalidate();




        }

        void spawnNotes(float currentTime)
        {
            for (int i = beatmap.Count - 1; i >= 0; i--)
            {
                float spawnTime = beatmap[i].time - traveltime;

                if (currentTime >= spawnTime)
                {
                    notes.Add(new Note
                    {
                        lane = beatmap[i].lane,
                        y = spawnY,
                        speed = virtualHeight / traveltime
                    });
                    beatmap.RemoveAt(i);
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(offsetX, offsetY);
            g.ScaleTransform(scaleX, scaleY);

            foreach (var note in notes)
            {
                g.FillRectangle(Brushes.Blue,
                    note.lane * virtualWidth / 4,
                    (int)note.y,
                    100,
                    20);
            }
            float hitLineY = virtualHeight - 50;



            // Draw hit line

            g.DrawLine(Pens.Red, 0, hitLineY, virtualWidth, hitLineY);
            e.Graphics.DrawString($"Score: {score}", new Font("Arial", 24), Brushes.White, 10, 10);
        }


        void Input()
        {
            if ((Control.ModifierKeys & Keys.D) == Keys.D || IsKeyDown(Keys.D))
            {
                CheckHit(0);
            }
            if ((Control.ModifierKeys & Keys.F) == Keys.F || IsKeyDown(Keys.F))
            {
                CheckHit(1);
            }
            if ((Control.ModifierKeys & Keys.J) == Keys.J || IsKeyDown(Keys.J))
            {
                CheckHit(2);
            }
            if ((Control.ModifierKeys & Keys.K) == Keys.K || IsKeyDown(Keys.K))
            {
                CheckHit(3);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }

        void CheckHit(int lane)
        {
            float hitLineY = virtualHeight - 50;
            for (int i = notes.Count - 1; i >= 0; i--)
            {
                if (notes[i].lane == lane &&
                    Math.Abs(notes[i].y - hitLineY) < 20 &&
                    !notes[i].hit)
                {
                    score += 15;
                    notes.RemoveAt(i);
                    return;
                }
            }
        }
        void CheckMissedNotes()
        {

            for (int i = notes.Count - 1; i >= 0; i--)
            {
                if (notes[i].y > virtualHeight - 50 && !notes[i].hit)
                {
                    score -= 10; // Penalize for missed note
                    notes.RemoveAt(i);
                }
            }

        }
    }
}
