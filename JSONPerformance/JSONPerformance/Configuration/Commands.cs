using CommandDotNet;

namespace JSONPerformance.Configuration;

public class Commands
{
    [Subcommand]
    public DatabaseCommands Database { get; set; } = null!;

    [Subcommand]
    public PerformanceCommands Performance { get; set; } = null!;
    
    [Subcommand]
    public IntegrityCommands Integrity { get; set; } = null!;
}