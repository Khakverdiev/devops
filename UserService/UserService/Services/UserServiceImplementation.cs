using Grpc.Core;
using GrpcExample;

namespace UserService.Services;

public class UserServiceImplementation  : GrpcExample.UserService.UserServiceBase
{
    public override Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
    {
        return Task.FromResult(new UserResponse
        {
            Message = $"Hello, {request.Username}! Welcome to UserService."
        });
    }
}