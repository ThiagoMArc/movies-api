using MediatR;
using Movies.Domain.Results;

namespace Movies.Domain.Queries.Contracts;

public interface IQuery : IRequest<GenericQueryResult>
{

}
