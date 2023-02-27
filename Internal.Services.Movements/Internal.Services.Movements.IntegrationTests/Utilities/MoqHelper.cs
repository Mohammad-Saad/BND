using Internal.Services.Movements.ProxyClients;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
    public class MoqHelper
    {

        private readonly Mock<IMovementsClient> _movementsMock;

        public MoqHelper(Mock<IMovementsClient> movementsMock)
        {
            _movementsMock = movementsMock;
        }


        //When valid input parameters are passed, the function returns a valid response.
        public async Task<PagedMovements> GetMovement_WhenPageNumber_PageSize_MovementIsSet(int pageNumber , int pageSize , EnumMovementType movmenttype)
        {
            try
            {
                var pagedMovements = new ProxyClients.PagedMovements
                {
                    PageNumber = 1,
                    PageSize = 20,
                    Movements = new List<Movement>
                    {
                        new Movement
                        {
                            MovementId = 1000,
                            Account = "NL54FAKE0062046111",
                            MovementType = movmenttype,
                            Amount = 0.42M,
                            AccountFrom = "SystemFakeInterestAccount",
                            AccountTo = "NL54FAKE0062046111"
                        }
                    }
                };

                var setupMovement = new Movement { MovementId = 1000, Account = "NL54FAKE0062046111", MovementType = movmenttype, Amount = 0.42M, AccountFrom = "SystemFakeInterestAccount", AccountTo = "NL54FAKE0062046111" };

                _movementsMock.Setup(x => x.GetMovementsAsync(pageNumber, pageSize, It.IsAny<string>(), movmenttype, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                    .ReturnsAsync(pagedMovements);

                var result = await _movementsMock.Object.GetMovementsAsync(pageNumber, pageSize, setupMovement.Account, setupMovement.MovementType, setupMovement.AccountFrom, setupMovement.AccountTo, 0, 1000);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        //When the pageNumber and pageSize are null function should return exception, 
        public async Task<PagedMovements> GetMovement_WhenPageNumber_PageSizeAreNull()
        {
                var setupMovement = new Movement { MovementId = 1000, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Unknown, Amount = 0.42M, AccountFrom = "SystemFakeInterestAccount", AccountTo = "NL54FAKE0062046111" };

                _movementsMock.Setup(x => x.GetMovementsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), EnumMovementType.Unknown, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                    .ThrowsAsync(new Exception("pageNumber or pageSize has not been set"));

                var result = await _movementsMock.Object.GetMovementsAsync(0, 0, setupMovement.Account, setupMovement.MovementType, setupMovement.AccountFrom, setupMovement.AccountTo, 0, 1000);

                return result;
            
        }

        //When all values are invalid or empty should return exception
        public async Task<PagedMovements> GetMovement_WhenAllValuesAreNullAndInvalid_shouldReturnException()
        {
            var setupMovement = new Movement { MovementId = 1000, Account = "NL54FAKE0062046111", MovementType = ProxyClients.EnumMovementType.Unknown, Amount = 0.42M, AccountFrom = "SystemFakeInterestAccount", AccountTo = "NL54FAKE0062046111" };

            _movementsMock.Setup(x => x.GetMovementsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), EnumMovementType.Unknown, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                .ThrowsAsync(new Exception("pageNumber or pageSize has not been set"));

            var result = await _movementsMock.Object.GetMovementsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<EnumMovementType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>());

            return result;

        }

        // When all values are valid should return a response
        public async Task<PagedMovements> GetMovement_WhenAllValuesAreValid(int pagenumber, int pagesize, Movement setupMovement , decimal amountfrom)
        {
            var pagedMovements = new ProxyClients.PagedMovements
            {
                PageNumber = 1,
                PageSize = 20,
                Movements = new List<Movement>
                {
                    new Movement
                    {
                        MovementId = 1000,
                        Account = "NL54FAKE0062046111",
                        MovementType = setupMovement.MovementType,
                        Amount = 0.42M,
                        AccountFrom = "SystemFakeInterestAccount",
                        AccountTo = "NL54FAKE0062046111"
                    }
                }
            };
            
            _movementsMock.Setup(x => x.GetMovementsAsync(pagenumber, pagesize, setupMovement.Account, setupMovement.MovementType, setupMovement.AccountFrom, setupMovement.AccountTo, amountfrom, setupMovement.Amount))
                .ReturnsAsync(pagedMovements);

            var result = await _movementsMock.Object.GetMovementsAsync(pagenumber, pagesize, setupMovement.Account, setupMovement.MovementType, setupMovement.AccountFrom, setupMovement.AccountTo, amountfrom, setupMovement.Amount);

            return result;

        }
    }
}