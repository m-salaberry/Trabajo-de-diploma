using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Domain;
using Services.Implementations;
using UI.secondaryForms;

namespace UI
{
    public partial class frmMain : Form
    {
        
        PermissionService _permissionService = new PermissionService();
        User currentUser;
        private frmMain(User logedUser)
        {
            InitializeComponent();
            ResetMainPanelSize();
            this.CenterToScreen();
            currentUser = logedUser;
            getPatents();
            this.Text = $"StockHelper - Logged in as: {currentUser.Name}";
        }

        private static frmMain _instance;

        public static frmMain GetInstance(User logedUser = null!)
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new frmMain(logedUser);
            }
            return _instance;
        }

        private Dictionary<Guid, string> patentsDict = new Dictionary<Guid, string> { };

        private void frmMain_Load(object sender, EventArgs e)
        {
            List<Services.Domain.Component> family = currentUser.Permissions;
            List<Services.Domain.Component> patents = new List<Services.Domain.Component>();

            foreach (Services.Domain.Component c in family)
            {
                if (c is Family)
                {
                    foreach (Services.Domain.Component child in c.Children)
                    {
                        patents.Add(child);
                        Console.WriteLine(child.Name);
                    }
                }
                else
                {
                    patents.Add(c);
                    Console.WriteLine(c.Name);
                }
            }

        }

        private void getPatents()
        {
            List<Services.Domain.Component> patents = _permissionService.GetAll();
            foreach (Services.Domain.Component c in patents)
            {
                if (!(c is Family))
                {
                    patentsDict.Add(c.Id, c.Name);
                }
            }
        }

        public void ResetMainPanelSize()
        {
            this.MinimumSize = new Size(800, 600);
        }

        private void showContent(UserControl newContent)
        {
            this.MinimumSize = new Size(0, 0);
            panelContainerMain.Controls.Clear();
            newContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContainerMain.Controls.Add(newContent);
            newContent.BringToFront();
        }

        private void tsmUsers_Click(object sender, EventArgs e)
        {
            ctrlUsers userManagement = new ctrlUsers();
            showContent(userManagement);
        }

        private void tsmPerms_Click(object sender, EventArgs e)
        {
            ctrlPermsissions permissionManagement = new ctrlPermsissions();
            showContent(permissionManagement);
        }
    }
}
