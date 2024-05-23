using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd5.Repositories.Interfaces
{
    public interface IClientDbRepository
    {
        Task DeleteClientAsync(int idClient);
    }
}