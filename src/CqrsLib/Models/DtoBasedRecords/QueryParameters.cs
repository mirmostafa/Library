using Library.Cqrs.Models.Commands;
using Library.Cqrs.Models.Queries;

namespace Library.Cqrs.Models.DtoBasedRecords;

public readonly record struct DtoQuery<TParamDto, TResultDto>(TParamDto Dto) : IQuery<TResultDto>;
public readonly record struct DtoCommand<TParamDto, TResultDto>(TParamDto Dto) : ICommand;