namespace AutoTrust;

public struct DependencyNode {
  public int Depth { get; set; }
  public string Name { get; set; }
  public string ParentName { get; set; }
  public bool HasInitScript { get; set; }
  public bool HasAnalyzers { get; set; }
  public bool IsDeprecated { get; set; }
  public HashSet<string> Frameworks { get; set; }
}
