using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Settings.Color_theme
{
    public partial class frmColorThempickercs : Form
    {
        string idst = "";
        int id = 0;
        string headercolor = "";
        string Textcolor = "";
        public frmColorThempickercs()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmColorThempickercs_Load(object sender, EventArgs e)
        {
            //COLORPICKERHEADER.Text = "Red";
            //COLORPICKERTEXT.Text = "White";
            colorheaderload();

        }
        private void colorheaderload()
        {
            using (var reader = DBClass.ExecuteReader("SELECT id,headerColor,TextColor FROM tbl_color"))
            {
                if (reader.Read() && reader["id"] != DBNull.Value)
                {

                    idst = reader["headerColor"].ToString();
                    headercolor = reader["headerColor"].ToString();
                    Textcolor = reader["TextColor"].ToString();
                   COLORPICKERHEADER.Text = headercolor;
                   COLORPICKERTEXT.Text = Textcolor;

                    id = Convert.ToInt32(reader["id"]);

                    panel6.BackColor = System.Drawing.Color.FromName(COLORPICKERHEADER.Text);
                    Lbheader.ForeColor = System.Drawing.Color.FromName(COLORPICKERTEXT.Text);

                }
                else
                COLORPICKERHEADER.Text = "RoyalBlue";
                COLORPICKERTEXT.Text = "White";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string qry1 = "";

            if (id == 0)
            {
                qry1 = "INSERT INTO `tbl_color`(id,headerColor,TextColor) VALUES (1,@headerColor,@TextColor);";
            }
            else
            {
                qry1 = "Update  tbl_color set headerColor = @headerColor , TextColor = @TextColor  where id= 1 ";

            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@headerColor", COLORPICKERHEADER.Text);
            ht.Add("@TextColor", COLORPICKERTEXT.Text);
            if (RMSClass.SQl(qry1, ht) > 0)
            {
                //MessageBox.Show("save Successfully...");
            }

            colorheaderload();
        }

      private void colorload ()
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            int id = 1;
            //string qry1 = "Delete from tbl_color where id= " + id + "";
            //Hashtable ht = new Hashtable();
            //RMSClass.SQl(qry1, ht);
            //MessageBox.Show("Delete Successfully");
            //colorheaderload();
        }
    }
}

public class MyColorTable : ProfessionalColorTable
{
    public override Color MenuItemSelected => Color.LightSkyBlue;
    public override Color MenuItemSelectedGradientBegin => Color.DodgerBlue;
    public override Color MenuItemSelectedGradientEnd => Color.LightBlue;

    public override Color MenuItemPressedGradientBegin => Color.RoyalBlue;
    public override Color MenuItemPressedGradientEnd => Color.SkyBlue;

    public override Color MenuItemBorder => Color.Navy;
}

public class MyRenderer : ToolStripProfessionalRenderer
{
    public MyRenderer() : base(new MyColorTable()) { }
}
