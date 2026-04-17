namespace rythmgame
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
        List<Note> notes = new List<Note>();

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            notes.Add(new Note { lane = 0, y = 0 });
            notes.Add(new Note { lane = 1, y = -100 });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void GameLoop(object sender, EventArgs e)
        {
            // Game logic and rendering code goes here
            foreach(var note in notes)
            {
                note.y += note.speed;
            }
            CheckMissedNotes();
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var note in notes)
            {
                g.FillRectangle(Brushes.Blue,
                    note.Lane * 100,
                    (int)note.Y,
                    80,
                    20);
            }

            // Draw hit line
            g.DrawLine(Pens.Red, 0, 400, 400, 400);
        }



        void CheckMissedNotes()
        {
            foreach(var note in notes)
            {
                if(note.y > this.ClientSize.Height && !note.hit)
                {
                    // Handle missed note (e.g., decrease score, show feedback)
                }
            }
        }
    }
}
