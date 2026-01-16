using Domain.Entities;
using Domain.Entities.Runtime;
using MediatR;

namespace Application.Handlers;

public class BaseRequest<T> : IRequest<T>
{
    public RuntimeUser User { get; set; }
}