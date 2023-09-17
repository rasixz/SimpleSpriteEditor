namespace Spritery.Revertable;

public class UndoList
{
    private readonly Stack<RevertableAction> _undoElements = new Stack<RevertableAction>();
    private readonly Stack<RevertableAction> _redoElements = new Stack<RevertableAction>();

    public bool HasUndoableActions => _undoElements.Count > 0;
    public bool HasRedoableActions => _redoElements.Count > 0;

    public void Do(RevertableAction action)
    {
        _redoElements.Clear();
        // Push the performed action to the undo stack
        _undoElements.Push(action);
        
        Console.WriteLine($"Action: {action}");
    }

    public RevertableAction Undo()
    {
        if (_undoElements.Count == 0)
            throw new InvalidOperationException("No actions to undo.");

        var action = _undoElements.Pop();
        _redoElements.Push(action); // Push undone action to the redo stack
        Console.WriteLine($"Undone {action}");
        return action;
    }

    public RevertableAction Redo()
    {
        if (_redoElements.Count == 0)
            throw new InvalidOperationException("No actions to redo.");

        var action = _redoElements.Pop();
        _undoElements.Push(action); // Push redone action back to the undo stack
        Console.WriteLine($"Redone {action}");
        return action;
    }
}