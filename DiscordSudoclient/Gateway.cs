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
    class GatwayPacket
    {
        public GatewayOpcode op;
        public JsonValue d;
        public int? s;
        public string? t;
    }
    class Gateway
    {
        public static byte[] StreamEnd = { 0x00, 0x00, 0xFF, 0xFF };
        public static Uri Url = new Uri("wss://gateway.discord.gg?v=9&encoding=json&compress=zlib-stream");
        private int Seq;
        private WebsocketClient Socket;
        private byte[] Message = new byte[0];
        private System.Timers.Timer HeartBeat = new System.Timers.Timer();
        public Gateway()
        {
            HeartBeat.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                Send(new GatwayPacket { op = GatewayOpcode.Heartbeat, d = (JsonValue)(Seq) });
            };
            HeartBeat.AutoReset = true;
            Socket = new WebsocketClient(Url);
            Socket.IsTextMessageConversionEnabled = false;
            Socket.MessageReceived.Subscribe(OnMessage);
            Socket.Start();
        }
        void Send(GatwayPacket packet)
        {
            string toSend = JsonSerializer.Serialize(packet);
            Socket.Send(toSend);
        }
        void OnPacket(GatewayOpcode op, Dictionary<string, JsonValue> data, int seq, string ev) {
            if (seq != null) Seq = seq;
            switch (op)
            {
                case GatewayOpcode.Hello:
                    HeartBeat.Interval = ((double)data["heartbeat_interval"]);
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
            var packet = JsonSerializer.Deserialize<GatwayPacket>(data);
            OnPacket(packet.op, packet.d., packet.s, packet.t);
        }
        void OnMessage(ResponseMessage msg) {
            byte[] end = msg.Binary.Take(new Range(msg.Binary.Length - 4, msg.Binary.Length)).ToArray();
            Message = Message.Concat(msg.Binary).ToArray();
            if (end[0] == StreamEnd[0] && end[1] == StreamEnd[1] && end[2] == StreamEnd[2] && end[3] == StreamEnd[3]) 
                FlushInflator();
        }
    }
}
