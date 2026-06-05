using CTK.Engine;
using Raylib_cs;
using System;
using System.Numerics;

namespace CTK.Test;

internal sealed class Renderer : IDisposable
{
    private readonly Color[] Type2Color =
    [
        Color.Black,
        Color.White,
        Color.Red,
        Color.Blue,

        Color.Green,
        Color.Yellow,
        Color.Magenta,
        Color.SkyBlue
    ];

    private readonly uint Width;
    private readonly Texture2D Texture;
    private readonly Color[] Pixels;

    private Camera2D Camera;

    public bool IsOpen => !Raylib.WindowShouldClose();

    public Renderer(uint width, uint height)
    {
        Width = width;

        Raylib.InitWindow(1280, 720, "CLETKI");

        Image image = Raylib.GenImageColor((int)width, (int)height, Color.Black);
        Texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);

        Pixels = new Color[width * height];

        Camera = new Camera2D
        {
            //just some default values so user can see the window
            Offset = Vector2.Zero,
            Target = Vector2.Zero,
            Rotation = 0,
            Zoom = 1
        };
    }

    private void HandleZoom()
    {
        const float decreaseZoomMultiplier = 0.9f, increaseZoomMultiplier = 1.1f;

        float wheel = Raylib.GetMouseWheelMove();
        if(wheel == 0) return;

        Vector2 mouseScreen = Raylib.GetMousePosition();
        Vector2 beforeZoom = Raylib.GetScreenToWorld2D(mouseScreen, Camera);

        Camera.Zoom *= wheel > 0 ? increaseZoomMultiplier : decreaseZoomMultiplier;

        Vector2 afterZoom = Raylib.GetScreenToWorld2D(mouseScreen, Camera);

        Camera.Target += beforeZoom - afterZoom;
    }

    public void Update(Field field)
    {
        HandleZoom();

        var start = field.MyBounds.ValidStart;
        var end = field.MyBounds.ValidEnd;

        for(int y = start.Item2; y < end.Item2; y++)
        {
            int row = (y - start.Item2)* (int)Width;

            for(int x = start.Item1; x < end.Item1; x++)
            {
                Pixels[row + x - start.Item1] =
                    Type2Color[field.Map[x, y].Type];
            }
        }

        Raylib.UpdateTexture(Texture, Pixels);
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.Black);
        Raylib.BeginMode2D(Camera);
        Raylib.DrawTexture(Texture, 0, 0, Color.White);
        Raylib.EndMode2D();

        Raylib.EndDrawing();
    }

    public bool CanRender() => !Raylib.WindowShouldClose();

    public void Dispose()
    {
        Raylib.UnloadTexture(Texture);
        Raylib.CloseWindow();
    }
}