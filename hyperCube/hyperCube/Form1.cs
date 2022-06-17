using System.Diagnostics;
using System.Data.SQLite;


namespace hyperCube
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Stopwatch stopwatch = new Stopwatch();

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateHighscore();

        }

        private void UpdateHighscore()
        {
            lstScores.Items.Clear();
            string cs = @"URI=file:highscore.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM score ORDER BY time DESC";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                lstScores.Items.Add($"{rdr.GetString(1)}" + "         |      " + $"{rdr.GetString(0)}");
            }


        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            stopwatch.Start();
            btnEnd.Focus();
        }


        private void btnEnd_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            btnStart.Focus();
            
        }




        private void myTimer_Tick_1(object sender, EventArgs e)
        {
            txtTime.Text = String.Format("{0:mm\\:ss\\:ff}", stopwatch.Elapsed);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnStart.Focus();
            string cs = @"URI=file:highscore.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            string timestring = DateTime.Now.ToString();

            cmd.CommandText = "INSERT INTO score(date,time) VALUES (?,?)";
            
            cmd.Parameters.AddWithValue("@p1", DateTime.Now.ToShortDateString());
            cmd.Parameters.AddWithValue("@p2", string.Format(txtTime.Text));

            cmd.ExecuteNonQuery();
            UpdateHighscore();


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnStart.Focus();
            string cs = @"URI=file:highscore.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM score";
            cmd.ExecuteNonQuery();
            UpdateHighscore();
        }
    }
}