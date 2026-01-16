using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;
using YamyProject.RMS;
using YamyProject.UI.Construction.ProjectPlanning;
using YamyProject.UI.CRM;
using YamyProject.UI.Default;
using YamyProject.UI.Default.Dashboard;
using YamyProject.UI.Manufacturing;
using YamyProject.UI.Reports;
using YamyProject.UI.Reports.Bank;
using YamyProject.UI.Reports.Construction;
using YamyProject.UI.Settings;
using YamyProject.UI.Settings.Color_theme;
using YamyProject.UI.Test;
using YamyProject.UI.VAT;

namespace YamyProject
{
    public partial class MainForm : Form
    {
        bool toLogin = false;
        private bool isDragging = false;
        private Point dragStartPoint;
        bool IsClick = false;
        private Dictionary<string, List<SubMenu>> menuData = new Dictionary<string, List<SubMenu>>();
        EventHandler roleEvent;
        public MainForm()
        {
            InitializeComponent();
            //ApplyTheme(this);

            menuStrip1.Renderer = new MyRenderer();
            LocalizationManager.LocalizeForm(this);
            roleEvent = (sender, args) => LoadUserPermissions(frmLogin.RoleId);
            EventHub.Roles += roleEvent;
            LoadAllMenuData();
        }
        //private void ApplyTheme(Control parent)
        //{
        //    parent.BackColor = AppTheme.BackgroundColor;
        //    parent.ForeColor = AppTheme.TextColor;

        //    foreach (Control ctrl in parent.Controls)
        //    {
        //        switch (ctrl)
        //        {
        //            case Button btn:
        //                btn.BackColor = AppTheme.ButtonBackColor;
        //                btn.ForeColor = AppTheme.ButtonForeColor;
        //                btn.FlatStyle = FlatStyle.Flat;
        //                btn.FlatAppearance.BorderSize = 0;

        //                btn.MouseEnter += (s, e) => btn.BackColor = AppTheme.ButtonHoverColor;
        //                btn.MouseLeave += (s, e) => btn.BackColor = AppTheme.ButtonBackColor;
        //                break;

        //            case Guna2Button gunaBtn:
        //                gunaBtn.BackColor = AppTheme.ButtonBackColor;
        //                gunaBtn.ForeColor = AppTheme.ButtonForeColor;

        //                gunaBtn.MouseEnter += (s, e) => gunaBtn.BackColor = AppTheme.ButtonHoverColor;
        //                gunaBtn.MouseLeave += (s, e) => gunaBtn.BackColor = AppTheme.ButtonBackColor;
        //                break;

        //            case Panel pnl:
        //                pnl.BackColor = AppTheme.PanelColor; // Add PanelColor in AppTheme
        //                break;

        //            case Label lbl:
        //                lbl.ForeColor = AppTheme.TextColor;
        //                break;

        //            case TextBox txt:
        //                txt.BackColor = AppTheme.InputBackgroundColor; // Optional
        //                txt.ForeColor = AppTheme.TextColor;
        //                break;

        //            case Guna2TextBox gunaTxt:
        //                gunaTxt.FillColor = AppTheme.InputBackgroundColor;
        //                gunaTxt.ForeColor = AppTheme.TextColor;
        //                break;
        //        }

        //        // Apply recursively to all child controls
        //        if (ctrl.HasChildren)
        //        {
        //            ApplyTheme(ctrl);
        //        }
        //    }
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                // Find the last non-main, visible, open form
                foreach (Form openForm in Application.OpenForms.Cast<Form>().Reverse())
                {
                    if (openForm != this && openForm.Visible && !openForm.IsDisposed)
                    {
                        openForm.Close();
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadUserPermissions(int _userId)
        {
            //object netResult = DBClass.ExecuteScalar(
            //    "SELECT count(*) FROM tbl_user_permissions WHERE user_id = @user_id",
            //    DBClass.CreateParameter("user_id", _userId)
            //);

            //int count = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;

            //if (count > 0)
            //{
            //    DataTable userTable = DBClass.ExecuteDataTable(
            //        "SELECT p.id, p.user_id, s.id AS sub_menu_id, s.name AS sub_menu_name, m.id AS main_menu_id, m.name AS main_menu_name, p.can_view, p.can_edit, p.can_delete FROM tbl_user_permissions p JOIN tbl_sub_menus s ON p.sub_menu_id = s.id JOIN tbl_main_menus m ON m.id = s.m_id WHERE p.user_id = @user_id",
            //        DBClass.CreateParameter("user_id", _userId)
            //    );

            //    UserPermissions.LoadPermissions(userTable);
            //}
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    guna2HtmlLabel3.Text = reader["name"].ToString();
                }
            }
        }
        //public Form addcontrols2(Form f)
        //{
        //    pnlMain.Controls.Clear();
        //    //f.Dock = DockStyle.Fill;
        //    f.TopLevel = false;
        //    pnlMain.Controls.Add(f);
        //    f.Show();
        //    return f;
        //}
        //public Form LoadFormIntoPanel(Form f)
        //{
        //    //pnlMain.Controls.Clear();

        //    f.TopLevel = false;
        //    f.FormBorderStyle = FormBorderStyle.None;

        //    // Set manual location (disable Docking)
        //    f.StartPosition = FormStartPosition.Manual;

        //    // Calculate center position
        //    int x = (pnlMain.Width - f.Width) / 2;
        //    int y = (pnlMain.Height - f.Height) / 2;
        //    f.Location = new Point(x, y);

        //    pnlMain.Controls.Add(f);
        //    f.BringToFront(); 
        //    f.Show();

