
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.master
{
    public interface ILocalObjectionService
    {
        public Task<List<ObjectionDto>> AllObjections();
        public Task<bool> Insert(string description);
    }
}