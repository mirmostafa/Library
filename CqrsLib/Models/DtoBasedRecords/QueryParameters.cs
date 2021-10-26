namespace Library.Cqrs;

public readonly record struct DtoQuery<TParamDto, TResultDto>(TParamDto Dto) : IQuery<TResultDto>;
public readonly record struct DtoCommand<TParamDto, TResultDto>(TParamDto Dto) : ICommand;
