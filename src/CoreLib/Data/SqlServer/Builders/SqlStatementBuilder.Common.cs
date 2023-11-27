﻿using Library.Data.SqlServer.Builders.Bases;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static TCommandStatement ForceFormatValues<TCommandStatement>([DisallowNull] this TCommandStatement statement, bool forceFormatValues = true)
        where TCommandStatement : ICommandStatement => statement.With(x => x.ForceFormatValues = forceFormatValues);
}