using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

public class PixelateOverlay : Form
{
    private System.Windows.Forms.Timer timer;
    private int pixelSize = 2; // Adjust pixelation level
    private IntPtr lastWindow = IntPtr.Zero;
    private Bitmap lastFrame = null; // Store last frame for disposal

    // DllImport for user32 functions
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }
    public PixelateOverlay()
    {
        this.BackColor = Color.Black;
        this.Opacity = 0.85; // Semi-transparent overlay
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.DoubleBuffered = true;
        this.Visible = false;

        int exStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
        SetWindowLong(this.Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
    }

    public void StartOverlay()
    {
        if (!this.Visible)
        {
            this.Bounds = GetWindowBounds(GetForegroundWindow());
        }
        // Work around for "White Box" of form showing on first draw
        this.Location = new Point(-10000, -10000); 

        Thread.Sleep(100); 
        ForceFreshCapture();

        this.Invoke((MethodInvoker)delegate {
            this.Show(); 
        });

        if (timer == null)
        {
            timer = new System.Windows.Forms.Timer { Interval = 100 }; // 10 FPS
            //timer = new System.Windows.Forms.Timer { Interval = 16 }; // Set to 60 FPS (~16.67ms)
            timer.Tick += (s, e) => UpdateOverlay();
        }

        if (!timer.Enabled)
        {
            timer.Start();
        }
    }

    private void ForceFreshCapture()
    {
        Bitmap screenshot = CaptureScreen();
        if (screenshot != null)
        {
            Bitmap pixelated = Pixelate(screenshot, pixelSize);

            if (lastFrame != null)
            {
                lastFrame.Dispose();
            }

            lastFrame = pixelated; // Store current frame for next disposal

            this.BackgroundImage = pixelated;
            this.Invalidate(); // Redraw smoothly

            screenshot.Dispose(); 
        }
    }

    public void StopOverlay()
    {
        if (this.Visible)
        {
            this.Hide();
        }

        // Stop the timer when overlay is hidden
        if (timer != null && timer.Enabled)
        {
            timer.Stop();
        }
    }

    private void UpdateOverlay()
    {
        IntPtr hwnd = GetForegroundWindow();
        if (hwnd == IntPtr.Zero || hwnd == this.Handle) return;

        // Update position & size only if the window changed
        if (hwnd != lastWindow || this.Bounds != GetWindowBounds(hwnd))
        {
            lastWindow = hwnd;
            this.Bounds = GetWindowBounds(hwnd);
        }

        Bitmap screenshot = CaptureScreen();
        if (screenshot != null)
        {
            Bitmap pixelated = Pixelate(screenshot, pixelSize);

            // Dispose old frame to free memory
            if (lastFrame != null)
                lastFrame.Dispose();

            lastFrame = pixelated; // Store current frame for disposal next cycle

            // Ensure old background image is disposed before assigning a new one
            if (this.BackgroundImage != null)
                this.BackgroundImage.Dispose();

            this.BackgroundImage = pixelated;
            this.Invalidate(); // Redraw smoothly

            screenshot.Dispose(); // Dispose of the original screenshot immediately
        }
    }

    private Rectangle GetWindowBounds(IntPtr hwnd)
    {
        if (!GetWindowRect(hwnd, out RECT rect)) return Rectangle.Empty;
        return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    }

    private Bitmap CaptureScreen()
    {
        // Get the bounds of the screen (full screen capture)
        Rectangle bounds = Screen.PrimaryScreen.Bounds;
        Bitmap bmp = new Bitmap(bounds.Width, bounds.Height);

        // Capture the screen using CopyFromScreen
        using (Graphics gfx = Graphics.FromImage(bmp))
        {
            gfx.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
        }

        return bmp;
    }

    private Bitmap Pixelate(Bitmap original, int pixelSize)
    {
        int width = original.Width;
        int height = original.Height;

        Bitmap small = new Bitmap(width / pixelSize, height / pixelSize);
        using (Graphics g = Graphics.FromImage(small))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(original, new Rectangle(0, 0, small.Width, small.Height));
        }

        Bitmap pixelated = new Bitmap(original.Width, original.Height);
        using (Graphics g = Graphics.FromImage(pixelated))
        {
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(small, new Rectangle(0, 0, width, height));
        }

        small.Dispose(); // Dispose the small resized version
        return pixelated;
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Ensure the last frame is properly disposed when closing
        if (lastFrame != null)
        {
            lastFrame.Dispose();
            lastFrame = null;
        }
        base.OnFormClosing(e);
    }
}
