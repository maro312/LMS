namespace Core.Contracts.Cruds;

/// <summary>
/// Interface for a CRUD (Create, Read, Update, Delete) application service.
/// </summary>
/// <typeparam name="TInput">The input type, which must be a class.</typeparam>
/// <typeparam name="TOutput">The output type, which must be a class.</typeparam>
/// <typeparam name="TId">The identifier type.</typeparam>
/// <typeparam name="TOutputList">The list output type, which must be a class.</typeparam>
public interface ICrudAppService<TInput, TOutput, TId, TOutputList>
    : IUpdateAppService<TInput, TOutput, TId>
    , IDeleteAppService<TOutput, TId>
    , IGetByIdAppService<TOutput, TId>
    , IGetListAppService<TOutputList>
    , ICreateAppService<TInput, TOutput>
    where TInput : class
    where TOutput : class
    where TOutputList : class
{
}

/// <summary>
/// Interface for a CRUD (Create, Read, Update, Delete) application service with distinct create and update DTOs.
/// </summary>
/// <typeparam name="TCreateDto">The create dto type, which must be a class.</typeparam>
/// <typeparam name="TUpdateDto">The update dto type, which must be a class.</typeparam>
/// <typeparam name="TOutput">The output type, which must be a class.</typeparam>
/// <typeparam name="TId">The identifier type.</typeparam>
/// <typeparam name="TOutputList">The list output type, which must be a class.</typeparam>
public interface ICrudAppService<TCreateDto, TUpdateDto, TOutput, TId, TOutputList>
    : IUpdateAppService<TUpdateDto, TOutput, TId>
    , IDeleteAppService<TOutput, TId>
    , IGetByIdAppService<TOutput, TId>
    , IGetListAppService<TOutputList>
    , ICreateAppService<TCreateDto, TOutput>
    where TCreateDto : class
    where TUpdateDto : class
    where TOutput : class
    where TOutputList : class
{
}
