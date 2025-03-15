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
    public partial class Message : UserControl
    {
        public string Id;
        public string UserId;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Username { get => lblUsername.Text; set => lblUsername.Text = value; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Content { get => lblContent.Text; set => lblContent.Text = value; }
        private string profile;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Profile
        {
            get => profile; 
            set {
                profile = value; 
                pbUser.ImageLocation = $"https://cdn.discordapp.com/avatars/{UserId}/{profile}.png";
            }
        }
        public Message()
        {
            InitializeComponent();
        }
    }
}
