namespace rythmgame
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
        List<Note> notes = new List<Note>();
        int virtualWidth = 800;
        int virtualHeight = 600;

        float scaleX = 1f;
        float scaleY = 1f;
        int score = 0;

        public Form1()
        {
            InitializeComponent();

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

            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            
            notes.Add(new Note { lane = 0, y = 0 });
            notes.Add(new Note { lane = 1, y = -100 });
            notes.Add(new Note { lane = 2, y = -200 });
            notes.Add(new Note { lane = 3, y = -300 });
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

        void GameLoop(object sender, EventArgs e)
        {
            // Game logic and rendering code goes here
            Input();
            foreach (var note in notes)
            {
                note.y += note.speed;
            }
            CheckMissedNotes();
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(offsetX, offsetY);
            g.ScaleTransform(scaleX, scaleY);

            foreach (var note in notes)
            {
                g.FillRectangle(Brushes.Blue,
                    note.lane * virtualWidth/4,
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
            foreach(var note in notes)
            {
                if(note.lane == lane && Math.Abs(note.y - hitLineY) < 20 && !note.hit)
                {
                    note.hit = true;
                    score += 15;
                    notes.Remove(note);
                    return;
                }
            }
        }
        void CheckMissedNotes()
        {
            foreach(var note in notes)
            {
                if(note.y > this.ClientSize.Height && !note.hit)
                {
                    score -= 10; // Penalize for missed note
                }
            }
        }
    }
}
