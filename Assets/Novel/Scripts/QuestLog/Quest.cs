using System.Collections.Generic;

public class Quest
{
    public string Id { get; }
    public string Title { get; }
    public List<string> Updates { get; } = new List<string>();
    public bool IsComplete { get; private set; }

    public Quest(string id, string title)
    {
        Id = id;
        Title = title;
    }

    public void AddUpdate(string text) => Updates.Add(text);
    public void Complete() => IsComplete = true;
}