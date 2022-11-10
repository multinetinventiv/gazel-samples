using Gazel.Service;

namespace Inventiv.Sample.Module.Todo;

/// <summary>
/// Result code class creates a code block for each module in your project
/// Put this class in a common module so that all modules can use it.
/// </summary>
public class ResultCodes : ResultCodeBlocks
{
    // every code block reserves 700 error codes, 100 warning codes and 100 info codes
    public static readonly ResultCodeBlock Todo = CreateBlock(1, "Todo"); // Creates a code block at index 1

    // Result code blocks helps you to create unique error codes and are optional.

    // 0 = Success
    // 1-10000 = Success with an information
    // 10001-20000 = Success with a warning
    // 20001-90000 = Handled exception -> creates a log in warning level
    // 99999 = Unhandled exception -> creates a log in error level
}
