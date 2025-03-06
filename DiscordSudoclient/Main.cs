namespace DiscordSudoclient
{
    public partial class Main : Form
    {
        private static Gateway Client;
        public Main()
        {
            InitializeComponent();
            Client = new Gateway();
        }
    }
}
