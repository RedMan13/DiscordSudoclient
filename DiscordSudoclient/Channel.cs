using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordSudoclient
{
    public partial class Channel : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Name { get => lblName.Text; set => lblName.Text = value; }
        public string Id;
        private bool selected = false;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Selected {
            get => selected;
            set
            {
                selected = value;
                if (selected)
                    lblName.Font = new Font(lblName.Font, FontStyle.Bold);
                else
                    lblName.Font = new Font(lblName.Font, FontStyle.Regular);
            }
        }
        public Channel()
        {
            InitializeComponent();
        }

        public delegate void SelectedEventHandler(string id);
        public event SelectedEventHandler OnSelected;
        private void Channel_Click(object sender, EventArgs e) { OnSelected(Id); }
    }
}
