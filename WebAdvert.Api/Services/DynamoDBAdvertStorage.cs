using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models; 
using AutoMapper; 
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAdvert.Api.Services
{
    public class DynamoDbAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper _mapper;

        public DynamoDbAdvertStorage(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CreateAdvert(AdvertModel model)
        {
            var dbModel = _mapper.Map<AdvertDTO>(model);
            dbModel.Id = new Guid().ToString();
            dbModel.CreationDate = DateTime.UtcNow;
            dbModel.Status = AdvertStatus.Pending;
            using var client = new AmazonDynamoDBClient();
            using (var context = new DynamoDBContext(client))
            {
                await context.SaveAsync(dbModel);
            }
            return dbModel.Id;
        }

        public async Task ConfirmAdvert(AdvertConfirmModel model)
        {
            using var client = new AmazonDynamoDBClient();
            using (var context = new DynamoDBContext(client))
            {
                var record = await context.LoadAsync<AdvertDTO>(model.Id);
                if(record == null)
                    throw new KeyNotFoundException($"A record with ID= {model.Id} was not found.");
                if (model.Status == AdvertStatus.Active)
                {
                    record.Status = AdvertStatus.Active;
                    await context.SaveAsync(record);
                }
                else
                {
                    await context.DeleteAsync(record);
                }
            }
        }
    }
}
