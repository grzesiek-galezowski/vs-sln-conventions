using System.Linq;
using System.Text.RegularExpressions;
using FubuCsProjFile;
using VsSlnConventionsSpecification;

namespace VsSlnConventions
{
  public class RuleSet
  {
    private readonly ConsoleResultBuilder _consoleResultBuilder;

    public RuleSet(ConsoleResultBuilder consoleResultBuilder)
    {
      _consoleResultBuilder = consoleResultBuilder;
    }

    public void ApplyTo(Solution solution)
    {
      //bug configurable suffix
      var productionProjectNames = solution.Projects.Where(p => !p.ProjectName.EndsWith("Specification")).Select(p => p.ProjectName);
      var testProjectNames = solution.Projects.Where(p => p.ProjectName.EndsWith("Specification")).Select(p => p.ProjectName);

      foreach (var productionProjectName in productionProjectNames)
      {
        var expectedTestProjectName = productionProjectName + "Specification";
        if (testProjectNames.Any(testProjectName => testProjectName == expectedTestProjectName))
        {
          _consoleResultBuilder.Info(productionProjectName + " has a specification: " + expectedTestProjectName);
        }
        else
        {
          //bug errors!!!
        }
      }

      foreach (var testProjectName in testProjectNames)
      {
        var expectedProductionProjectName = Regex.Replace(testProjectName, "Specification$", string.Empty);
        if (productionProjectNames.Any(productionProjectName => productionProjectName == expectedProductionProjectName))
        {
          _consoleResultBuilder.Info(testProjectName + " has a production project: " + expectedProductionProjectName);
        }
        else
        {
          //bug errors
        }
      }
    }
  }
}