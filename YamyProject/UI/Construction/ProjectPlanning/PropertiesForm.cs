using DlhSoft.ProjectDataControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Construction.ProjectPlanning
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        public Control Control
        {
            get { return propertyGrid.SelectedObject as Control; }
            set { propertyGrid.SelectedObject = value; }
        }
    }
}