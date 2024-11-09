using System.Drawing.Drawing2D;

public class StoreTile : Panel
{
    public string MachineName { get; private set; }
    private Label labelMachineName;
    private Label labelLocation;
    private Label labelWANIP;
    private Label labelISP;
    private Label labelLastConnected;

    private string currentWanIp;
    private string currentIsp;
    private DateTime currentLastConnected;
    private Color currentTileColor;


    public StoreTile(string machineName, string location, string wanIp, string isp, DateTime lastConnected, Color tileColor)
    {
        this.MachineName = machineName;
        this.Size = new Size(350, 180); // Adjusted height to accommodate the new label
        this.Margin = new Padding(10, 10, 10, 10); // Increased top margin
        this.BackColor = tileColor; // Set the color based on the parameter
        this.BorderStyle = BorderStyle.None;
        this.Padding = new Padding(10);

        InitializeTile(machineName, location, wanIp, isp, lastConnected);
        this.DoubleBuffered = true;

        // Set initial values
        currentWanIp = wanIp;
        currentIsp = isp;
        currentLastConnected = lastConnected;
        currentTileColor = tileColor;
    }

    private void InitializeTile(string machineName, string location, string wanIp, string isp, DateTime lastConnected)
    {
        // Dispose of existing controls to prevent memory leaks
        labelMachineName?.Dispose();
        labelLocation?.Dispose();
        labelWANIP?.Dispose();
        labelISP?.Dispose();
        labelLastConnected?.Dispose();

        labelMachineName = new Label
        {
            Text = $"Machine: {machineName}",
            Location = new Point(20, 20),
            AutoSize = true,
            Font = new Font("Arial", 13, FontStyle.Bold)
        };

        labelLocation = new Label
        {
            Text = $"Location: {location}",
            Location = new Point(20, 60),
            AutoSize = true,
            Font = new Font("Arial", 12)
        };

        labelWANIP = new Label
        {
            Text = $"WAN IP: {wanIp}",
            Location = new Point(20, 80),
            AutoSize = true,
            Font = new Font("Arial", 12)
        };

        labelISP = new Label
        {
            Text = $"ISP: {isp}",
            Location = new Point(20, 100), // Positioned under the WAN IP
            AutoSize = true,
            Font = new Font("Arial", 12)
        };

        labelLastConnected = new Label
        {
            Text = $"Last Connected: {lastConnected:dd-MM-yyyy HH:mm:ss}",
            Location = new Point(20, 120), // Positioned under the ISP
            AutoSize = true,
            Font = new Font("Arial", 12)
        };

        this.Controls.Add(labelMachineName);
        this.Controls.Add(labelLocation);
        this.Controls.Add(labelWANIP);
        this.Controls.Add(labelISP);
        this.Controls.Add(labelLastConnected);
    }

    public void UpdateTile(string wanIp, string isp, DateTime lastConnected, Color tileColor)
    {
        if (wanIp != currentWanIp || isp != currentIsp || lastConnected != currentLastConnected || tileColor != currentTileColor)
        {
            currentWanIp = wanIp;
            currentIsp = isp;
            currentLastConnected = lastConnected;
            currentTileColor = tileColor;

            this.BackColor = tileColor;

            labelWANIP.Text = $"WAN IP: {wanIp}";
            labelISP.Text = $"ISP: {isp}";
            labelLastConnected.Text = $"Last Connected: {lastConnected:dd-MM-yyyy HH:mm:ss}";

            this.Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Increase the radius for smoother corners
        int radius = 30;  // Increased radius for smoother corners
        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
        path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), -90, 90);
        path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
        path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
        path.CloseFigure();

        // Set the path as the region for the panel
        this.Region = new Region(path);

        // Enable anti-aliasing for smoother edges
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Brush brush = new SolidBrush(this.BackColor))
        {
            e.Graphics.FillPath(brush, path);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Dispose of any resources used by the tile
            labelMachineName?.Dispose();
            labelLocation?.Dispose();
            labelWANIP?.Dispose();
            labelISP?.Dispose();
            labelLastConnected?.Dispose();
        }

        base.Dispose(disposing);
    }
}
