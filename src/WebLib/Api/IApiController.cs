using Library.Web.Results;

namespace Library.Web.Api;

public interface IApiController
{
}

public interface IAsyncReadApi<TId, TBriefResultDto, TFullResultDto> : IApiController
{
    Task<ApiResult<IEnumerable<TBriefResultDto>>> GetAllAsync();
    Task<ApiResult<TFullResultDto?>> GetByIdAsync(TId id);
}
public interface IAsyncWriteApi<TId, TFullWriteDto> : IApiController
{
    Task<ApiResult<TId>> CreateAsync(TFullWriteDto dto);
    Task<ApiResult> UpdateAsync(TId id, TFullWriteDto dto);
    Task<ApiResult> DeleteAsync(TId id);
}
public interface IAsyncRestFulApi<TId, TBriefResultDto, TFullResultDto, TFullParamsDto>
    : IApiController, IAsyncReadApi<TId, TBriefResultDto, TFullResultDto>, IAsyncWriteApi<TId, TFullParamsDto>
{

}

public interface IAsyncRestFulApi<TId, TFullResultDto, TFullParamsDto>
    : IApiController, IAsyncReadApi<TId, TFullResultDto, TFullResultDto>, IAsyncWriteApi<TId, TFullParamsDto>
{

}