        //    return f;
        //}
        public Form LoadFormIntoPanel(Form f)
        {
            // Check if the same form type already exists
            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form && control.GetType() == f.GetType())
                {
                    Form existingForm = (Form)control;
                    if (existingForm.WindowState == FormWindowState.Minimized && existingForm.Tag != null)
                    {
                        existingForm.WindowState = FormWindowState.Normal;
                    }
                    existingForm.BringToFront();
                    return existingForm;
                }
            }

            // Prepare new form
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;

            // Center it in the panel
            int x = (pnlMain.Width - f.Width) / 2;
            int y = (pnlMain.Height - f.Height) / 2;
            f.Location = new Point(x, y);

            // Add and show it
            pnlMain.Controls.Add(f);
            f.BringToFront();
            f.Show();

            return f;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //pnlLeft.Size = new Size(160, 704);
            btnUser.Text = frmLogin.userFName + "  ▼";
            btnFullUser.Text = frmLogin.userFName + " " + frmLogin.userLName;

            // Ensure the form does not cover the taskbar
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            //AdjustFormSize();
            BindDataTable.FetchData();
            BindCombos.ClearCache();
            loadCompany();

            string savedCulture = Properties.Settings.Default.ApplicationLanguage ?? "en";
            btnLanguage.Text = savedCulture;

            // default form
            openChildFormX(new FrmDashboard());

            // Hide Modules based on permissions
            //BtnCRM.Visible = BtnManufacture.Visible = BtnRestaurant.Visible = true;
            BtnCRM.Visible = Utilities.GetModules("CRM");
            BtnManufacture.Visible = Utilities.GetModules("Manufacture");
            BtnRestaurant.Visible = Utilities.GetModules("Restaurant");
            constructionToolStripMenuItem.Visible = Utilities.GetModules("Construction");

