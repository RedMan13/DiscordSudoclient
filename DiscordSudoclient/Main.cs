using System;
using System.Reactive;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace DiscordSudoclient
{
    public partial class Main : Form
    {
        enum ChannelType {
            GUILD_TEXT = 0,  // a text channel within a server
            DM = 1,  // a direct message between users
            GUILD_VOICE = 2,  // a voice channel within a server
            GROUP_DM = 3,  // a direct message between multiple users
            GUILD_CATEGORY = 4,  // an organizational category that contains up to 50 channels
            GUILD_ANNOUNCEMENT = 5,  // a channel that users can follow and crosspost into their own server (formerly news channels)
            ANNOUNCEMENT_THREAD = 10, // a temporary sub-channel within a GUILD_ANNOUNCEMENT channel
            PUBLIC_THREAD = 11, // a temporary sub-channel within a GUILD_TEXT or GUILD_FORUM channel
            PRIVATE_THREAD = 12, // a temporary sub-channel within a GUILD_TEXT channel that is only viewable by those invited and those with the MANAGE_THREADS permission
            GUILD_STAGE_VOICE = 13, // a voice channel for hosting events with an audience
            GUILD_DIRECTORY = 14, // the channel in a hub containing the listed servers
            GUILD_FORUM = 15, // Channel that can only contain threads
            GUILD_MEDIA = 16  // Channel that can only contain threads, similar to GUILD_FORUM channels
        }
        private static Gateway Client;
        private Dictionary<string, JToken> Guilds = new Dictionary<string, JToken>();
        private Dictionary<string, JToken> Channels = new Dictionary<string, JToken>();
        private string SelectedId;
        private string SelectedServer { 
            get => SelectedId; 
            set {
                SelectedId = value;
                var guild = Guilds[value];
                pbSelectedIcon.ImageLocation = $"https://cdn.discordapp.com/icons/{guild["id"]}/{guild["properties"]["icon"]}";

                flpChannels.Controls.Clear();
                foreach (var channel in guild["channels"])
                {
                    if ((ChannelType)((int)channel["type"]) != ChannelType.GUILD_TEXT) continue;
                    if (ChannelId == null) SelectedChannel = ((string)channel["id"]);
                    var obj = new Channel();
                    obj.Name = ((string)channel["name"]);
                    obj.Id = ((string)channel["id"]);
                    obj.OnSelected += OnChannelSelect;
                    flpChannels.Invoke(new MethodInvoker(delegate { flpChannels.Controls.Add(obj); }));
                }
            } 
        }
        private string ChannelId;
        private string SelectedChannel
        {
            get => ChannelId;
            set
            {
                ChannelId = value;
                foreach (Channel obj in flpChannels.Controls) 
                    obj.Selected = obj.Id == ChannelId;
            }
        }
        public Main()
        {
            InitializeComponent();
            Client = new Gateway("MTA2MDg1NzE4Mzc1Mjk0OTc5MA.GY_eNX.oeFw-DUQKCUqgTp1ona7laVFdzea0ioTw0xTAo");
            Client.Dispatch += OnDispatch;
        }
        void OnServerSelect(string id) { SelectedServer = id; }
        void OnChannelSelect(string id) { SelectedChannel = id; }
        void OnDispatch(string ev, JToken data)
        {
            switch (ev)
            {
                case "READY":
                    foreach (var guild in data["guilds"])
                    {
                        Guilds.Add(((string)guild["id"]), guild);
                        var server = new Server();
                        server.Name = ((string?)guild["properties"]["name"]);
                        server.Icon = $"https://cdn.discordapp.com/icons/{guild["id"]}/{guild["properties"]["icon"]}";
                        server.Id = (string)guild["id"];
                        server.OnSelected += OnServerSelect;
                        flpServers.Invoke(new MethodInvoker(delegate { flpServers.Controls.Add(server); }));

                        foreach (var channel in guild["channels"])
                        {
                            channel["guild_id"] = guild["id"];
                            Channels.Add(((string?)channel["id"]), channel);
                        }
                        if (SelectedId == null) SelectedServer = ((string)guild["id"]);
                    }
                    break;
            }
        }
    }
}
