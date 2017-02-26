using System;
using System.IO;
using FluentAssertions;
using FubuCsProjFile;
using VsSlnConventions;

namespace VsSlnConventionsSpecification
{
  public class VsConventionsDriver : IShould
  {
    private readonly string _slnDirectory;
    private readonly string _slnFileName;

    public VsConventionsDriver(string slnDirectory, string slnFileName)
    {
      _slnDirectory = slnDirectory;
      _slnFileName = slnFileName;
    }

    public int Result { get; set; } = 0;

    public static StringWriter ConsoleToStringWriter()
    {
      var consoleToStringWriter = new StringWriter();
      Console.SetOut(consoleToStringWriter); //bug command-query violation
      return consoleToStringWriter;
    }

    public IShould Should => (IShould)this;

    void IShould.ContainTestToProductionProjectMatch(string output, string testProjectName, string projectName)
    {
      output.Should().Contain($"{testProjectName} has a production project: {projectName}");
    }

    void IShould.ContainProductionToTestProjectMatch(string output, string projectName, string testProjectName)
    {
      output.Should().Contain($"{projectName} has a specification: {testProjectName}");
    }

    void IShould.ReportSuccess()
    {
      Result.Should().Be(0);
    }

    public string Run()
    {
      string output;
      using (var stringWriter = ConsoleToStringWriter())
      {
        Result = new App(_slnDirectory, _slnFileName).Run();
        output = stringWriter.ToString();
      }
      return output;
    }
  }

  public interface IShould
  {
    void ReportSuccess();
    void ContainProductionToTestProjectMatch(string output, string projectName, string testProjectName);
    void ContainTestToProductionProjectMatch(string output, string testProjectName, string projectName);
  }
}