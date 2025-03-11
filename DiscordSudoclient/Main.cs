using Newtonsoft.Json.Linq;

namespace DiscordSudoclient
{
    public partial class Main : Form
    {
        private static Gateway Client;
        public Main()
        {
            InitializeComponent();
            Client = new Gateway("...");
            Client.Dispatch += OnDispatch;
        }
        void OnDispatch(string ev, JToken data) {
            switch (ev)
            {
                case "READY":
                    break;
            }
        }
    }
}
