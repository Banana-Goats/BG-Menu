using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public class UdpListener
{
    private UdpClient udpClient;
    private Thread listenThread;
    private Form mainForm;
    private NotifyIcon notifyIcon;
    private volatile bool isListening;

    public UdpListener(Form form)
    {
        mainForm = form;
        if (IsPortInUse(50548))
        {
            MessageBox.Show("Listener is already active on port 50548.");
            return;
        }

        udpClient = new UdpClient(50548);
        isListening = true;
        InitializeNotifyIcon();
        listenThread = new Thread(new ThreadStart(ListenForCommands));
        listenThread.IsBackground = true;
        listenThread.Start();

        ShowBalloonNotification("Listener started on port 50548");
    }

    private bool IsPortInUse(int port)
    {
        bool isPortInUse = false;
        try
        {
            UdpClient testClient = new UdpClient(port);
            testClient.Close();
        }
        catch (SocketException)
        {
            isPortInUse = true;
        }
        return isPortInUse;
    }

    private void InitializeNotifyIcon()
    {
        notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Information, // You can set a custom icon here
            Visible = true,
            BalloonTipTitle = "UDP Listener"
        };
    }

    private void ShowBalloonNotification(string message)
    {
        notifyIcon.BalloonTipText = message;
        notifyIcon.ShowBalloonTip(3000); // Show the balloon tip for 3 seconds
    }

    private void ListenForCommands()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 50548);

        while (isListening)
        {
            try
            {
                if (udpClient.Available > 0)  // Check if data is available
                {
                    byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);

                    // Show the received command in a message box
                    mainForm.Invoke((MethodInvoker)delegate
                    {
                        // Your UI update code here
                    });

                    // Optional: If you still want to process the command
                    if (receivedData.StartsWith("(Command)|"))
                    {
                        ProcessCommand(receivedData);
                    }
                }
                else
                {
                    Thread.Sleep(100); // Sleep for a while to reduce CPU usage
                }
            }
            catch (Exception ex)
            {
                if (isListening) // Only show the message if the listener was not explicitly stopped
                {
                    MessageBox.Show("Error receiving UDP data: " + ex.Message);
                }
            }
        }
    }

    private void ProcessCommand(string command)
    {
        string store = string.Empty;
        string message = string.Empty;

        string[] segments = command.Split('|');
        foreach (string segment in segments)
        {
            if (segment.Contains("-"))
            {
                string[] keyValue = segment.Split('-');
                if (keyValue.Length == 2)
                {
                    string field = keyValue[0].Trim('(', ')');  // Remove surrounding parentheses if present
                    string value = keyValue[1].Trim('(', ')');

                    if (field.Equals("Store", StringComparison.OrdinalIgnoreCase))
                    {
                        store = value;
                    }
                    else if (field.Equals("Message", StringComparison.OrdinalIgnoreCase))
                    {
                        message = value;
                    }
                }
            }
        }

        // If both store and message are found, show the notification
        if (!string.IsNullOrEmpty(store) && !string.IsNullOrEmpty(message))
        {
            ShowCustomBalloonNotification(store, message);
        }
    }

    private void ShowCustomBalloonNotification(string store, string message)
    {
        mainForm.Invoke((MethodInvoker)delegate
        {
            notifyIcon.BalloonTipTitle = "Network Notification";
            notifyIcon.BalloonTipText = $"{store} - {message}";
            notifyIcon.ShowBalloonTip(5000);  // Show the balloon tip for 5 seconds
        });
    }

    public void StopListening()
    {
        isListening = false; // Signal the thread to stop
        udpClient.Close();  // Close the UDP client to unblock the Receive call
        listenThread.Join();  // Wait for the thread to finish
        notifyIcon.Visible = false; // Hide the notify icon when stopping
    }
}
