using Godot;
using System;
using System.IO.Ports;

public partial class question2 : Node2D
{
    private SerialPort serialPort;

    public override void _Ready()
    {
        
        GetNode<Button>("ButtonA").Pressed += () => GoTo("res://questions/question3.tscn", 'W');
        GetNode<Button>("ButtonB").Pressed += () => GoTo("res://other screens/loser.tscn", 'W');
        GetNode<Button>("ButtonC").Pressed += () => GoTo("res://other screens/loser.tscn", 'R');

        try
        {
            serialPort = new SerialPort("COM4", 9600)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 50
            };
            serialPort.Open();
            GD.Print("Question 2: Serial port opened.");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Question 2: Failed to open serial port: " + ex.Message);
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

                if (rawData.Contains("A"))
                    GetNode<Button>("ButtonA").EmitSignal(Button.SignalName.Pressed);
                else if (rawData.Contains("B"))
                    GetNode<Button>("ButtonB").EmitSignal(Button.SignalName.Pressed);
                else if (rawData.Contains("C"))
                    GetNode<Button>("ButtonC").EmitSignal(Button.SignalName.Pressed);
            }
            catch (TimeoutException) { }
            catch (Exception ex)
            {
                GD.PrintErr("Serial Read Error: " + ex.Message);
            }
        }
    }

    private void GoTo(string path, char result)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write(new char[] { result }, 0, 1);
        }

        if (ResourceLoader.Exists(path))
        {
            GetTree().ChangeSceneToFile(path);
        }
        else
        {
            GD.PrintErr($"ERROR: Cannot find scene at '{path}'. Check your FileSystem path and capitalization.");
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
