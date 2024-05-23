using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd5.Models;
using apbd5.Models.DTO.Request;
using apbd5.Models.DTO.Response;
using apbd5.Repositories.Interfaces;

namespace apbd5.Repositories.Implementations
{
    public class TripDbRepository : ITripDbRepository
    {
        private readonly s16896Context contex;

        public TripDbRepository(s16896Context contex)
        {
            this.contex = contex;
        }

        public async Task AddTripToClientAsync(int idTrip, AddTripToClientRequestDto dto)
        {
            bool ClientExists = await contex.Clients.AnyAsync(row => row.Pesel == dto.Pesel);

            Client wantedClient;
            if (!ClientExists)
            {
                wantedClient = new Client
                {
                    IdClient = await contex.Clients.Select(row => row.IdClient).MaxAsync() + 1,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Telephone = dto.Telephone,
                    Pesel = dto.Pesel
                };

                await contex.Clients.AddAsync(wantedClient);
                await contex.SaveChangesAsync();
            }
            else
            {
                wantedClient = await contex.Clients.FirstOrDefaultAsync(row => row.Pesel == dto.Pesel);
            }


           
            bool TripExists = await contex.Trips.AnyAsync(row => row.IdTrip == idTrip);
            if (!TripExists) throw new Exception($"There is no such trip with ID: {idTrip}!");

            bool isClientAlreadyReservedThisTrip = await contex.ClientTrips.AnyAsync(row => row.IdClient == wantedClient.IdClient && row.IdTrip == idTrip);
            if (isClientAlreadyReservedThisTrip) throw new Exception("Client is already reserved that trip!");


            await contex.ClientTrips.AddAsync(new ClientTrip
            {
                IdClient = wantedClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            });
            await contex.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetTripsResponseDto>> GetTripsAsync()
        {
            var trips = await contex.Trips.Select(row => new GetTripsResponseDto
            {
                Name = row.Name,
                Description = row.Description,
                DateFrom = row.DateFrom,
                DateTo = row.DateTo,
                MaxPeople = row.MaxPeople,
                Countries = row.CountryTrips.Select(row => new CountryResponseDto { Name = row.IdCountryNavigation.Name }),
                Clients = row.ClientTrips.Select(row => new ClientResponseDto
                {
                    FirstName = row.IdClientNavigation.FirstName,
                    LastName = row.IdClientNavigation.LastName
                })
            }).OrderByDescending(column => column.DateFrom).ToListAsync();

            if (!trips.Any()) throw new Exception("There is no data in collection!");

            return trips;
        }
    }
}