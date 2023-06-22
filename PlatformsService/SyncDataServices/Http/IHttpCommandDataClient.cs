using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface IHttpCommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platformReadDto);
    }
}
