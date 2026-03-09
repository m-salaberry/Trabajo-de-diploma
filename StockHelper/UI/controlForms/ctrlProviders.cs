using Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Implementations;

namespace UI.controlForms
{
    public partial class ctrlProviders : TranslatableUserControls
    {
        LanguageService languageService = LanguageService.GetInstance;
        public ctrlProviders()
        {
            InitializeComponent();
        }
    }
}
