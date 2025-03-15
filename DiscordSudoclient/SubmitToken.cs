using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordSudoclient
{
    public partial class SubmitToken : Form
    {
        public SubmitToken()
        {
            InitializeComponent();
        }

        public delegate void CancelTokenHandle();
        public event CancelTokenHandle OnCancel;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
            OnCancel();
        }

        public delegate void SubmitTokenHendle(string token);
        public event SubmitTokenHendle OnSubmit;
        private void btnOk_Click(object sender, EventArgs e)
        {
            Hide();
            OnSubmit(txtToken.Text);
        }
    }
}
