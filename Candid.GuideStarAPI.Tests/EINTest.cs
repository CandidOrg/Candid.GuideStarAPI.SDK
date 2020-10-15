using System;
using Xunit;

namespace Candid.GuideStarAPI.Tests
{
  public class EINTest
  {
    [Fact]
    public void EIN_CorrectFormat_ExpectSuccess()
    {
      var ein = new EIN("12-3456789");
      Assert.Equal("12-3456789", ein.EinString);
    }

    [Fact]
    public void EIN_9Digits_ExpectReformat()
    {
      var ein = new EIN("123456789");
      Assert.Equal("12-3456789", ein.EinString);
    }

    [Fact]
    public void EIN_Spaces_ExpectNullException()
    {
      Assert.Throws<ArgumentNullException>(() => { var ein = new EIN("  "); });
    }

    [Fact]
    public void EIN_8Digits_ExpectArgException()
    {
      Assert.Throws<ArgumentException>(() => { var ein = new EIN("12-345678"); });
    }

    [Fact]
    public void EIN_TwoHyphens_ExpectArgException()
    {
      Assert.Throws<ArgumentException>(() => { var ein = new EIN("12-345-678"); });
    }

    [Fact]
    public void EIN_Garbage_ExpectArgException()
    {
      Assert.Throws<ArgumentException>(() => { var ein = new EIN("asdlkfj"); });
    }
  }
}
