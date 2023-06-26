using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Types;
public static class Actions
{
    public static Action Empty() => () => { };
    public static Action<T> Empty<T>() => t => { };

    public static Func<bool> True() => () => true;
}
