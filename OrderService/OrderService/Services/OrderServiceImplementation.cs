using Grpc.Core;
using GrpcExample;

namespace OrderService.Services;

public class OrderServiceImplementation : GrpcExample.OrderService.OrderServiceBase
{
    public override Task<OrderResponse> CreateOrder(OrderRequest request, ServerCallContext context)
    {
        return Task.FromResult(new OrderResponse
        {
            Status = $"Order for {request.Product} placed successfully by {request.Username}."
        });
    }
}