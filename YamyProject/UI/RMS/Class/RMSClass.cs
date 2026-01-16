using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using System.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Bibliography;
using YamyProject.UI.CRM;
using YamyProject.UI.Manufacturing;

namespace YamyProject.RMS.Class
{
    internal class RMSClass
    {
        private static string GetConnectionString()
        {
            string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);

            string connectionString = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);
            var builder = new MySqlConnectionStringBuilder(connectionString);
            builder.Database = DBClass.Database;
            return builder.ToString();
        }
        public static MySqlConnection conn()
        {
            MySqlConnection con = new MySqlConnection(GetConnectionString());
            return con;
        }
        public static int SQl(string qry1, Hashtable ht)
        {
            int res = 0;
            MySqlCommand cmd = new MySqlCommand(qry1, conn());

            try
            {
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);


                }
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                //if (conn().State == ConnectionState.Closed) { conn().Open(); }
                res = cmd.ExecuteNonQuery();

                //if (conn().State == ConnectionState.Open) { conn().Close(); }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                conn().Close();
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return res;

        }

        public static void loadData(string qry1, DataGridView gv, System.Windows.Forms.ListBox lb)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry1, conn());
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                }


                gv.DataSource = dt;


            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }


        public static void blurbackground(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMainRMS.Instance.Size;
                Background.Location = frmMainRMS.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();

            }
        }
        public static void blurbackground2(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmCRMmain.Instance.Size;
                Background.Location = frmCRMmain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();

            }
        }
        public static void blurbackground3(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMainManufacturing.Instance.Size;
                Background.Location = frmMainManufacturing.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();

            }
        }


        public static void CBFILL(string qry, ComboBox cb)
        {
            MySqlCommand cmd = new MySqlCommand(qry, conn());
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }


    }
}
