using Services.Contracts.CustomsException;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.secondaryForms
{
    public partial class ctrlUsers : UserControl
    {
        public ctrlUsers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            try
            {
                newUserForm newUserForm = new newUserForm();
                newUserForm.ShowDialog();
                newUserForm.BringToFront();

            }
            catch (Exception ex)
            {
                throw new MySystemException(ex.Message, "");
            }
        }
    }
}
