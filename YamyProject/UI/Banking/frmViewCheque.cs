using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using static Guna.UI2.Native.WinApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace YamyProject
{
    public partial class frmViewCheque : Form
    {
        int id;
        int chq_book_no;
        bool isLoading = false;

        public frmViewCheque(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = id == 0 ? "New Check Book" : "Edit Check Book";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewCheque_Load(object sender, EventArgs e)
        {
            isLoading = true;

            DataTable dt = DBClass.ExecuteDataTable("SELECT distinct tb.name as name,tb.id as id  from tbl_bank_card tbc inner join tbl_bank tb on tbc.bank_id = tb.id");
            cmbBankName.ValueMember = "id";
            cmbBankName.DisplayMember = "name";
            cmbBankName.DataSource = dt;

            if (id > 0)
            {
                using (var reader = DBClass.ExecuteReader(@"SELECT 
                                    tbl_cheque.*, 
                                    tbl_bank_card.id AS bank_card_id,
                                    tbl_bank_card.bank_id
                                FROM 
                                    tbl_cheque
                                INNER JOIN 
                                    tbl_bank_card ON tbl_cheque.bank_card_id = tbl_bank_card.id
                                INNER JOIN 
                                    tbl_bank ON tbl_bank_card.bank_id = tbl_bank.id
                                WHERE 
                                    tbl_cheque.id = @id", DBClass.CreateParameter("id", id.ToString())))
                {
                    if (reader.Read())
                    {
                        txtChqBookNo.Text = reader["chq_book_no"].ToString();
                        txtChqBookQty.Text = reader["chq_book_qty"].ToString();
                        txtStartFrom.Text = reader["leaves_start_from"].ToString();
                        txtTO.Text = reader["leaves_end_in"].ToString();
                        cmbBankName.SelectedValue = reader["bank_id"].ToString();
                        cmbBankAccountName.SelectedValue = reader["bank_card_id"].ToString();

                        btnDelete.Enabled = true;

                        if (HasNextChequeBook())
                        {
                            txtChqBookNo.Enabled = false;
                            txtChqBookQty.Enabled = false;
                            txtStartFrom.Enabled = false;
                            txtTO.Enabled = false;
                            MessageBox.Show("This cheque book cannot be edited because a newer book already exists.");
                        }
                    }
                }
            }

            isLoading = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                if (updateChq())
                {
                    EventHub.RefreshCheck();
                    this.Close();
                }
            }
            else
            {
                if (insertChq())
                {
                    EventHub.RefreshCheck();
                    this.Close();
                }
            }
        }

        private bool insertChq()
        {
            if (!ChkValidation())
                return false;

            string query = @"
            INSERT INTO `tbl_cheque` (`bank_card_id`,`chq_book_no`, `chq_book_qty`, `leaves_start_from`, `leaves_end_in`, `created_by`, `created_date`) 
            VALUES (@bank_card_id, @chq_book_no, @chq_book_qty,  @leaves_start_from, @leaves_end_in, @created_by, @created_date);";

            DBClass.ExecuteNonQuery(query,
                DBClass.CreateParameter("@bank_card_id", cmbBankAccountName.SelectedValue),
                DBClass.CreateParameter("@chq_book_no", txtChqBookNo.Text),
                DBClass.CreateParameter("@chq_book_qty", txtChqBookQty.Text),
                DBClass.CreateParameter("@leaves_start_from", txtStartFrom.Text),
                DBClass.CreateParameter("@leaves_end_in", txtTO.Text),
                DBClass.CreateParameter("@created_by", frmLogin.userId),
                DBClass.CreateParameter("@created_date", DateTime.Now.ToString("yyyy-MM-dd"))
            );

            return true;
        }

        private bool updateChq()
        {
            if (!ChkValidation())
                return false;

            if (HasNextChequeBook())
            {
                MessageBox.Show("You cannot update this cheque book because a newer book exists.");
                return false;
            }

            string query = @"
            UPDATE `tbl_cheque` 
            SET 
                `bank_card_id` = @bank_card_id,
                `chq_book_no` = @chq_book_no,
                `chq_book_qty` = @chq_book_qty,
                `leaves_start_from` = @leaves_start_from,
                `leaves_end_in` = @leaves_end_in
            WHERE 
                `id` = @id;";

            DBClass.ExecuteNonQuery(query,
                DBClass.CreateParameter("@bank_card_id", cmbBankAccountName.SelectedValue),
                DBClass.CreateParameter("@chq_book_no", txtChqBookNo.Text),
                DBClass.CreateParameter("@chq_book_qty", txtChqBookQty.Text),
                DBClass.CreateParameter("@leaves_start_from", txtStartFrom.Text),
                DBClass.CreateParameter("@leaves_end_in", txtTO.Text),
                DBClass.CreateParameter("@id", id)
            );

            return true;
        }

        private bool ChkValidation()
        {
            if (cmbBankAccountName.SelectedValue == null)
            {
                MessageBox.Show("Choose Bank Account Name First");
                return false;
            }

            if (cmbBankName.SelectedValue == null)
            {
                MessageBox.Show("Choose Bank Name First");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtChqBookQty.Text))
            {
                MessageBox.Show("Enter Book QTY First");
                return false;
            }

            return true;
        }

        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBankName.SelectedValue == null)
                return;

            DataTable dt = DBClass.ExecuteDataTable("SELECT id,account_name FROM tbl_bank_card WHERE bank_id= " + cmbBankName.SelectedValue.ToString());
            cmbBankAccountName.DisplayMember = "account_name";
            cmbBankAccountName.ValueMember = "id";
            cmbBankAccountName.DataSource = dt;
        }

        private void checkqty()
        {
            if (int.TryParse(txtChqBookQty.Text, out int value))
            {
                if (value > 50)
                {
                    MessageBox.Show("Value cannot be greater than 50!");
                    txtChqBookQty.Text = "";
                    txtChqBookQty.Focus();
                }
            }
        }

        private void txtNumberOfLeaves_TextChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (!int.TryParse(txtChqBookQty.Text, out int qty))
            {
                txtTO.Text = "";
                return;
            }

            //if (qty > 50)
            //{
            //    MessageBox.Show("Value cannot be greater than 50!");
            //    txtChqBookQty.Text = "";
            //    txtTO.Text = "";
            //    return;
            //}

            if (id == 0 && cmbBankAccountName.SelectedValue != null)
            {
                // Get next starting cheque number and book no
                string nextStart = GenerateNextChequeCode();
                txtStartFrom.Text = nextStart;
                txtChqBookNo.Text = chq_book_no.ToString();
            }

            if (int.TryParse(txtStartFrom.Text, out int start))
            {
                txtTO.Text = (start + qty - 1).ToString();
            }
            else
            {
                txtTO.Text = "";
            }
            //if (int.TryParse(txtChqBookQty.Text, out int qty) && int.TryParse(txtStartFrom.Text, out int start))
            //{
            //    txtTO.Text = (start + qty - 1).ToString();
            //}
            //else
            //{
            //    txtTO.Text = "";
            //}
        }

        private string GenerateNextChequeCode()
        {
            int code;
            chq_book_no = 1;
            int nextStart = 1;

            //using (var reader = DBClass.ExecuteReader("SELECT MAX(leaves_end_in) AS lastCode, MAX(chq_book_no) AS lastBookNo FROM tbl_cheque WHERE bank_card_id = @id",
            //    DBClass.CreateParameter("id", cmbBankAccountName.SelectedValue.ToString())))
            //{
            //    if (reader.Read() && reader["lastCode"] != DBNull.Value)
            //    {
            //        code = int.Parse(reader["lastCode"].ToString()) + 1;
            //        chq_book_no = int.Parse(reader["lastBookNo"].ToString()) + 1;
            //    }
            //    else
            //    {
            //        code = 1;
            //        chq_book_no = 1;
            //    }
            //}

            //return code.ToString();

            // Get the cheque book with the highest chq_book_no for this bank card
            using (var reader = DBClass.ExecuteReader(@"
                SELECT chq_book_no, leaves_end_in 
                FROM tbl_cheque 
                WHERE bank_card_id = @id 
                ORDER BY chq_book_no DESC LIMIT 1",
                DBClass.CreateParameter("id", cmbBankAccountName.SelectedValue.ToString())))
            {
                if (reader.Read())
                {
                    int lastEnd = Convert.ToInt32(reader["leaves_end_in"]);
                    int lastBook = Convert.ToInt32(reader["chq_book_no"]);

                    nextStart = lastEnd + 1;
                    chq_book_no = lastBook + 1;
                }
            }

            return nextStart.ToString();
        }

        private bool HasNextChequeBook()
        {
            using (var reader = DBClass.ExecuteReader("SELECT COUNT(*) FROM tbl_cheque WHERE bank_card_id = @id AND chq_book_no > @currentBookNo",
                DBClass.CreateParameter("id", cmbBankAccountName.SelectedValue.ToString()),
                DBClass.CreateParameter("currentBookNo", txtChqBookNo.Text)))
            {
                if (reader.Read())
                {
                    return Convert.ToInt32(reader[0]) > 0;
                }
            }

            return false;
        }

        private void cmbBankAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbBankAccountName.SelectedValue == null)
                return;

            if (id == 0 && cmbBankAccountName.Focused)
            {
                txtStartFrom.Text = GenerateNextChequeCode();
                txtChqBookNo.Text = chq_book_no.ToString();

                if (int.TryParse(txtChqBookQty.Text, out int qty) && int.TryParse(txtStartFrom.Text, out int start))
                {
                    txtTO.Text = (start + qty - 1).ToString();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                using (var reader = DBClass.ExecuteReader("SELECT count(*) FROM tbl_check_details WHERE check_id = @id AND check_no = @bNo",
                    DBClass.CreateParameter("id", id), DBClass.CreateParameter("bNo", txtChqBookNo.Text)))
                {
                    if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                    {
                        MessageBox.Show("You cannot delete this check book because it is already used in bank transaction");
                        return;
                    }
                }

                DBClass.ExecuteNonQuery("DELETE FROM tbl_cheque WHERE id = @id", DBClass.CreateParameter("id", id));
                EventHub.RefreshCheck();
                this.Close();
            }
        }
    }

    //public partial class frmViewCheque : Form
    //{

    //    int id;
    //    public frmViewCheque(int id = 0)
    //    {
    //        InitializeComponent();
    //        LocalizationManager.LocalizeForm(this);
    //        this.id = id;
    //        headerUC1.FormText = id == 0 ? "New Check Book" : "Edit Check Book";
    //    }
    //    private void btnClose_Click(object sender, EventArgs e)
    //    {
    //        this.Close();
    //    }
    //    private void frmViewCheque_Load(object sender, EventArgs e)
    //    {
    //        DataTable dt = DBClass.ExecuteDataTable("SELECT distinct tb.name as name,tb.id as id  from tbl_bank_card tbc inner join tbl_bank tb on tbc.bank_id = tb.id");
    //        cmbBankName.ValueMember = "id";
    //        cmbBankName.DisplayMember = "name";
    //        cmbBankName.DataSource = dt;


    //        if (id > 0)
    //        {
    //            using (var reader = DBClass.ExecuteReader(@"SELECT 
    //                                    tbl_cheque.*, 
    //                                    tbl_bank_card.id AS bank_card_id,
    //                                    tbl_bank_card.bank_id
    //                                FROM 
    //                                    tbl_cheque
    //                                INNER JOIN 
    //                                    tbl_bank_card ON tbl_cheque.bank_card_id = tbl_bank_card.id
    //                                INNER JOIN 
    //                                    tbl_bank ON tbl_bank_card.bank_id = tbl_bank.id
    //                                WHERE 
    //                                    tbl_cheque.id = @id", DBClass.CreateParameter("id", id.ToString())))
    //            {
    //                if (reader.Read())
    //                {
    //                    txtChqBookNo.Text = reader["chq_book_no"].ToString();
    //                    txtChqBookQty.Text = reader["chq_book_qty"].ToString();
    //                    txtStartFrom.Text = reader["leaves_start_from"].ToString();
    //                    txtTO.Text = reader["leaves_end_in"].ToString();
    //                    cmbBankName.SelectedValue = reader["bank_id"].ToString();
    //                    cmbBankAccountName.SelectedValue = reader["bank_card_id"].ToString();

    //                    btnDelete.Enabled = true;
    //                }
    //            }
    //        }
    //    }
    //    private void btnSave_Click(object sender, EventArgs e)
    //    {
    //        if (id > 0)
    //        {
    //            if (updateChq())
    //            {
    //                EventHub.RefreshCheck();
    //                this.Close();
    //            }
    //        }
    //        else
    //        {
    //            if (insertChq())
    //            {
    //                EventHub.RefreshCheck();
    //                this.Close();
    //            }
    //        }
    //    }

    //    private bool insertChq()
    //    {
    //        if (!ChkValidation())
    //            return false;

    //        string query = @"
    //                        INSERT INTO `tbl_cheque` (`bank_card_id`,`chq_book_no`, `chq_book_qty`, `leaves_start_from`, `leaves_end_in`, `created_by`, `created_date`) 
    //                        VALUES (@bank_card_id, @chq_book_no, @chq_book_qty,  @leaves_start_from, @leaves_end_in, @created_by, @created_date);";
    //        DBClass.ExecuteNonQuery(query,
    //            DBClass.CreateParameter("@bank_card_id", cmbBankAccountName.SelectedValue),
    //            DBClass.CreateParameter("@chq_book_no", txtChqBookNo.Text),
    //            DBClass.CreateParameter("@chq_book_qty", txtChqBookQty.Text),
    //            DBClass.CreateParameter("@leaves_start_from", int.Parse(txtStartFrom.Text).ToString("")),
    //            DBClass.CreateParameter("@leaves_end_in", int.Parse(txtTO.Text).ToString("")),
    //            DBClass.CreateParameter("@created_by", frmLogin.userId),
    //            DBClass.CreateParameter("@created_date", DateTime.Now.ToString("yyyy-MM-dd"))
    //        );
    //        return true;
    //    }
    //    private bool updateChq()
    //    {
    //        if (!ChkValidation())
    //            return false;

    //        string query = @"
    //                        UPDATE `tbl_cheque` 
    //                        SET 
    //                            `bank_card_id` = @bank_card_id,
    //                            `chq_book_no` = @chq_book_no,
    //                            `chq_book_qty` = @chq_book_qty,
    //                            `leaves_start_from` = @leaves_start_from,
    //                            `leaves_end_in` = @leaves_end_in
    //                        WHERE 
    //                            `id` = @id;
    //                    ";

    //        DBClass.ExecuteNonQuery(query,
    //            DBClass.CreateParameter("@bank_card_id", cmbBankAccountName.SelectedValue),
    //            DBClass.CreateParameter("@chq_book_no", txtChqBookNo.Text),
    //            DBClass.CreateParameter("@chq_book_qty", txtChqBookQty.Text),
    //            DBClass.CreateParameter("@leaves_start_from", int.Parse(txtStartFrom.Text).ToString("")),
    //            DBClass.CreateParameter("@leaves_end_in", int.Parse(txtTO.Text).ToString("")),
    //            DBClass.CreateParameter("@id", id)
    //        );

    //        return true;
    //    }

    //    private bool ChkValidation()
    //    {
    //        if (cmbBankAccountName.SelectedValue == null)
    //        {
    //            MessageBox.Show("Choose Bank Account Name First");
    //            return false;
    //        }

    //        if (cmbBankName.SelectedValue == null)
    //        {
    //            MessageBox.Show("Choose Bank Name First");
    //            return false;
    //        }
    //        if (txtChqBookQty.Text.Trim() == "")
    //        {
    //            MessageBox.Show("Enter Book QTY First");
    //            return false;
    //        }
    //        return true;
    //    }


    //    private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (cmbBankName.SelectedValue == null)
    //            return;
    //        DataTable dt = DBClass.ExecuteDataTable("SELECT id,account_name  from tbl_bank_card WHERE bank_id= " + cmbBankName.SelectedValue.ToString());
    //        cmbBankAccountName.DisplayMember = "account_name";
    //        cmbBankAccountName.ValueMember = "id";
    //        cmbBankAccountName.DataSource = dt;

    //    }

    //    private void checkqty()
    //    {
    //        int value;
    //        if (int.TryParse(txtChqBookQty.Text, out value))
    //        {
    //            if (value > 50)
    //            {
    //                MessageBox.Show("Value cannot be greater than 50!");
    //                txtChqBookQty.Text = "";
    //                txtChqBookQty.Focus();
    //            }
    //        }
    //    }

    //    private void txtNumberOfLeaves_TextChanged(object sender, EventArgs e)
    //    {
    //        if (txtChqBookQty.Text.Trim() != "" && txtStartFrom.Text.Trim() != "")
    //            txtTO.Text = (int.Parse(txtStartFrom.Text) + int.Parse(txtChqBookQty.Text) - 1).ToString();
    //    }
    //    int chq_book_no;
    //    private string GenerateNextChequeCode()
    //    {
    //        int code;
    //        using (var reader = DBClass.ExecuteReader("select max(leaves_end_in) as lastCode ,chq_book_no from tbl_cheque where bank_card_id =@id GROUP BY chq_book_no ",
    //            DBClass.CreateParameter("id", cmbBankAccountName.SelectedValue.ToString())))
    //        {
    //            if (reader.Read() && reader["lastCode"] != DBNull.Value)
    //            {
    //                code = int.Parse(reader["lastCode"].ToString()) + 1;
    //                chq_book_no = int.Parse(reader["chq_book_no"].ToString()) + 1;
    //            }
    //            else
    //            {
    //                code = 1;
    //                chq_book_no = 1;
    //            }
    //        }
    //        return code.ToString("");
    //    }
    //    private void cmbBankAccountName_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (cmbBankAccountName.SelectedValue == null)
    //            return;

    //        if (!cmbBankAccountName.Focused) return;

    //        txtStartFrom.Text = GenerateNextChequeCode();
    //        txtChqBookNo.Text = chq_book_no.ToString();
    //        if (txtChqBookQty.Text.Trim() != "")
    //            txtTO.Text = (int.Parse(txtStartFrom.Text) + int.Parse(txtChqBookQty.Text)).ToString("");
    //    }

    //    private void btnDelete_Click(object sender, EventArgs e)
    //    {
    //        var confirmResult = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);
    //        if (confirmResult == DialogResult.Yes)
    //        {
    //            using (var reader = DBClass.ExecuteReader("SELECT count(*) FROM tbl_check_details WHERE check_id = @id AND check_no = @bNo", DBClass.CreateParameter("id", id), DBClass.CreateParameter("bNo", txtChqBookNo.Text)))
    //            {
    //                if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
    //                {
    //                    MessageBox.Show("You cannot delete this check book because it is already used in bank transaction");
    //                    return;
    //                }
    //            }
    //            DBClass.ExecuteNonQuery("DELETE FROM tbl_cheque WHERE id = @id", DBClass.CreateParameter("id", id));
    //            EventHub.RefreshCheck();
    //            this.Close();
    //        }
    //    }
    //}
}
