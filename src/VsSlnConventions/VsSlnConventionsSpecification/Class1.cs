using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
      var testProjectSuffix = Any.Identifier();
      var testProjectName = projectName + testProjectSuffix;

      var sln = Solution.CreateNew(SlnDirectory, SlnFileName);
      sln.AddProject(projectName);
      sln.AddProject(testProjectName);

      using (new SolutionScope(sln))
      using (var stringWriter = ConsoleToStringWriter())
      {
        var conventionsApp = new VsConventionsDriver(SlnDirectory, SlnFileName);
        conventionsApp.Run();

        var output = stringWriter.ToString();

        conventionsApp.Result.Should().Be(0);
        output.Should().Contain($"{projectName} has a specification: {testProjectName}");
        output.Should().Contain($"{testProjectName} has a production project: {projectName}");
      }
    }

    private static StringWriter ConsoleToStringWriter()
    {
      var consoleToStringWriter = new StringWriter();
      Console.SetOut(consoleToStringWriter); //bug command-query violation
      return consoleToStringWriter;
    }
  }

  public class SolutionScope : IDisposable
  {
    private readonly Solution _sln;

    public SolutionScope(Solution sln)
    {
      _sln = sln;
      sln.Save();
    }

    public void Dispose()
    {
      File.Delete(_sln.Filename);
    }

  }

  public class VsConventionsDriver
  {
    private readonly string _slnDirectory;
    private readonly string _slnFileName;

    public VsConventionsDriver(string slnDirectory, string slnFileName)
    {
      _slnDirectory = slnDirectory;
      _slnFileName = slnFileName;
    }

    public int Result { get; private set; }

    public void Run()
    {
      var solution = Solution.LoadFrom(Path.Combine(_slnDirectory, _slnFileName));
    }
  }

  public class ConventionsResult
  {
  }

  public class ConventionsDomainLogic
  {
  }
}
