using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JupiterX.Menu
{
    public static class WebWrapper
    {
        public static void Post(string url, string body, Action<string> callback)
            => Run(() => Send("POST", url, body), callback);

        public static void Get(string url, Action<string> callback)
            => Run(() => Send("GET", url, null), callback);

        private static void Run(Func<string> work, Action<string> callback)
        {
            var t = new Thread(() =>
            {
                string result;
                try { result = work(); }
                catch (Exception e) { result = "ERROR: " + e.Message; }
                callback?.Invoke(result);
            });
            t.IsBackground = true;
            t.Start();
        }

        private static string Send(string method, string url, string body)
        {
            bool https = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
            int schemeLen = https ? 8 : (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? 7 : 0);
            string rest = url.Substring(schemeLen);

            int slash = rest.IndexOf('/');
            string hostPort = slash == -1 ? rest : rest.Substring(0, slash);
            string path = slash == -1 ? "/" : rest.Substring(slash);

            string host;
            int port;
            int colon = hostPort.IndexOf(':');
            if (colon == -1) { host = hostPort; port = https ? 443 : 80; }
            else { host = hostPort.Substring(0, colon); port = int.Parse(hostPort.Substring(colon + 1)); }

            byte[] payload = body != null ? Encoding.UTF8.GetBytes(body) : new byte[0];

            var sb = new StringBuilder();
            sb.Append(method).Append(' ').Append(path).Append(" HTTP/1.1\r\n");
            sb.Append("Host: ").Append(host).Append("\r\n");
            sb.Append("User-Agent: JupiterX\r\n");
            if (body != null)
            {
                sb.Append("Content-Type: application/json\r\n");
                sb.Append("Content-Length: ").Append(payload.Length).Append("\r\n");
            }
            sb.Append("Connection: close\r\n\r\n");
            byte[] header = Encoding.ASCII.GetBytes(sb.ToString());

            using (var client = new TcpClient { SendTimeout = 10000, ReceiveTimeout = 10000 })
            {
                client.Connect(host, port);

                Stream stream = client.GetStream();
                if (https)
                {
                    var ssl = new SslStream(stream, false, (s, c, ch, e) => true);
                    ssl.AuthenticateAsClient(host);
                    stream = ssl;
                }

                stream.Write(header, 0, header.Length);
                if (payload.Length > 0) stream.Write(payload, 0, payload.Length);
                stream.Flush();

                var ms = new MemoryStream();
                byte[] buf = new byte[8192];
                int n;
                while ((n = stream.Read(buf, 0, buf.Length)) > 0)
                    ms.Write(buf, 0, n);

                string raw = Encoding.UTF8.GetString(ms.ToArray());
                int split = raw.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                return split == -1 ? raw : raw.Substring(split + 4);
            }
        }
    }
}