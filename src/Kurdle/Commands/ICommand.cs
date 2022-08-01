using System.Threading;
using System.Threading.Tasks;

namespace Kurdle.Commands
{
    public interface ICommand<T>
    {
        Task<int> ExecuteAsync(T options, CancellationToken cancellationToken);
    }
}
