using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd5.Models.DTO.Request;
using apbd5.Models.DTO.Response;

namespace apbd5.Repositories.Interfaces
{
    public interface ITripDbRepository
    {
        Task<IEnumerable<GetTripsResponseDto>> GetTripsAsync();
        Task AddTripToClientAsync(int idTrip, AddTripToClientRequestDto dto);
    }
}