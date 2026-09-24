using System;
using System.IO;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace TTTS.Windows;

public class TungWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    private const int Columns = 8, Rows = 16, FrameCount = 127;
    private const float Fps = 15f;

    private readonly string spriteSheetPath;

    // We give this window a constant ID using ###.
    // This allows for labels to be dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public TungWindow(Plugin plugin) : base("Tung###Tung Window")
    {
        Flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        configuration = plugin.Configuration;

        // Relative paths resolve against the game's working directory, so anchor to the plugin's install folder
        spriteSheetPath = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory!.FullName, "Data", "tung.png");
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        
    }

    public override void Draw()
    {
        var tex = Plugin.TextureProvider.GetFromFile(spriteSheetPath).GetWrapOrEmpty();

        var frame = (int)(ImGui.GetTime() * Fps) % FrameCount;
        var col = frame % Columns;
        var row = frame / Columns;

        var uvSize = new Vector2(1f / Columns, 1f / Rows);
        var uv0 = new Vector2(col, row) * uvSize;
        var frameSize = new Vector2(tex.Width / (float)Columns, tex.Height / (float)Rows);

        ImGui.Image(tex.Handle, frameSize * Dalamud.Interface.Utility.ImGuiHelpers.GlobalScale, uv0, uv0 + uvSize);
    }
}
