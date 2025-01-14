
using Avalonia.Media;

namespace USBRevertTool;

public class Particle
{
    public Avalonia.Point Position { get; set; }
    public Avalonia.Vector Velocity { get; set; }
    public int LifeTime { get; set; }
    public Color Color { get; set; } 
}