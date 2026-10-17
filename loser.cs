using Godot;
using System;
using System.IO.Ports;

public partial class loser : Node2D
{
    private SerialPort serialPort;

    public override void _Ready()
    {
        
        GetNode<Button>("Button").Pressed += RestartGame;

        try
        {
            serialPort = new SerialPort("COM4", 9600)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 50
            };
            serialPort.Open();
            GD.Print("Loser Scene: Serial port opened.");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Loser Scene: Failed to open serial port: " + ex.Message);
        }
    }

    public override void _Process(double delta)
    {
        if (serialPort == null || !serialPort.IsOpen)
            return;

        if (serialPort.BytesToRead > 0)
        {
            try
            {
                string rawData = serialPort.ReadExisting();

                
                if (rawData.Contains("A") || rawData.Contains("B") || rawData.Contains("C") || rawData.Contains("R"))
                {
                    GetNode<Button>("Button").EmitSignal(Button.SignalName.Pressed);
                }
            }
            catch (TimeoutException) { }
            catch (Exception ex)
            {
                GD.PrintErr("Serial Read Error: " + ex.Message);
            }
        }
    }

    private void RestartGame()
    {
        
        string targetPath = "res://questions/question1.tscn";

        if (ResourceLoader.Exists(targetPath))
        {
            GetTree().ChangeSceneToFile(targetPath);
        }
        else
        {
            GD.PrintErr($"ERROR: Cannot find scene at '{targetPath}'. Right-click question1.tscn in FileSystem and select 'Copy Path' to verify.");
        }
    }

    public override void _ExitTree()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}