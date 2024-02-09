using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.master
{
    public interface IGobalObjectionService
    {
        public Task<List<ObjectionDto>> AllObjections();
    }
}