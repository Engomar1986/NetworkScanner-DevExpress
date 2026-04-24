
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public class NetworkScanner
{
    public static string GetBaseIP()
    {
        foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                var parts = ip.ToString().Split('.');
                return $"{parts[0]}.{parts[1]}.{parts[2]}";
            }
        }
        return "192.168.1";
    }

    public static Task ScanAsync(
        Action<NetworkDevice> onFound,
        Action<int> onProgress,
        CancellationToken token)
    {
        return Task.Run(() =>
        {
            string baseIP = GetBaseIP();
            int total = 254;
            int count = 0;

            Parallel.For(1, 255, (i, state) =>
            {
                if (token.IsCancellationRequested)
                    state.Stop();

                string ip = $"{baseIP}.{i}";

                try
                {
                    using (Ping ping = new Ping())
                    {
                        var reply = ping.Send(ip, 200);

                        if (reply.Status == IPStatus.Success)
                        {
                            string host = GetHostName(ip);
                            string mac = GetMacAddress(ip);
                            string type = DetectDeviceType(host, mac);

                            onFound?.Invoke(new NetworkDevice
                            {
                                IP = ip,
                                HostName = host ?? "Unknown",
                                MAC = mac ?? "N/A",
                                DeviceType = type,
                                Status = "Online"
                            });
                        }
                    }
                }
                catch { }

                int progress = Interlocked.Increment(ref count);
                onProgress?.Invoke(progress * 100 / total);
            });
        });
    }

    private static string GetHostName(string ip)
    {
        try { return Dns.GetHostEntry(ip).HostName; }
        catch { return null; }
    }

    // 🔥 جلب MAC باستخدام arp
    private static string GetMacAddress(string ip)
    {
        try
        {
            Process p = new Process();
            p.StartInfo.FileName = "arp";
            p.StartInfo.Arguments = "-a " + ip;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            var match = Regex.Match(output,
                @"([0-9A-Fa-f]{2}[-:]){5}([0-9A-Fa-f]{2})");

            return match.Success ? match.Value : null;
        }
        catch
        {
            return null;
        }
    }

    // 🔥 تحديد نوع الجهاز (تقريبي)
    private static string DetectDeviceType(string host, string mac)
    {
        if (string.IsNullOrEmpty(host))
            return "Unknown";

        host = host.ToLower();

        if (host.Contains("printer"))
            return "Printer";

        if (host.Contains("router") || host.Contains("gateway"))
            return "Router";

        if (host.Contains("android") || host.Contains("phone"))
            return "Mobile";

        if (host.Contains("pc") || host.Contains("desktop"))
            return "Computer";

        return "Device";
    }
}
