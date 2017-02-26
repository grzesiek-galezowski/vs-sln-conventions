using System;
using System.IO;
using FubuCsProjFile;
using TddEbook.TddToolkit;
using Xunit;

namespace VsSlnConventionsSpecification
{
  public class Class1
  {
    private const string SlnDirectory = ".";
    private const string SlnFileName = "TestSolution.sln";

    [Fact]
    public void ShouldWhatever() //bug
    {
      var projectName = Any.Identifier();
      var testProjectSuffix = "Specification"; //bug configurable suffix
      var testProjectName = projectName + testProjectSuffix;

      var sln = Solution.CreateNew(SlnDirectory, SlnFileName);
      sln.AddProject(projectName);
      sln.AddProject(testProjectName);

      var conventionsApp = new VsConventionsDriver(SlnDirectory, SlnFileName);

      string output;
      using (new PhysicalSolutionExistenceScope(sln))
      {
        output = conventionsApp.Run();
      }

      conventionsApp.Should.ReportSuccess();
      conventionsApp.Should.ContainProductionToTestProjectMatch(output, projectName, testProjectName);
      conventionsApp.Should.ContainTestToProductionProjectMatch(output, testProjectName, projectName);
    }
  }

  public class PhysicalSolutionExistenceScope : IDisposable
  {
    private readonly Solution _sln;

    public PhysicalSolutionExistenceScope(Solution sln)
    {
      _sln = sln;
      sln.Save();
    }

    public void Dispose()
    {
      File.Delete(_sln.Filename);
    }

  }

}
