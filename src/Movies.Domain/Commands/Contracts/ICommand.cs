using MediatR;
using Movies.Domain.Results;

namespace Movies.Domain.Commands.Contracts;
public interface ICommand : IRequest<GenericCommandResult>
{

}
