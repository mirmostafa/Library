using Microsoft.Extensions.Logging;

namespace Library.Logging;

public interface IDbCrudLogger : IMsLoggerMessageWapper
{
    void ItemAdded(string log);
    void ItemAddFail(string log, Exception? exception = null);
    void ItemDeleted(string log);
    void ItemDeleteFail(string log, Exception? exception = null);
    void ItemUpdated(string log);
    void ItemUpdateFail(string log, Exception? exception = null);
}

public sealed class DbCrudLogger : MsLoggerMessageWrapperBase<DbCrudLogger>, IDbCrudLogger
{
    private readonly Action<IMsLogger, string, Exception?> _itemAdded;
    private readonly Action<IMsLogger, string, Exception?> _itemAddFail;

    private readonly Action<IMsLogger, string, Exception?> _itemUpdated;
    private readonly Action<IMsLogger, string, Exception?> _itemUpdateFail;

    private readonly Action<IMsLogger, string, Exception?> _itemDeleted;
    private readonly Action<IMsLogger, string, Exception?> _itemDeleteFail;

    public DbCrudLogger(IMsLogger logger, string name, int eventId)
        : base(logger, name, eventId)
    {
        this._itemAdded = LoggerMessage.Define<string>(MsLogLevel.Information, new EventId(eventId, name), "Item added: {item}");
        this._itemAddFail = LoggerMessage.Define<string>(MsLogLevel.Error, new EventId(eventId, name), "item add fail: {item}");

        this._itemUpdated = LoggerMessage.Define<string>(MsLogLevel.Information, new EventId(eventId, name), "item updated: {item}");
        this._itemUpdateFail = LoggerMessage.Define<string>(MsLogLevel.Error, new EventId(eventId, name), "item update fail: {item}");

        this._itemDeleted = LoggerMessage.Define<string>(MsLogLevel.Information, new EventId(eventId, name), "item deleted: {item}");
        this._itemDeleteFail = LoggerMessage.Define<string>(MsLogLevel.Error, new EventId(eventId, name), "item delete fail: {item}");
    }

    public void ItemAdded(string log) => this._itemAdded(this._logger, log, null);
    public void ItemAddFail(string log, Exception? exception = null) => this._itemAddFail(this._logger, log, exception);

    public void ItemUpdated(string log) => this._itemUpdated(this._logger, log, null);
    public void ItemUpdateFail(string log, Exception? exception = null) => this._itemUpdateFail(this._logger, log, exception);

    public void ItemDeleted(string log) => this._itemDeleted(this._logger, log, null);
    public void ItemDeleteFail(string log, Exception? exception = null) => this._itemDeleteFail(this._logger, log, exception);
}
