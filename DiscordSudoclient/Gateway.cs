﻿using System.Text;
using Websocket.Client;
using Newtonsoft.Json.Linq;
using Ionic.Zlib;
using System.Net.Http.Json;

// coppied from https://stackoverflow.com/a/66261077
public class ZlibStreamContext
{
    private ZlibCodec _inflator;

    public ZlibStreamContext(bool expectRFC1950Header = false)
    {
        _inflator = new ZlibCodec();
        _inflator.InitializeInflate(expectRFC1950Header);
    }

    public byte[] InflateByteArray(byte[] deflatedBytes)
    {
        _inflator.InputBuffer = deflatedBytes;
        _inflator.AvailableBytesIn = deflatedBytes.Length;
        // account for a lot of possible size inflation (could be much larger than 4x)
        _inflator.OutputBuffer = new byte[0xFFFFFF];
        _inflator.AvailableBytesOut = _inflator.OutputBuffer.Length;
        _inflator.NextIn = 0;
        _inflator.NextOut = 0;

        _inflator.Inflate(FlushType.Sync);
        return _inflator.OutputBuffer[0.._inflator.NextOut];
    }
}
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
        private ZlibStreamContext DecompressContext = new ZlibStreamContext(false);
        private bool FirstMessage = true;
        private HttpClient httpClient = new HttpClient();
        public Gateway(string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
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
            byte[] decompressed = DecompressContext.InflateByteArray(Message);
            string data = Encoding.UTF8.GetString(decompressed);
            var packet = JObject.Parse(data);
            Message = new byte[0];
            OnPacket((GatewayOpcode)(int)packet["op"], packet["d"], (int?)(packet["s"]), (string?)(packet["t"]));
        }
        void OnMessage(ResponseMessage msg) {
            byte[] end = msg.Binary.Take(new Range(msg.Binary.Length - 4, msg.Binary.Length)).ToArray();
            Message = Message.Concat(FirstMessage ? msg.Binary.Take(new Range(2, msg.Binary.Length)) : msg.Binary).ToArray();
            FirstMessage = false;
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
        async Task<JObject> GetHTTP(string path, Dictionary<string, string> args)
        {
            string url = $"https://discord.com/api/v9{path}";
            bool firstArg = true;
            foreach (var arg in args)
            {
                url += firstArg ? "?" : "&";
                url += $"{arg.Key}={arg.Value}";
                firstArg = false;
            }
            var req = await httpClient.GetAsync(url);
            req.EnsureSuccessStatusCode();
            string res = await req.Content.ReadAsStringAsync();
            return JObject.Parse(res);
        }
        async Task<JObject> GetHTTP(string path, JObject body)
        {
            string url = $"https://discord.com/api/v9{path}";
            var data = new JsonContent();
            var req = await httpClient.PostAsync(url, );
            req.EnsureSuccessStatusCode();
            string res = await req.Content.ReadAsStringAsync();
            return JObject.Parse(res);
        }
    }
}
