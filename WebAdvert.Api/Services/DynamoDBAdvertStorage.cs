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
        private readonly AmazonDynamoDBClient _client;

        public DynamoDbAdvertStorage(IMapper mapper, IAmazonDynamoDB client)
        {
            _mapper = mapper;
            _client = (AmazonDynamoDBClient) client;
        }

        public async Task<string> CreateAdvert(AdvertModel model)
        {
            var dbModel = _mapper.Map<AdvertDTO>(model);
            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationDate = DateTime.UtcNow;
            dbModel.Status = AdvertStatus.Pending;
            using (var context = new DynamoDBContext(_client))
            {
                await context.SaveAsync(dbModel);
            }
            return dbModel.Id;
        }

        public async Task ConfirmAdvert(AdvertConfirmModel model)
        {
            using (var context = new DynamoDBContext(_client))
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

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                var tableData = await _client.DescribeTableAsync("Adverts");
                return string.Compare(tableData.Table.TableStatus, "active", true) == 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return false;
        }

        public async Task<string> HealthAsync()
        {
            try
            {
                var tableData = await _client.DescribeTableAsync("Adverts");
                return tableData.Table.TableStatus.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message + " || " + e.StackTrace;
            }
        }
    }
}
