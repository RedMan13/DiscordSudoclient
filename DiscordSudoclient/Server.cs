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
    public partial class Server : UserControl
    {
        public string Id;
        public string Name;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Icon { get => pbIcon.ImageLocation; set => pbIcon.ImageLocation = value; }
        public Server()
        {
            InitializeComponent();
        }

        public delegate void SelectedEventHandler(string id);
        public event SelectedEventHandler OnSelected;

        private void pbIcon_Click(object sender, EventArgs e) { OnSelected(Id); }
    }
}
