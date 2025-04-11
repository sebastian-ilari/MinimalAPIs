using Models;
using Persistence;
using Services;

namespace Api.Endpoints
{
    class CreateTodoRequest
    {
        public TodoDto TodoDto { get; init; } = default!;
        public TodoDb Db { get; init; } = default!;
        public ISecretService SecretService { get; init; } = default!;
    }
}
