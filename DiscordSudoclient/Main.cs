using System;
using System.Reactive;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace DiscordSudoclient
{
    public partial class Main : Form
    {
        enum ChannelType
        {
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
        private string SelectedServer
        {
            get => SelectedId;
            set
            {
                SelectedId = value;
                var guild = Guilds[value];
                pbSelectedIcon.ImageLocation = $"https://cdn.discordapp.com/icons/{guild["id"]}/{guild["properties"]["icon"]}";

                flpChannels.Controls.Clear();
                foreach (var channel in guild["channels"])
                {
                    if ((ChannelType)((int)channel["type"]) != ChannelType.GUILD_TEXT) continue;
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
        private SubmitToken SubmitPopup = new SubmitToken();
        public Main()
        {
            InitializeComponent();
            SubmitPopup.OnCancel += () => Dispose();
            SubmitPopup.OnSubmit += OnSubmitToken;
            SubmitPopup.Show();
        }
        void OnSubmitToken(string token)
        {
            Client = new Gateway(token);
            Client.Dispatch += OnDispatch;
        }
        void OnServerSelect(string id) { SelectedServer = id; }
        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                var msg = new { content = txtMessage.Text };
                txtMessage.Text = "";
                var res = await Client.PostHTTP($"/channels/{SelectedChannel}/messages", msg);
                if (res["message"] != null)
                {
                    PushMessageString((string)res["message"]);
                    return;
                }
            } catch (Exception err)
            {
                PushMessageString($"Fatal: {err}");
            }
        }
        void PushMessageString(string message)
        {
            var msg = new Message();
            msg.Username = "Meta Info";
            msg.Content = message;
            flpMessages.Controls.Add(msg);
        }
        async void OnChannelSelect(string id)
        {
            SelectedChannel = id;
            flpMessages.Controls.Clear();
            PushMessageString("Loading channel contents");
            try
            {
                var res = await Client.GetHTTP($"/channels/{SelectedChannel}/messages");
                if (res == null)
                {
                    PushMessageString("No messages list can be located");
                    return;
                }
                if (res.Type != JTokenType.Array)
                {
                    PushMessageString((string)(res["message"]));
                    return;
                }
                flpMessages.Controls.Clear();
                foreach (var message in res.Reverse())
                {
                    var msg = new Message();
                    msg.Id = (string)message["id"];
                    msg.UserId = (string)message["author"]["id"];
                    msg.Username = (string)message["author"]["username"];
                    msg.Content = (string)message["content"];
                    if ((string?)message["author"]["avatar"] != null)
                        msg.Profile = (string)message["author"]["avatar"];
                    flpMessages.Invoke(new MethodInvoker(delegate { flpMessages.Controls.Add(msg); }));
                }
            }
            catch (Exception err)
            {
                PushMessageString($"Fatal: {err}");
            }
        }
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
                    }
                    break;
                case "MESSAGE_CREATE":
                    if ((string)data["channel_id"] != SelectedChannel) break;
                    var msg = new Message();
                    msg.Id = (string)data["id"];
                    msg.UserId = (string)data["author"]["id"];
                    msg.Username = (string)data["author"]["username"];
                    msg.Content = (string)data["content"];
                    if ((string?)data["author"]["avatar"] != null)
                        msg.Profile = (string)data["author"]["avatar"];
                    flpMessages.Invoke(new MethodInvoker(delegate {
                        flpMessages.Controls.Add(msg);
                        flpMessages.Controls.RemoveAt(0);
                    }));
                    break;
                case "MESSAGE_UPDATE":
                    flpMessages.Invoke(new MethodInvoker(delegate
                    {
                        for (int i = 0; i < flpMessages.Controls.Count; i++)
                            if (((Message)flpMessages.Controls[i]).Id == (string)data["id"])
                                ((Message)flpMessages.Controls[i]).Content = (string)data["content"];
                    }));
                    break;
                case "MESSAGE_DELETE":
                    flpMessages.Invoke(new MethodInvoker(delegate {
                        for (int i = 0; i < flpMessages.Controls.Count; i++)
                        if (((Message)flpMessages.Controls[i]).Id == (string)data["id"])
                             flpMessages.Controls.RemoveAt(i--); 
                    }));
                    break;
                case "MESSAGE_DELETE_BULK":
                    flpMessages.Invoke(new MethodInvoker(delegate
                    {
                        for (int i = 0; i < flpMessages.Controls.Count; i++)
                            if (data["ids"].Contains(((Message)flpMessages.Controls[i]).Id))
                                flpMessages.Controls.RemoveAt(i--);
                    }));
                    break;
            }
        }
    }
}
