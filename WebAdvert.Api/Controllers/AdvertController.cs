using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;
using Amazon.DynamoDBv2;
using WebAdvert.Api.Services;

namespace WebAdvert.Api.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertStorageService _advertStorage;

        public AdvertController(IAdvertStorageService advertStorage)
        {
            _advertStorage = advertStorage;
        }

        [HttpPost]
        [Route("Create")] 
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type=typeof(CreateAdvertResponse))]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            //string recordId;
            try
            {
                var recordId = await _advertStorage.CreateAdvert(model);
                return StatusCode(201, new CreateAdvertResponse { Id = recordId });
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("Confirm")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(AdvertConfirmModel model)
        {
            
            try
            {
                await _advertStorage.ConfirmAdvert(model);
                return new OkResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
                throw;
            }
        }


        [HttpGet]
        [Route("CheckHealth")]
        public async Task<IActionResult> Health()
        {
            try
            {
                var result = await _advertStorage.HealthCheckAsync();
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
           
        }
    }
}
