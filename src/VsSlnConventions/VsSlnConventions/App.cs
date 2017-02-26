using System.IO;
using FubuCsProjFile;
using VsSlnConventionsSpecification;

namespace VsSlnConventions
{
  public class App
  {
    private readonly string _slnDirectory;
    private readonly string _slnFileName;

    public App(string slnDirectory, string slnFileName)
    {
      _slnDirectory = slnDirectory;
      _slnFileName = slnFileName;
    }

    public int Run()
    {
      var consoleResultBuilder = new ConsoleResultBuilder();
      var solution = Solution.LoadFrom(Path.Combine(_slnDirectory, _slnFileName));
      var ruleSet = new RuleSet(consoleResultBuilder);
      ruleSet.ApplyTo(solution);
      return 0; //bug needs more tests!!
    }
  }
}