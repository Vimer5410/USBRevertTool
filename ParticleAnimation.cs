using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;

namespace USBRevertTool;

public class ParticleAnimation:Window
{
    private static Canvas _canvas;
    private static Random _random = new Random();
    private static List<Particle> _particles = new List<Particle>();
    private const int ParticleCount = 50; 
    private const int CanvasWidth = 1800;
    private const int CanvasHeight = 1000;
    
    private static MainWindow _mainWindow;
    
    public static void Initialize(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }
    public static void StartAnimation()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(15);
                UpdateParticles();
                Draw(_mainWindow.canvas1,_particles);
            }
        });
    }



    private static void UpdateParticles()
    {
        
        _particles.RemoveAll(p => p.LifeTime <= 0);

        
        foreach (var particle in _particles)
        {
            particle.Position += particle.Velocity; 
            particle.LifeTime--; 
        }
        
        if (_particles.Count < ParticleCount)
        {
            GenerateParticles();
        }
    }

    private static void GenerateParticles()
    {
        for (int i = 0; i < ParticleCount; i++)
        {
            var color = Color.FromArgb(255, (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256));
            var particle = new Particle
            {
                Position = new Avalonia.Point(_random.Next(0, CanvasWidth), _random.Next(0,CanvasHeight)),
                Velocity = new Avalonia.Vector(_random.NextDouble() * 2 - 1, -(_random.NextDouble() * 2 + 1)),
                LifeTime = _random.Next(50, 100),
                Color = color
            };
            _particles.Add(particle);
            Console.WriteLine($"Particle {i}: Position {particle.Position} Color={particle.Color}");
        }
    }

    private static void Draw(Canvas canvas, List<Particle> particles)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            canvas.Children.Clear();
            
            foreach (var particle in particles)
            {
                byte alpha = (byte)(255 * (particle.LifeTime / 100.0)); 
                var brush = new SolidColorBrush(Color.FromArgb(alpha, particle.Color.R, particle.Color.G, particle.Color.B)); 

                var ellipse = new Ellipse
                {
                    Width = 10 + (100 - particle.LifeTime) * 0.05, 
                    Height = 10 + (100 - particle.LifeTime) * 0.05,
                    Fill = brush
                };
                Canvas.SetLeft(ellipse, particle.Position.X);
                Canvas.SetTop(ellipse, particle.Position.Y);
                
                canvas.Children.Add(ellipse);
            }
        });
    }
}