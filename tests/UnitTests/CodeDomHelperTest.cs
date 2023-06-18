using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Helpers.CodeGen;

using Xunit;

namespace UnitTests;
public class CodeDomHelperTest
{
    [Fact]
    public void MyTestMethod()
    {
        CodeCompileUnit unit = new CodeCompileUnit();
        unit.AddNewNameSpace("myNamespace")
            .AddNewClass("myClass");
    }
}
