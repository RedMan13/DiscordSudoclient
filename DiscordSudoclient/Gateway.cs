using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Websocket.Client;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace DiscordSudoclient
{
    enum GatewayOpcode
    {
        Dispatch                = 0,  // [Receive] An event was dispatched.
        Heartbeat               = 1,  // [Send/Receive] Fired periodically by the client to keep the connection alive.
        Identify                = 2,  // [Send]    Starts a new session during the initial handshake.
        PresenceUpdate          = 3,  // [Send]    Update the client's presence.
        VoiceStateUpdate        = 4,  // [Send]    Used to join/leave or move between voice channels.
        Resume                  = 6,  // [Send]    Resume a previous session that was disconnected.
        Reconnect               = 7,  // [Receive] You should attempt to reconnect and resume immediately.
        RequestGuildMembers     = 8,  // [Send]    Request information about offline guild members in a large guild.
        InvalidSession          = 9,  // [Receive] The session has been invalidated. You should reconnect and identify/resume accordingly.
        Hello                   = 10, // [Receive] Sent immediately after connecting, contains the heartbeat_interval to use.
        HeartbeatACK            = 11, // [Receive] Sent in response to receiving a heartbeat to acknowledge that it has been received.
        RequestSoundboardSounds = 31, // [Send]    Request information about soundboard sounds in a set of guilds.
    }
    class Gateway
    {
        public static byte[] StreamEnd = { 0x00, 0x00, 0xFF, 0xFF };
        public static Uri Url = new Uri("wss://gateway.discord.gg?v=9&encoding=json&compress=zlib-stream");
        private int? Seq;
        private WebsocketClient Socket;
        private byte[] Message = new byte[0];
        private System.Timers.Timer HeartBeat = new System.Timers.Timer();
        private string Token;
        public Gateway(string token)
        {
            Token = token;
            HeartBeat.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                Send(GatewayOpcode.Heartbeat, new JObject(Seq));
            };
            HeartBeat.AutoReset = true;
            Socket = new WebsocketClient(Url);
            Socket.IsTextMessageConversionEnabled = false;
            Socket.MessageReceived.Subscribe(OnMessage);
            Socket.Start();
        }

        public delegate void GatewayDispatchEvent(string ev, JToken data);
        public event GatewayDispatchEvent Dispatch;
        void OnPacket(GatewayOpcode op, JToken data, int? seq, string? ev) {
            if (seq != null) Seq = seq;
            switch (op)
            {
                case GatewayOpcode.Dispatch:
                    Dispatch(ev, data); break;
                case GatewayOpcode.Heartbeat:
                    Send(GatewayOpcode.HeartbeatACK); break;
                case GatewayOpcode.Hello:
                    HeartBeat.Interval = ((double)data["heartbeat_interval"]);
                    HeartBeat.Enabled = true;
                    var msg = new JObject();
                    msg["token"]        = Token;
                    msg["capabilities"] = 16381;
                    msg["properties"]   = new JObject();
                    msg["presence"]     = new JObject();
                    msg["presence"]["status"]     = "online";
                    msg["presence"]["since"]      = 0;
                    msg["presence"]["activities"] = new JArray();
                    msg["presence"]["afk"]        = false;
                    msg["compress"]     = false;
                    msg["client_state"] = new JObject();
                    msg["client_state"]["guild_versions"] = new JArray();
                    Send(GatewayOpcode.Identify, msg);
                    break;
            }
        }
        async void FlushInflator()
        {
            byte[] trimmed = Message.Take(new Range(2, Message.Length)).ToArray();
            MemoryStream memoryStream = new MemoryStream(trimmed);
            DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
            MemoryStream decompressedStream = new MemoryStream();
            deflateStream.CopyTo(decompressedStream);
            string data = Encoding.UTF8.GetString(decompressedStream.ToArray());
            var packet = JObject.Parse(data);
            OnPacket((GatewayOpcode)(int)packet["op"], packet["d"], (int)(packet["s"]), (string)(packet["t"]));
        }
        void OnMessage(ResponseMessage msg) {
            byte[] end = msg.Binary.Take(new Range(msg.Binary.Length - 4, msg.Binary.Length)).ToArray();
            Message = Message.Concat(msg.Binary).ToArray();
            if (end[0] == StreamEnd[0] && end[1] == StreamEnd[1] && end[2] == StreamEnd[2] && end[3] == StreamEnd[3]) 
                FlushInflator();
        }

        void Send(GatewayOpcode op) { Send(op, null); }
        void Send(GatewayOpcode op, JObject? d)
        {
            var toSend = new JObject();
            toSend["op"] = (int)op;
            toSend["d"] = d;
            Socket.Send(toSend.ToString());
        }
    }
}
