﻿using System.Runtime.Versioning;

namespace Library.Interfaces;

public interface IConvertible<TThis, TOther>
{
    TOther ConvertTo();
    static abstract TThis From(TOther other);
}
