using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Domain;
using BLL.Implementations;

namespace UI
{
    public partial class frmMain : Form
    {
        PermissionService _permissionService = new PermissionService();
        User currentUser;
        public frmMain(User logedUser)
        {
            InitializeComponent();
            currentUser = logedUser;
            getPatents();
        }

        private Dictionary<Guid,string> patentsDict = new Dictionary<Guid, string> {};

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
                    }
                }
                else
                {
                    patents.Add(c);
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
                    patentsDict.Add(c.Id , c.Name);
                }
            }
        }
    }
}
