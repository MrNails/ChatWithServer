using System.Threading.Tasks;
using Serilog;
using SimpleChatServer.Core.Models;

namespace SimpleChatServer.Services.Extensions
{
    public static class TaskExtensions
    {
        public static Task HandleTaskErrorAsync(this Task source)
        {
            return source.ContinueWith(result =>
            {
                if (result.IsFaulted)
                    Log.Logger.Error(result.Exception, string.Empty);
            });
        }
    }
}