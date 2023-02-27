using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Internal.Services.Movements.Data.Contexts;
using Internal.Services.Movements.Data.Models;
using Internal.Services.Movements.Data.Models.Enums;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections;
using Internal.Services.Movements.ProxyClients;
using Newtonsoft.Json;
using Internal.Services.Movements.IntegrationTests.Utilities;

namespace Internal.Services.Movements.IntegrationTests
{
    public class IntegrationTest : IClassFixture<TestStartup<Program>>
    {
        private readonly TestStartup<Program> _factory;
        private readonly Utilities.MoqHelper _moq;
        private readonly HttpClient _client;
        public string EndPoint = "https://localhost:7297/v1/GetMovements?";


        public IntegrationTest(TestStartup<Program> factory)
        {
            _factory = factory;
            _moq = new Utilities.MoqHelper(factory.MovementMock);
            _client = _factory.CreateClient();
        }

        // I had to create a new HttpClient because I had to handle 204 response through HTTPClientHandler due to SSL
        [Fact]
        public async Task IncomingMovmentType()
        { 
            //MovementsDataContext mb = new MovementsDataContext();

            var expectedMovement = new Movement { MovementId = 1003, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Unknown, Amount = 17000, AccountFrom = "NL54FAKE0326806738", AccountTo = "NL54FAKE0062046111" };

            string productid = "1";
            string movementtype = "Incoming";
            string pagenumber = "1";
            string pagesize = "10";


           
            // HttpClient client = new HttpClient();
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(httpClientHandler);
            try
            {
                HttpResponseMessage response = await client.GetAsync(EndPoint + "productId=" + productid + "&movementType=" + movementtype + "&pageNumber=" + pagenumber + "&pageSize=" + pagesize);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var actualMovements = JsonConvert.DeserializeObject<GetMovementsResponseDto>(responseBody);

                //Assertion
                Assert.Equal(1, actualMovements.PageNumber);
                Assert.Equal(10, actualMovements.PageSize);
                Assert.Equal(expectedMovement.MovementId, actualMovements.Movements[1].MovementId);
                Assert.Equal(expectedMovement.Account, actualMovements.Movements[1].Account);
                Assert.Equal(expectedMovement.MovementType.ToString(), actualMovements.Movements[1].MovementType);
                Assert.Equal(expectedMovement.Amount, actualMovements.Movements[1].Amount);
                Assert.Equal(expectedMovement.AccountFrom, actualMovements.Movements[1].AccountFrom);
                Assert.Equal(expectedMovement.AccountTo, actualMovements.Movements[1].AccountTo);
                Console.WriteLine($"Read {responseBody.Length} characters");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message} ");
            }
            httpClientHandler.Dispose();
            client.Dispose();
        }

        [Fact]
        public async Task FiscalTransferMovmentType()
        {
            var expectedMovement = new Movement { MovementId = 1015, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Unknown, Amount = 17002, AccountFrom = "NL54FAKE0326806738", AccountTo = "NL54FAKE0062046111" };


            string productid = "1";
            string movementtype = "FiscalTransfer";
            string pagenumber = "1";
            string pagesize = "10";


            HttpClient client = new HttpClient();
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            client = new HttpClient(httpClientHandler);
            try
            {
                HttpResponseMessage response = await client.GetAsync(EndPoint + "productId=" + productid + "&movementType=" + movementtype + "&pageNumber=" + pagenumber + "&pageSize=" + pagesize);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var actualMovements = JsonConvert.DeserializeObject<GetMovementsResponseDto>(responseBody);

                //Assertion
                Assert.Equal(1, actualMovements.PageNumber);
                Assert.Equal(10, actualMovements.PageSize);
                Assert.Equal(expectedMovement.MovementId, actualMovements.Movements[2].MovementId);
                Assert.Equal(expectedMovement.Account, actualMovements.Movements[2].Account);
                Assert.Equal(expectedMovement.MovementType.ToString(), actualMovements.Movements[2].MovementType);
                Assert.Equal(expectedMovement.Amount, actualMovements.Movements[2].Amount);
                Assert.Equal(expectedMovement.AccountFrom, actualMovements.Movements[2].AccountFrom);
                Assert.Equal(expectedMovement.AccountTo, actualMovements.Movements[2].AccountTo);

                Console.WriteLine($"Read {responseBody.Length} characters");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message} ");
            }

