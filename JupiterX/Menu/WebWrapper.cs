// ============================================================
//  JupiterX | Menu/WebWrapper.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JupiterX.Menu
{
    public static class WebWrapper
    {
        public static void Get(string url, Action<string> callback, Dictionary<string, string> headers = null)
            => Run(() => SendString("GET", url, null, headers), callback);
        public static void Post(string url, string body, Action<string> callback, Dictionary<string, string> headers = null)
            => Run(() => SendString("POST", url, body, headers), callback);
        public static void Put(string url, string body, Action<string> callback, Dictionary<string, string> headers = null)
            => Run(() => SendString("PUT", url, body, headers), callback);
        public static void Patch(string url, string body, Action<string> callback, Dictionary<string, string> headers = null)
            => Run(() => SendString("PATCH", url, body, headers), callback);
        public static void Delete(string url, string body, Action<string> callback, Dictionary<string, string> headers = null)
            => Run(() => SendString("DELETE", url, body, headers), callback);
        public static void Send(string method, string url, string body, Action<Response> callback, Dictionary<string, string> headers = null)
            => RunResponse(() => SendFull(method, url, body, headers), callback);
        public static void DownloadFile(string url, string savePath, Action<bool> callback, Dictionary<string, string> headers = null)
        {
            var t = new Thread(() =>
            {
                bool ok = false;
                try { ok = DownloadToFile(url, savePath, headers, 0); }
                catch { ok = false; }
                callback?.Invoke(ok);
            });
            t.IsBackground = true;
            t.Start();
        }

        public class Response
        {
            public int StatusCode;
            public string StatusText;
            public string Body;
            public Dictionary<string, string> Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
            public string Error;
        }

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

        private static void RunResponse(Func<Response> work, Action<Response> callback)
        {
            var t = new Thread(() =>
            {
                Response result;
                try { result = work(); }
                catch (Exception e) { result = new Response { StatusCode = 0, Error = e.Message }; }
                callback?.Invoke(result);
            });
            t.IsBackground = true;
            t.Start();
        }

        private static void ParseUrl(string url, out bool https, out string host, out int port, out string path)
        {
            https = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
            int schemeLen = https ? 8 : (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? 7 : 0);
            string rest = url.Substring(schemeLen);
            int slash = rest.IndexOf('/');
            string hostPort = slash == -1 ? rest : rest.Substring(0, slash);
            path = slash == -1 ? "/" : rest.Substring(slash);
            int colon = hostPort.IndexOf(':');
            if (colon == -1) { host = hostPort; port = https ? 443 : 80; }
            else { host = hostPort.Substring(0, colon); port = int.Parse(hostPort.Substring(colon + 1)); }
        }

        private static byte[] BuildRequest(string method, string host, string path, byte[] payload, bool hasBody, Dictionary<string, string> headers)
        {
            var sb = new StringBuilder();
            sb.Append(method).Append(' ').Append(path).Append(" HTTP/1.1\r\n");
            sb.Append("Host: ").Append(host).Append("\r\n");
            sb.Append("User-Agent: UnityPlayer/2025.1.0f1 (UnityWebRequest/1.0, libcurl/7.80.0)\r\n");
            sb.Append("Accept: */*\r\n");

            bool hasContentType = false;
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    if (h.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase)) hasContentType = true;
                    if (h.Key.Equals("Host", StringComparison.OrdinalIgnoreCase)) continue;
                    if (h.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase)) continue;
                    if (h.Key.Equals("Connection", StringComparison.OrdinalIgnoreCase)) continue;
                    sb.Append(h.Key).Append(": ").Append(h.Value).Append("\r\n");
                }
            }
            if (hasBody)
            {
                if (!hasContentType)
                    sb.Append("Content-Type: application/json\r\n");
                sb.Append("Content-Length: ").Append(payload.Length).Append("\r\n");
            }
            sb.Append("Connection: close\r\n\r\n");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        private static byte[] RawExchange(string method, string url, byte[] payload, bool hasBody, Dictionary<string, string> headers)
        {
            ParseUrl(url, out bool https, out string host, out int port, out string path);
            byte[] header = BuildRequest(method, host, path, payload, hasBody, headers);
            using (var client = new TcpClient { SendTimeout = 15000, ReceiveTimeout = 15000 })
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
                if (hasBody && payload.Length > 0) stream.Write(payload, 0, payload.Length);
                stream.Flush();
                var ms = new MemoryStream();
                byte[] buf = new byte[8192];
                int n;
                while ((n = stream.Read(buf, 0, buf.Length)) > 0)
                    ms.Write(buf, 0, n);
                return ms.ToArray();
            }
        }

        private static string SendString(string method, string url, string body, Dictionary<string, string> headers)
        {
            byte[] payload = body != null ? Encoding.UTF8.GetBytes(body) : new byte[0];
            byte[] all = RawExchange(method, url, payload, body != null, headers);
            return Encoding.UTF8.GetString(ExtractBodyBytes(all));
        }

        private static Response SendFull(string method, string url, string body, Dictionary<string, string> headers)
        {
            byte[] payload = body != null ? Encoding.UTF8.GetBytes(body) : new byte[0];
            byte[] all = RawExchange(method, url, payload, body != null, headers);
            var resp = new Response();
            int split = FindSplit(all);
            string headerBlock = split == -1 ? Encoding.ASCII.GetString(all) : Encoding.ASCII.GetString(all, 0, split);
            string[] lines = headerBlock.Split(new[] { "\r\n" }, StringSplitOptions.None);
            if (lines.Length > 0)
            {
                string[] statusParts = lines[0].Split(new[] { ' ' }, 3);
                if (statusParts.Length >= 2) int.TryParse(statusParts[1], out resp.StatusCode);
                if (statusParts.Length >= 3) resp.StatusText = statusParts[2];
            }
            for (int i = 1; i < lines.Length; i++)
            {
                int c = lines[i].IndexOf(':');
                if (c > 0)
                {
                    string k = lines[i].Substring(0, c).Trim();
                    string v = lines[i].Substring(c + 1).Trim();
                    resp.Headers[k] = v;
                }
            }
            resp.Body = Encoding.UTF8.GetString(ExtractBodyBytes(all));
            return resp;
        }

        private static bool DownloadToFile(string url, string savePath, Dictionary<string, string> headers, int redirectDepth)
        {
            if (redirectDepth > 5) return false;
            byte[] all = RawExchange("GET", url, new byte[0], false, headers);
            int split = FindSplit(all);
            string headerBlock = split == -1 ? "" : Encoding.ASCII.GetString(all, 0, split);
            if (headerBlock.StartsWith("HTTP/1.1 3") || headerBlock.StartsWith("HTTP/1.0 3"))
            {
                int li = headerBlock.IndexOf("Location:", StringComparison.OrdinalIgnoreCase);
                if (li >= 0)
                {
                    int le = headerBlock.IndexOf("\r\n", li, StringComparison.Ordinal);
                    if (le > li)
                    {
                        string loc = headerBlock.Substring(li + 9, le - (li + 9)).Trim();
                        if (!string.IsNullOrEmpty(loc) && loc != url)
                            return DownloadToFile(loc, savePath, headers, redirectDepth + 1);
                    }
                }
            }
            byte[] body = ExtractBodyBytes(all);
            if (body == null || body.Length == 0) return false;
            string dir = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(savePath, body);
            return true;
        }

        private static int FindSplit(byte[] all)
        {
            for (int i = 0; i < all.Length - 3; i++)
                if (all[i] == 13 && all[i + 1] == 10 && all[i + 2] == 13 && all[i + 3] == 10)
                    return i + 4;
            return -1;
        }

        private static byte[] ExtractBodyBytes(byte[] all)
        {
            int split = FindSplit(all);
            if (split == -1) return all;
            string headers = Encoding.ASCII.GetString(all, 0, split);
            bool chunked = headers.IndexOf("transfer-encoding: chunked", StringComparison.OrdinalIgnoreCase) >= 0;
            byte[] bodyBytes = new byte[all.Length - split];
            Array.Copy(all, split, bodyBytes, 0, bodyBytes.Length);
            if (!chunked) return bodyBytes;
            var outMs = new MemoryStream();
            int pos = 0;
            while (pos < bodyBytes.Length)
            {
                int lineEnd = -1;
                for (int i = pos; i < bodyBytes.Length - 1; i++)
                    if (bodyBytes[i] == 13 && bodyBytes[i + 1] == 10) { lineEnd = i; break; }
                if (lineEnd == -1) break;
                string sizeLine = Encoding.ASCII.GetString(bodyBytes, pos, lineEnd - pos).Trim();
                int semi = sizeLine.IndexOf(';');
                if (semi >= 0) sizeLine = sizeLine.Substring(0, semi);
                if (!int.TryParse(sizeLine, NumberStyles.HexNumber, null, out int chunkSize)) break;
                if (chunkSize == 0) break;
                int dataStart = lineEnd + 2;
                if (dataStart + chunkSize > bodyBytes.Length) chunkSize = bodyBytes.Length - dataStart;
                outMs.Write(bodyBytes, dataStart, chunkSize);
                pos = dataStart + chunkSize + 2;
            }
            return outMs.ToArray();
        }
    }
}