using QuickPulse.Explains;

namespace QuickFuzzr.Reactor.Tests.TheGreatExample;

[DocFile]
[DocFileHeader("Comparing Things")]
public class TheDocCreator
{
    [Fact(Skip = "explicit")]
    public void Go()
    {
        Explain.This<TheDocCreator>("comparison.md");
    }
}