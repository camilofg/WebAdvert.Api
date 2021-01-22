using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;
using Amazon.DynamoDBv2.DataModel;

namespace WebAdvert.Api.Services
{
    [DynamoDBTable("Adverts")]
    public class AdvertDTO
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public double Price { get; set; }

        [DynamoDBProperty]
        public DateTime CreationDate { get; set; }

        [DynamoDBProperty]
        public AdvertStatus Status { get; set; }
    }
}
