using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;

namespace WebAdvert.Api.Services
{
    public interface IAdvertStorageService
    {
        public Task<string> CreateAdvert(AdvertModel model);

        public Task ConfirmAdvert(AdvertConfirmModel model);

        public Task<bool> HealthCheckAsync();
    }
}
