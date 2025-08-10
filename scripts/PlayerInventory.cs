using System.Collections.Generic;
using Godot;

public partial class PlayerInventory : Node
{
    private readonly Dictionary<string, int> _tools = new();

    public void AddTools(string toolName, int amount)
    {
        if (_tools.ContainsKey(toolName))
        {
            _tools[toolName] += amount;
        }
        else
        {
            _tools[toolName] = amount;
        }
    }

    public bool HasTools(string toolName, int amount = 1)
    {
        return _tools.ContainsKey(toolName) && _tools[toolName] >= amount;
    }

    public bool RemoveTool(string toolName, int amount = 1)
    {
        if (!HasTools(toolName, amount))
            return false;

        _tools[toolName] -= amount;

        if (_tools[toolName] <= 0)
            _tools.Remove(toolName);

        return true;
    }

    public int GetTool(string toolName)
    {
        return _tools.TryGetValue(toolName, out int value) ? value : 0;
    }

    public void ClearInventoryOnDeath()
    {
        GD.Print("Player died — clearing inventory!");
        _tools.Clear();
    }
}
