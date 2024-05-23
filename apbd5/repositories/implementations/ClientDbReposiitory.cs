using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd5.Models;
using apbd5.Repositories.Interfaces;

namespace apbd5.Repositories.Implementations
{
    public class ClientDbReposiitory : IClientDbRepository
    {
        private readonly s16896Context context;

        public ClientDbReposiitory(s16896Context context)
        {
            this.context = context;
        }

        public async Task DeleteClientAsync(int idClient)
        {
            bool hasTrips = await context.ClientTrips.AnyAsync(row => row.IdClient == idClient);

            if (hasTrips) throw new Exception("Client has one or more trips!");

            Client client = await context.Clients.Where(row => row.IdClient == idClient).FirstOrDefaultAsync();
            context.Remove(client);

            await context.SaveChangesAsync();
        }
    }
}