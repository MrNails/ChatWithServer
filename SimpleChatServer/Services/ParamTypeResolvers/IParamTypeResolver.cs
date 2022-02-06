namespace SimpleChatServer.Services.ParamTypeResolvers
{
    public interface IParamTypeResolver
    {
        object? Resolve(object? param);
    }
}