            httpClientHandler.Dispose();
            client.Dispose();
        }

        [Fact]
        public async Task InterestPaymentMovmentType()
        {
            var expectedMovement = new Movement { MovementId = 1000, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Interest, Amount = 0.42M, AccountFrom = "SystemFakeInterestAccount", AccountTo = "NL54FAKE0062046111" };

            string productid = "1";
            string movementtype = "Interest";
            string pagenumber = "1";
            string pagesize = "10";


            HttpClient client = new HttpClient();
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            client = new HttpClient(httpClientHandler);
            try
            {
                HttpResponseMessage response = await client.GetAsync(EndPoint + "productId=" + productid + "&movementType=" + movementtype + "&pageNumber=" + pagenumber + "&pageSize=" + pagesize);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var actualMovements = JsonConvert.DeserializeObject<GetMovementsResponseDto>(responseBody);

                Assert.Equal(1, actualMovements.PageNumber);
                Assert.Equal(10, actualMovements.PageSize);
                Assert.Equal(expectedMovement.MovementId, actualMovements.Movements[0].MovementId);
                Assert.Equal(expectedMovement.Account, actualMovements.Movements[0].Account);
                Assert.Equal(expectedMovement.MovementType.ToString(), actualMovements.Movements[0].MovementType);
                Assert.Equal(expectedMovement.Amount, actualMovements.Movements[0].Amount);
                Assert.Equal(expectedMovement.AccountFrom, actualMovements.Movements[0].AccountFrom);
                Assert.Equal(expectedMovement.AccountTo, actualMovements.Movements[0].AccountTo);

                Console.WriteLine($"Read {responseBody.Length} characters");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message: {e.Message} ");
            }

            httpClientHandler.Dispose();
            client.Dispose();
        }


        //Mocks Integration Tests
        [Fact]
        public async Task Mock_WhenPageNumber_PageSizeAreNull()
        {
            try
            {
                var response = await _moq.GetMovement_WhenPageNumber_PageSizeAreNull();
            }
            catch(Exception ex)
            {
                Assert.Equal("pageNumber or pageSize has not been set",ex.Message);
            }
        }
        [Fact]
        public async Task Mock_WhenAllValuesAreNullAndInvalid_shouldReturnException()
        {
            try
            {
                var response = await _moq.GetMovement_WhenAllValuesAreNullAndInvalid_shouldReturnException();
            }
            catch (Exception ex)
            {
                Assert.Equal("pageNumber or pageSize has not been set", ex.Message);
            }
        }
        [Fact]
        public async Task Mock_WhenPageNumber_PageSize_MovementIsSet()
        {
            try
            {
                var response = await _moq.GetMovement_WhenPageNumber_PageSize_MovementIsSet(1, 20, ProxyClients.EnumMovementType.Fee);
                Assert.Equal(1, response.PageNumber);
                Assert.Equal(20, response.PageSize);
                foreach (var item in response.Movements)
                {
                    Assert.Equal(ProxyClients.EnumMovementType.Fee, item.MovementType);
                    Assert.Equal(0.42M, item.Amount);
                    Assert.Equal("SystemFakeInterestAccount", item.AccountFrom);
                    Assert.Equal("NL54FAKE0062046111", item.AccountTo);
                }
            }
            catch (Exception ex)
            {

            }
        }
        [Fact]
        public async Task Mock_WhenAllValuesAreValid()
        {
            try
            {
                var setupMovement = new Movement { MovementId = 1000, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Unknown, Amount = 0.42M, AccountFrom = "SystemFakeInterestAccount", AccountTo = "NL54FAKE0062046111" };
                var response = await _moq.GetMovement_WhenAllValuesAreValid(1, 20, setupMovement, 0);

                Assert.Equal(1, response.PageNumber);
                Assert.Equal(20, response.PageSize);
                foreach (var item in response.Movements)
                {
                    Assert.Equal(setupMovement.MovementId, item.MovementId);
                    Assert.Equal(setupMovement.Account, item.Account);
                    Assert.Equal(setupMovement.MovementType, item.MovementType);
                    Assert.Equal(setupMovement.Amount, item.Amount);
                    Assert.Equal(setupMovement.AccountFrom, item.AccountFrom);
                    Assert.Equal(setupMovement.AccountTo, item.AccountTo);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}