            // do a function call after 2 seconds
            var timer = new Timer { Interval = 2000 };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                NavigationBar();
            };
            timer.Start();
        }
        public void openChildFormX(Form childForm)
        {
            // Optional: Close existing same-type child form
            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form && control.GetType() == childForm.GetType())
                {
                    Form existingForm = (Form)control;
                    existingForm.Close(); // or BringToFront(), depending on your needs
                }
            }

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill; // Make it fill the panel

            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void LoadAllMenuData()
        {
            //guna2Button12.Visible = true;
            //constructionToolStripMenuItem.Visible = false;
            if (UserPermissions.ViewPermissions.Count>0)
            {
                toolStripMenuItem5.Visible = UserPermissions.canView("Company List");
                backUpCompanyToolStripMenuItem.Visible = UserPermissions.canView("Back Up Company");
                restoreCompanyToolStripMenuItem.Visible = UserPermissions.canView("Restore Company");
                chartOfAccountToolStripMenuItem.Visible = toolStripMenuItem2 .Visible = UserPermissions.canView("Chart Of Account");
                damageToolStripMenuItem.Visible = vatToolStripMenuItem.Visible = UserPermissions.canView("Inventory Items");
                //itemCategoryListToolStripMenuItem.Visible = UserPermissions.canView("");
                //itemVatListToolStripMenuItem.Visible = UserPermissions.canView("");
                //itemUnitListToolStripMenuItem.Visible = UserPermissions.canView("");
                //salesTaxCodeListToolStripMenuItem.Visible = UserPermissions.canView("");
                costCenterToolStripMenuItem.Visible = UserPermissions.canView("Cost Center");
                journalTransactionsToolStripMenuItem.Visible = UserPermissions.canView("Transactions Journal");
                fixedAssetsItemListToolStripMenuItem.Visible = UserPermissions.canView("Fixed Assets");
                payBillsToolStripMenuItem1.Visible = UserPermissions.canView("Vouchers");
                prepaidExpenseToolStripMenuItem.Visible = UserPermissions.canView("Prepaid Expense");
                pettyCashToolStripMenuItem.Visible = UserPermissions.canView("Petty Cash");
                usersToolStripMenuItem.Visible = btnCompanyProfile.Visible = companyToolStripMenuItem1 .Visible= UserPermissions.canView("Company");
                inventoryCenterToolStripMenuItem1.Visible = itemListToolStripMenuItem.Visible = inventoryToolStripMenuItem2.Visible = UserPermissions.canView("Inventory Center");
                stockManagementToolStripMenuItem.Visible = UserPermissions.canView("Stock Management");
                warehouseToolStripMenuItem.Visible = UserPermissions.canView("Warehouse Center");
                customerCenterToolStripMenuItem.Visible = btnCustomer.Visible = UserPermissions.canView("Customer Center");
                salesCenterToolStripMenuItem.Visible = UserPermissions.canView("Sales Center");
                createInvoiceToolStripMenuItem.Visible = UserPermissions.canView("Create Invoice");
                receivePaymentsToolStripMenuItem.Visible = UserPermissions.canView("Receipt Voucher");
                creditNoteToolStripMenuItem.Visible = UserPermissions.canView("Credit Note");
                quotationToolStripMenuItem.Visible = UserPermissions.canView("Quotation");
                salesOrderToolStripMenuItem.Visible = UserPermissions.canView("Sales Order");
                salesReturnToolStripMenuItem.Visible = UserPermissions.canView("Sales Return");
                salesProformaToolStripMenuItem.Visible = UserPermissions.canView("Sales Proforma");
                vendorCenterToolStripMenuItem.Visible = btnVendor.Visible = UserPermissions.canView("Vendor Center");
                purchasesCenterToolStripMenuItem.Visible = UserPermissions.canView("Purchases Center");
                purchaseReceiptToolStripMenuItem.Visible = UserPermissions.canView("Create Purchases");
                payBillsToolStripMenuItem.Visible = UserPermissions.canView("Payment Voucher");
                debitNoteToolStripMenuItem1.Visible = UserPermissions.canView("Debit Note");
                purchaseOrderToolStripMenuItem.Visible = UserPermissions.canView("Purchase Order");
                purchaseReturnToolStripMenuItem.Visible = UserPermissions.canView("Purchase Return");
                employeeCenterToolStripMenuItem.Visible = btnHr.Visible = UserPermissions.canView("Human Resource Center");
                attendanceSheetToolStripMenuItem.Visible = UserPermissions.canView("Attendance Sheet");
                salarySheetToolStripMenuItem.Visible = UserPermissions.canView("Salary Sheet");
                leaveSalaryToolStripMenuItem.Visible = UserPermissions.canView("Leave Salary");
                endOfServicesToolStripMenuItem.Visible = UserPermissions.canView("End Of Services");
                loansToolStripMenuItem.Visible = UserPermissions.canView("Loans");
                finalSettlementToolStripMenuItem.Visible = UserPermissions.canView("Final Settlement");
                paymentToolStripMenuItem.Visible = UserPermissions.canView("Payment Voucher");
                bankCenterToolStripMenuItem.Visible = btnBank.Visible = UserPermissions.canView("Bank Center");
                openBankCardToolStripMenuItem.Visible = UserPermissions.canView("Open Bank Card");
                chequesToolStripMenuItem.Visible = UserPermissions.canView("Cheques");
                pDCToolStripMenuItem.Visible = UserPermissions.canView("PDC");
                customerToolStripMenuItem1.Visible = UserPermissions.canView("Customer & Receivable");
                salesToolStripMenuItem.Visible = UserPermissions.canView("Sales");
                vendorToolStripMenuItem1.Visible = UserPermissions.canView("Vendor & Payable");
                purchaseToolStripMenuItem.Visible = UserPermissions.canView("Purchases");
                employeeToolStripMenuItem.Visible = UserPermissions.canView("Employees");
                accountantToolStripMenuItem1.Visible = btnAccount.Visible = UserPermissions.canView("Accountant");
                inventoryToolStripMenuItem.Visible = btnInventory.Visible = UserPermissions.canView("Inventory");
                listToolStripMenuItem.Visible = UserPermissions.canView("List");
                settingsToolStripMenuItem1.Visible = guna2Button8.Visible = UserPermissions.canView("Setting");
                changeCurrentPasswordToolStripMenuItem.Visible = UserPermissions.canView("Change Current Password");
                clearDataToolStripMenuItem.Visible = UserPermissions.canView("Clear Data");
                usersToolStripMenuItem1.Visible = UserPermissions.canView("Users");
                projectDashBoardToolStripMenuItem.Visible = UserPermissions.canView("Project DashBoard");
                projectToolStripMenuItem.Visible = UserPermissions.canView("Project Tender");
                projectEstimateToolStripMenuItem.Visible = UserPermissions.canView("Project Estimate");
                projectToolStripMenuItem1.Visible = UserPermissions.canView("Project Planning");
                //guna2Button5.Visible = UserPermissions.canView("No");
                //guna2Button11.Visible = UserPermissions.canView("No");
                if (frmLogin.RoleId == 1)
                {
                    frmAuditTrailToolStripMenuItem.Visible = true;
                }
            }
        }

        // Method to adjust the form size
        //private void AdjustFormSize()
        //{
        //    Screen screen = Screen.PrimaryScreen; // Get primary screen
        //    Rectangle workingArea = screen.WorkingArea; // Get area excluding the taskbar

        //    // Apply new size and position
        //    this.Size = new Size(workingArea.Width, workingArea.Height);
        //    this.Location = new Point(workingArea.Left, workingArea.Top);
        //}


        //private void CascadeAllForms()
        //{
        //    int offsetX = 30;
        //    int offsetY = 30;

        //    foreach (Form form in Application.OpenForms)
        //        if (pnlMain.Controls.Contains(form))
        //        {
        //            form.Dock = DockStyle.None;
        //            form.Location = new Point(offsetX, offsetY);
        //            offsetX += 30;
        //            offsetY += 30;
        //            form.Height = 650;
        //            form.Width = 850;
        //        }
        //}
        //private void ShowFormsSideBySide()
        //{
        //    int offsetX = 0;
        //    int offsetY = 0;

        //    foreach (Form form in Application.OpenForms)
        //    {
        //        if (pnlMain.Controls.Contains(form))
        //        {
        //            form.Dock = DockStyle.None;
        //            form.Height = 500;
        //            form.Width = 650;
        //            form.Location = new Point(offsetX, offsetY);
        //            offsetX += form.Width;
        //        }
        //        if (offsetX >= pnlMain.Width)
        //        {
        //            offsetX = 0;
        //            offsetY = form.Height;
        //        }
        //    }
        //}

        public void openChildForm(Form childForm)
        {
            // Check if form of same type already exists
            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form existingForm && control.GetType() == childForm.GetType())
                {
                    existingForm.BringToFront();
                    return; // Don't open a new instance
                }
            }

            // Configure the child form
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        public void openChildFormOld(Form childForm)
        {
            foreach (Control control in pnlMain.Controls)
            {
                if (control is Form && control.GetType() == childForm.GetType())
                {
                    Form existingForm = (Form)control;
                    //if (existingForm.WindowState == FormWindowState.Minimized && existingForm.Tag != null)
                    //{
                    //    existingForm.WindowState = FormWindowState.Normal;
                    //    existingForm.BringToFront();
                    //    return;
                    //}
                    //else
                    //    existingForm.Close();
                }
            }

            childForm.TopLevel = false;
            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;
            if (childForm.Tag != null && childForm.Tag.ToString() != "")
                childForm.Dock = DockStyle.Fill;
            else
            {
                childForm.Dock = DockStyle.None;
                childForm.Location = new Point(frmLogin.frmMain.Width / 2 - childForm.Width / 2, frmLogin.frmMain.Height / 2 - childForm.Height / 2);

            }
            childForm.BringToFront();


            //MakeFormDraggable(childForm);

            childForm.Show();

        }
        //private void MakeFormDraggable(Form childForm)
        //{
        //    AttachMouseEvents(childForm, childForm);

        //    childForm.MouseDown += (s, e) =>
        //    {
        //        //if (e.Button == MouseButtons.Left)
        //        //{
        //        //    isDragging = true;
        //        //    dragStartPoint = e.Location;
        //        //}
        //    };

        //    childForm.MouseMove += (s, e) =>
        //    {
        //        //if (isDragging)
        //        //{
        //        //    childForm.Left += e.X - dragStartPoint.X;
        //        //    childForm.Top += e.Y - dragStartPoint.Y;
        //        //}
        //    };

        //    childForm.MouseUp += (s, e) =>
        //    {
        //        if (e.Button == MouseButtons.Left)
        //            isDragging = false;
        //    };
        //    childForm.Click += (s, e) =>
        //    {
        //        childForm.BringToFront();
        //    };
        //}
        //private void AttachMouseEvents(Control control, Form childForm)
        //{
        //    if (control is TextBox || control is Button || control is Guna.UI2.WinForms.Guna2DataGridView || control is Guna.UI2.WinForms.Guna2GroupBox || control is System.Windows.Forms.DateTimePicker || control is Guna.UI2.WinForms.Guna2ComboBox)
        //        return;

        //    control.MouseDown += (s, e) =>
        //    {
        //        if (e.Button == MouseButtons.Left)
        //        {
        //            isDragging = true;
        //            dragStartPoint = e.Location;
        //            childForm.BringToFront();
        //        }
        //    };

        //    control.MouseMove += (s, e) =>
        //    {
        //        if (isDragging)
        //        {
        //            childForm.Left += e.X - dragStartPoint.X;
        //            childForm.Top += e.Y - dragStartPoint.Y;
        //        }
        //    };

        //    control.MouseUp += (s, e) =>
        //    {
        //        if (e.Button == MouseButtons.Left)
        //            isDragging = false;
        //    };

        //    foreach (Control child in control.Controls)
        //        AttachMouseEvents(child, childForm);
        //}
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!toLogin)
                Application.Exit();
            EventHub.Roles -= roleEvent;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnMazimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            
            else
                this.WindowState = FormWindowState.Normal;
        }
        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in pnlMain.Controls)
                if (ctrl is Form)
                    ((Form)ctrl).Close();
        }

        private void btnCascade_Click(object sender, EventArgs e)
        {
            //CascadeAllForms();
        }
        private void btnSideBySide_Click(object sender, EventArgs e)
        {
            //ShowFormsSideBySide();
        }
        private void btnMain_MouseHover(object sender, EventArgs e)
        {
            UpdateSubMenuPositionAndLoad((Guna2Button)sender);
        }
        private void guna2Button8_MouseEnter(object sender, EventArgs e)
        {
            UpdateSubMenuPositionAndLoad((Guna2Button)sender);
        }
        //private void ToggleSubMenu(Guna2Button button)
        //{
        //    if (IsClick)
        //    {
        //        pnlSub.Visible = false;
        //        IsClick = false;
        //    }
        //    else
        //    {
        //        IsClick = true;
        //        UpdateSubMenuPositionAndLoad(button);
        //        pnlSub.Visible = true;
        //    }
        //}
        private void UpdateSubMenuPositionAndLoad(Guna2Button button)
        {
            if (IsClick)
            {
                pnlSub.Location = new Point(pnlLeft.Width + 5, button.Location.Y + 65);

                loadSubs(button.Tag.ToString());
            }
        }
        private void loadSubs(string name)
        {
            flowSub.Controls.Clear();
            pnlSub.Height = 0;
            if (menuData.ContainsKey(name))
            {
                foreach (var subMenu in menuData[name])
                {
                    Guna2Button btn = new Guna2Button
                    {
                        BorderColor = Color.CornflowerBlue,
                        BorderRadius = 5,
                        Dock = DockStyle.Top,
                        FillColor = Color.FromArgb(45, 62, 80),
                        ForeColor = Color.White,
                        Size = new Size(174, 40),
                        Text = subMenu.SubMenuText,
                        Tag = subMenu.SubMenuName
                    };

                    btn.Click += SubMenuButton_Click;
                    flowSub.Controls.Add(btn);
                    pnlSub.Height += 47;
                }
            }
        }

        private void SubMenuButton_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = sender as Guna2Button;
            string formName = clickedButton.Tag.ToString();

            OpenForm(formName);
            IsClick = pnlSub.Visible = false;
        }
        private void OpenForm(string formName)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == formName)
                {
                    form.BringToFront();
                    return;
                }
            }
            CreateFormByName(formName);
        }

        private void CreateFormByName(string formName)
        {
            string fullFormName = "YamyProject." + formName;
            Type formType = Type.GetType(fullFormName);

            if (formType != null && formType.IsSubclassOf(typeof(Form)))
            {
                var constructor = formType.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    Form newForm = (Form)constructor.Invoke(null);
                    newForm.Name = formName;
                    openChildForm(newForm);
                    pnlSub.Visible = false;
                    IsClick = false;
                }
            }
        }
       
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            pnlUser.Visible = false;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (pnlUser.Visible)
            {
                pnlUser.Visible = false;
                pnlUser.SendToBack();
            }
            else {
                pnlUser.Visible = true;
                pnlUser.Parent.Controls.SetChildIndex(pnlUser, 0);
                pnlUser.BringToFront();
            }
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            IsClick = pnlUser.Visible = pnlSub.Visible = false;
        }

        private void btnSignout_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            toLogin = true;
            this.Hide();
        }
        private bool isDraggingMain = false;
        private Point dragStartPointMain;

        private void pnlHeader_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (this.WindowState == FormWindowState.Maximized)
            //    this.WindowState = FormWindowState.Normal;
            //else
            //    this.WindowState = FormWindowState.Maximized;
        }

        private void pnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    isDraggingMain = false;
            //}
        }

        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            //if (isDraggingMain)
            //{
            //    this.WindowState = FormWindowState.Normal;
            //    this.Left += e.X - dragStartPointMain.X;
            //    this.Top += e.Y - dragStartPointMain.Y;
            //}
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    isDraggingMain = true;
            //    dragStartPointMain = e.Location;
            //}
        }

        private void customerCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCustomer());
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frmLogin.RoleId == 1)
                openChildForm(new MasterSetting());
        }

        private void customerBalanceSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCustomerSummary());
        }

        private void vendorBalanceSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVendorSummary());
        }

        private void customerBalanceDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openChildForm(new MasterBalanceDetails("Customer"));
        }

        private void vendorBalanceDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openChildForm(new MasterBalanceDetails("Vendor"));
        }

        private void itemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterInventory());
        }

        private void balanceSheetSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmBalanceSheetReport());
        }

        private void trialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterTrialBalance());

        }

        private void salesByCustomerSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSalesByCustomerSummary());
        }

        private void purchaseByVendorSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchaseByVendorSummary());
        }

        private void purchaseByVendorDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchaseByVendorDetails());

        }
        private void empToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmEmployeeBalanceSummary());
        }

        private void salesCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSales());
        }

        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSales());
        }

        private void receivePaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewReceiptVoucher());
        }

        private void vendorCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterVendor());
        }

        private void purchasesCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPurchases());
        }

        private void purchaseReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchase());
        }

        private void employeeCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterEmployee());
        }

        private void bankCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterBanking());
        }

        private void companyCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCompany());
        }

        private void journalTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterTransactionJournal("0",""));
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            NavigationBar();
        }

        private void NavigationBar()
        {
            pnlLeft.SuspendLayout();
            pnlSub.SuspendLayout();

            if (pnlLeft.Width == 225)
            {
                foreach (Control ctrl in pnlLeft.Controls)
                {
                    ctrl.Tag = ctrl.Text;
                }
                pnlLeft.Width = 40;
                if (pnlLeft.Width <= 0) pnlLeft.Width = 1;

                pnlSub.Location = new Point(pnlSub.Location.X - 175, pnlSub.Location.Y);
                comboBox1.Visible = false;
                guna2Button2.Visible = false;
                guna2HtmlLabel1.Text = "";
                guna2HtmlLabel3.Text = "";
                btnExpand.Image = Properties.Resources.Chevron_Righ22t;
            }
            else
            {
                foreach (Control ctrl in pnlLeft.Controls)
                {
                    ctrl.Text = ctrl.Tag?.ToString();
                }
                pnlLeft.Width = 225;
                pnlSub.Location = new Point(pnlSub.Location.X + 175, pnlSub.Location.Y);
                comboBox1.Visible = true;
                guna2Button2.Visible = true;
                guna2HtmlLabel1.Text = "My Shortcuts";
                loadCompany();
                btnExpand.Image = Properties.Resources.Back;
            }

            pnlLeft.ResumeLayout();
            pnlSub.ResumeLayout();
        }

        private void NavigationBarOld()
        {
            if (pnlLeft.Width == 225)
            {
                foreach (Control ctrl in pnlLeft.Controls)
                {
                    ctrl.Tag = ctrl.Text;
                    //ctrl.Text = "";
                }
                //btnExpand.Text = ">>";
                pnlLeft.Width = 40;
                pnlSub.Location = new Point(pnlSub.Location.X - 175, pnlSub.Location.Y);
                comboBox1.Visible = false;
                guna2Button2.Visible = false;
                guna2HtmlLabel1.Text = "";
                guna2HtmlLabel3.Text = "";
                btnExpand.Image = Properties.Resources.Chevron_Righ22t;

            }
            else
            {
                foreach (Control ctrl in pnlLeft.Controls)
                {
                    ctrl.Text = ctrl.Tag.ToString();
                }
                //btnExpand.Text = "<<";
                pnlLeft.Width = 225;
                pnlSub.Location = new Point(pnlSub.Location.X + 175, pnlSub.Location.Y);
                comboBox1.Visible = true;
                guna2Button2.Visible = true;
                guna2HtmlLabel1.Text = "My Shortcuts";
                loadCompany();
                btnExpand.Image = Properties.Resources.Back;
            }
        }

        private void backupCurrentCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

        private void restorePreviousCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        public static bool RestoreDatabase(string backupFilePath)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["Secret"].ConnectionString;
                var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);

                string server = builder.Server;
                string user = builder.UserID;
                string password = builder.Password;
                string database = builder.Database;

                string cmd = $"mysql --host={server} --user={user} --password={password} {database} < \"{backupFilePath}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    using (StreamWriter sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine(cmd);
                        }
                    }
                    process.WaitForExit();
                }

                MessageBox.Show("Restore Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Restore Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCompany());
        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAccountantCenter()); // Open Accountant Center Form
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            openChildFormX(new FrmDashboard());
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterBanking());
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterInventory());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCustomer());
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterVendor());
        }

        private void btnHr_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterEmployee());
        }

        private void csotCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCostCenter());
        }

        private void openBankCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterBankCard());
        }

        private void chequesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCheque());
        }

        private void pDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPDC());
        }

        private void attendanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterAttendanceSheet());
        }

        private void salarySheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSalarySheet());
        }

        private void leaveSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmLeaveSalary());
        }

        private void endOfServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmEndOfService());
        }

        private void loansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterLoan());
        }

        private void finalSettlementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterFinalSettlement());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            if (frmLogin.RoleId == 1)
                openChildForm(new MasterSetting());
        }
        private void damageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewItem());
        }

        private void stockManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterStockManagement());
        }
        private void debitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterDebitNote());
        }

        private void creditNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCreditNote());
        }
        private void payBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewPaymentVoucher());
        }

        private void changeCurrentPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFormIntoPanel(new frmChangePassword());
        }

        private void itemCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewCategory());
        }

        private void itemVatListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterItemTaxCode());
        }

        private void itemUnitListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterItemUnit());
        }

        private void listOfFixedAssetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterFixedAssets());
        }

        private void quotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesQuotation());
        }

        private void salesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesOrder());
        }

        private void salesReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesReturn());
        }

        private void purchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPurchaseOrder());
        }

        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPurchaseReturn());
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewPaymentVoucher());
        }

        private void journalVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterJournalVoucher());
        }

        private void paymentVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPaymentVoucher());
        }

        private void reciptVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterReceiptVoucher());
        }

        private void addFixedAssetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewFixedAssets());
        }

        private void fixedAssetsCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterFixedAssets());

        }

        private void fixedAssetsCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewFixedAssetsCategory());
        }

        private void chartOfAccountToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterChartOfAccount());
        }

        private void costCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCostCenter());
        }

        private void addPrepaidExpenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewPrepaidExpense());
        }

        private void prepaidExpenseCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPrepaidExpense());
        }

        private void pettyCashCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPettyCash());
        }

        private void addAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPettyCashCard());
        }

        private void addUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewItemUnit().ShowDialog();
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewCategory().ShowDialog();

        }

        private void wareHouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterWarehouseNew());
        }

        private void iNVENTORYCENTERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterInventory());
        
        }
        private void salesTaxCodeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterItemTaxCode());
        }

        private void itemTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterItemTransaction());
        }

        private void salesByItemDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesReport("Item Details"));
        }

        private void salesAgainSummeryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesReport("Aging Summary"));
        }

        private void purchaseAgingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesReport("Aging Summary"));
        }

        private void purchaseAgingReportDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesReport("Aging Details"));
        }

        private void purchaseByItemSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchaseByItemSummary());
        }

        private void salesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmSalesByItemSummary());
        }
        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmDataHandler());
        }

        private void pettyCashToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //
        }

        private void purchaseByItemDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchaseByItemDetails(0));
        }

        private void generalLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmGeneralLedger());
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterChartOfAccount());
        }

        private void salesProformaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSalesProforma());
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            new frmCompanyList().Show();
            this.Hide();
        }
        private void itemMovingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewItemMovingReport());
        }

        private void itemTransactionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openChildForm(new MasterItemTransaction());
        }

        private void advanceVoucherToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterAdvancePaymentVoucher());
        }

        private void itemProfitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterInventoryProfitStatement());
        }

        private void vATReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterVatReport());
        }

        private void inventoryCenterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterInventory());
        }

        private void advancePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterAdvancePaymentVoucher());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Attendance Sheet Center")
            {
                openChildForm(new MasterAttendanceSheet());
            }
            else if (comboBox1.Text == "Bank Card Center")
            {
                openChildForm(new MasterBankCard());
            }
            else if (comboBox1.Text == "Bank Center")
            {
                openChildForm(new MasterBanking());
            }
            else if (comboBox1.Text ==
            "Change Current Password")
            {
                LoadFormIntoPanel(new frmChangePassword());
            }
            else if (comboBox1.Text ==
            "Chart Of Account")
            {
                openChildForm(new MasterChartOfAccount());
            }
            else if (comboBox1.Text ==
            "Cheques Center")
            {
                openChildForm(new MasterCheque());
            }
            else if (comboBox1.Text ==
            "Company Profile")
            {
                openChildForm(new MasterCompany());
            }
            else if (comboBox1.Text ==
            "Cost Center")
            {
                openChildForm(new MasterCostCenter());
            }
            else if (comboBox1.Text ==
            "Create Sales")
            {
                openChildForm(new frmSales());
            }
            else if (comboBox1.Text ==
            "Create Purchases")
            {
                openChildForm(new frmPurchase());
            }
            else if (comboBox1.Text ==
            "Credit Note")
            {
                openChildForm(new MasterCreditNote());
            }
            else if (comboBox1.Text ==
            "Customer Center")
            {
                openChildForm(new MasterCustomer());
            }
            else if (comboBox1.Text ==
            "Debit Note")
            {
                openChildForm(new MasterDebitNote());
            }
            else if (comboBox1.Text ==
            "Employee Center")
            {
                openChildForm(new MasterEmployee());
            }
            else if (comboBox1.Text ==
            "End Of Services")
            {
                openChildForm(new frmEndOfService());
            }
            else if (comboBox1.Text ==
            "Final Settlement")
            {
                openChildForm(new MasterFinalSettlement());
            }
            else if (comboBox1.Text ==
            "Fixed Assets")
            {
                openChildForm(new MasterFixedAssets());
            }
            else if (comboBox1.Text ==
            "Inventory Center")
            {
                openChildForm(new MasterInventory());
            }
            else if (comboBox1.Text ==
            "Leave Salary")
            {
                openChildForm(new frmLeaveSalary());
            }
            else if (comboBox1.Text ==
            "Loan Center")
            {
                openChildForm(new MasterLoan());
            }
            else if (comboBox1.Text ==
            "Open Bank Card")
            {
                openChildForm(new MasterBankCard());
            }
            else if (comboBox1.Text ==
            "Payment Voucher")
            {
                openChildForm(new MasterPaymentVoucher());
            }
            else if (comboBox1.Text ==
            "PDC Center")
            {
                openChildForm(new MasterPDC());
            }
            else if (comboBox1.Text ==
            "Petty Cash Center")
            {
                openChildForm(new MasterPettyCash());
            }
            else if (comboBox1.Text ==
            "Prepaid Expense Center")
            {
                openChildForm(new MasterPrepaidExpense());
            }
            else if (comboBox1.Text ==
            "Purchase Order")
            {
                openChildForm(new MasterPurchaseOrder());
            }
            else if (comboBox1.Text ==
            "Purchases Invoice")
            {
                openChildForm(new frmPurchase());
            }
            else if (comboBox1.Text ==
            "Purchases Return")
            {
                openChildForm(new MasterPurchaseReturn());
            }
            else if (comboBox1.Text ==
            "Receipt Voucher")
            {
                openChildForm(new MasterReceiptVoucher());
            }
            else if (comboBox1.Text ==
            "Salary Sheet")
            {
                openChildForm(new frmSalarySheet());
            }
            else if (comboBox1.Text ==
            "Sales Order")
            {
                openChildForm(new MasterSalesOrder());
            }
            else if (comboBox1.Text ==
            "Sales Quotation")
            {
                openChildForm(new MasterSalesQuotation());
            }
            else if (comboBox1.Text ==
            "Sales Proforma")
            {
                openChildForm(new MasterSalesProforma());
            }
            else if (comboBox1.Text ==
            "Sales Return")
            {
                openChildForm(new MasterSalesReturn());
            }
            else if (comboBox1.Text ==
            "Stock Management")
            {
                openChildForm(new MasterStockManagement());
            }
            else if (comboBox1.Text ==
            "Vendor Center")
            {
                openChildForm(new MasterVendor());
            }
            else if (comboBox1.Text ==
            "Warehouse Center")
            {
                openChildForm(new MasterWarehouseNew());
            }
            else if(comboBox1.Text == "Transactions Journal")
            {
                openChildForm(new MasterTransactionJournal("0", ""));
            }
        }

        private void panel19_Paint(object sender, PaintEventArgs e)
        {

        }

        private void incomeByCustomerSummeryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmIncomeByCustomerSummary());
        }

        private void expenseFromVendorSummeryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmIncomeByVendorSummary());
        }

        private void customerAgingSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCustomerAgingSummary());
        }

        private void profitLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new ProfitandLossReport(""));
        }

        private void customerBalanceSummaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCustomerBalanceSummary());
        }

        private void customerIncomeSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmIncomeByCustomerSummary());
        }

        private void vendorBalanceSummaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVendorAgingSummary());
        }

        private void vendorBalanceSummaryToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVendorBalanceSummary());
        }

        private void vendorIncomSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmIncomeByVendorSummary());
        }

        private void backUpCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateBackUp();
        }
        private void CreateBackUp()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Backup Folder";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath.ToString()+@"\";
                    if (string.IsNullOrEmpty(selectedPath))
                    {
                        selectedPath = @"C:\";
                    }
                    DatabaseBackupRestore.BackupDatabase(selectedPath);
                }
            }
        }

        private void restoreCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select SQL Backup File";
                openFileDialog.Filter = "SQL Files (*.sql)|*.sql";
                openFileDialog.InitialDirectory = @"D:\Yamy";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        DatabaseBackupRestore.RestoreDatabase(filePath);
                    }
                }
            }

        }

        private void chartOfAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterChartOfAccount());
        }

        private void costCenterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCostCenterSummary());
        }

        private void fixedAssetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmFixedAsset());
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPrepaidExpenseSummary());
        }

        private void vatCategoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterVAT());
        }
        
        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectTendering());
        }

        private void projectEstimateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectEstimating());
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectPlanning());
        }

        private void projectManagingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openChildForm(new MasterProjectManagement());
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            new frmCRMmain().Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            openChildForm(new frmHome());
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            new frmMainRMS().Show();
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            string url = "https://yamyco.co/";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // This is required for .NET Core and .NET 5+
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening link: " + ex.Message);
            }
        }

        private void userCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void usersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frmLogin.RoleId==1)
                openChildForm(new frmUserAccess());
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (this.Width == 0 || this.Height == 0) return;
            base.OnPaint(e);
        }

        private void themeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFormIntoPanel(new frmColorThempickercs());
        }

        private void vatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVATCorporate());
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmUpdate().Show();
        }

        private void projectProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewProjectTimeLine());
        }

        private void ganttChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MyForm());
        }

        private void projectSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProject());
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAddProject(this, 0));
        }

        private void tenderCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectTender());
        }

        private void newTenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmAddTenderName(null, 0));
        }

        private void siteCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectSite());
        }

        private void newSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmProjectSites(null, 0));
        }

        private void projectWorkDoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectWorkDone());
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            new frmMainManufacturing().Show();
        }

        private void projectSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterProjectManagement());
        }

        private void projectDashboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewProjectDashBoard());
        }

        private void projectActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewActivityReport());
        }

        private void projectAssignSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewAssignReport());
        }

        private void projectResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewResourceReport());
        }

        private void projectProgressSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewProgressSummary());
        }

        private void projectWorkProgressSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewWorkProgressSummary());
        }

        private void reconciliationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterBankReconciliation());
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            ContextMenuStrip languageMenu = new ContextMenuStrip();
            languageMenu.Items.Add("English", null, (s, ea) => ChangeLanguage("en"));
            languageMenu.Items.Add("Arabic", null, (s, ea) => ChangeLanguage("ar"));
            languageMenu.Items.Add("French", null, (s, ea) => ChangeLanguage("fr"));
            languageMenu.Items.Add("Hindi", null, (s, ea) => ChangeLanguage("hi"));
            languageMenu.Items.Add("Urdu", null, (s, ea) => ChangeLanguage("ur"));
            languageMenu.Items.Add("Spanish", null, (s, ea) => ChangeLanguage("es"));
            languageMenu.Items.Add("German", null, (s, ea) => ChangeLanguage("gr"));
            languageMenu.Items.Add("Japanese", null, (s, ea) => ChangeLanguage("ja"));
            languageMenu.Items.Add("Korean", null, (s, ea) => ChangeLanguage("ko"));
            languageMenu.Items.Add("Tagalog / Filipino", null, (s, ea) => ChangeLanguage("fil"));
            languageMenu.Items.Add("Simplified Chinese", null, (s, ea) => ChangeLanguage("zh-CN"));
            languageMenu.Items.Add("Traditional Chinese", null, (s, ea) => ChangeLanguage("zh-TW"));

            var button = sender as Guna2Button;
            languageMenu.Show(button, new Point(0, button.Height));
        }
        private void ChangeLanguage(string cultureCode)
        {
            LocalizationManager.SetLanguage(cultureCode);

            this.Controls.Clear();
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void statementOfCashFlowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCashFlowStatement());
        }

        private void frmAuditTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmUserActivityReport());
        }

        private void incomeExpenseStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmIncomeExpenseStatement());
        }

        private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPettyCashBalanceDetails());
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPettyCashBalanceDetailEMP());
        }

        private void chequeSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmChequeReportView());
        }

        private void bankBalanceSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmBankBalanceSummary());
        }

        private void pDCOutstandingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPDCOutstanding());
        }

        private void pDCClearedHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPDCClearedHistory());
        }

        private void returnedChequesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmReturnedChequesReport());
        }

        private void chequeLinkedToPaymentReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmChequeLinkedToPaymentReceipt());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }

        private void CloseApplication()
        {
            DialogResult result = MessageBox.Show(
                "Do you want to exit the application?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel; // Or Abort
                Application.Exit();
            }
        }

        private void fixedAssetItemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmFixedAssetItemList());
        }

        private void remindersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmReminders());
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterChartOfAccount());
        }

        private void bankListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Bank List"));
        }

        private void bankCardListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Bank Card List"));
        }

        private void chequeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Cheque List"));
        }

        private void cityListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("City List"));
        }

        private void countryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Country List"));
        }

        private void costCenterListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Cost Center List"));
        }

        private void customerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Customer List"));
        }

        private void customerCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Customer Category List"));
        }

        private void departmentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Department List"));
        }

        private void employeeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Employee List"));
        }

        private void fixedAssetListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Fixed Asset List"));
        }

        private void fixedAssetCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Fixed Asset Category List"));
        }

        private void fixedAssetItemListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("FixedAsset Item List"));
        }

        private void itemListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Item List"));
        }

        private void itemCategoryListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Item Category List"));
        }

        private void itemTaxCodeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Item Tax Code List"));
        }

        private void itemUnitListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Item Unit List"));
        }

        private void itemWarehouseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Item Warehouse List"));
        }

        private void pettyCashCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Petty Cash Category List"));
        }

        private void positionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Position List"));
        }

        private void prepaidExpenseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Prepaid Expense List"));
        }

        private void prepaidExpenseCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Prepaid Expense Category List"));
        }

        private void vendorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Vendor List"));
        }

        private void vendorCategoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmListReport("Vendor Category List"));
        }

        private void addTaxCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewItemTaxCodes().ShowDialog();
        }

        private void itemTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterWarehouseTransfer());
        }

        private void warehouseSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmWarehouseSummary());
        }

        private void timelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmProjectTimelineSummary());
        }


        private void createPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchase());
        }

        private void paymentVoucherToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new frmViewPaymentVoucher(0, 0, true));
        }

        private void SubcontractorCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterSubcontractor());
        }

        private void purchasesCenterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterPurchases(true));
        }

        private void purchaseBySubcontractorSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPurchaseByVendorSummary(true));
        }

        private void licenseActivatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmLicense().ShowDialog();
        }

        private void btnCompanyProfile_Click(object sender, EventArgs e)
        {
            openChildForm(new MasterCompany());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlMain_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void pnlLeft_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pnlLeft_SizeChanged(object sender, EventArgs e)
        {
            if (pnlLeft.Width <= 0) pnlLeft.Width = 1;
            if (pnlLeft.Height <= 0) pnlLeft.Height = 1;
        }
    }

    public class SubMenu
    {
        public string SubMenuText { get; set; }
        public string SubMenuName { get; set; }
    }

}
