using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.CodeGeneration.v2.Back;
public interface ICanBePartial
{
    bool IsPartial { get; }
}
