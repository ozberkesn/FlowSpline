using FlowSpline.Application.ExecutionEngine.DTOs;
using MediatR;

namespace FlowSpline.Application.ExecutionEngine.GetExecution;

public sealed record GetExecutionQuery(Guid Id) : IRequest<ExecutionRunDto?>;
