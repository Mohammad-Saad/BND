using Internal.Services.Movements.Data.Contexts;
using Internal.Services.Movements.Data.Models;
using Internal.Services.Movements.Data.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
    public class DbHelper
	{
		private readonly MovementsDataContext _movementsDb;

		public DbHelper(MovementsDataContext movementsDb)
		{
			_movementsDb = movementsDb;
		}

		//Assuming that the ProductID, CustomerID and the ProductCustomerID is autogenrated by the database
		public void InitializeDbForTests()
		{
			//Since the ProductCustomers is nullable I did not enter the List of ProductCustomers 
			var products = new List<Product>
				{
					new Product { ProductType = EnumProductType.SavingsRetirement, ExternalAccount = "A-001" , },
					new Product { ProductType = EnumProductType.Unknown, ExternalAccount = "B-001" },
					new Product { ProductType = EnumProductType.SavingsRetirement, ExternalAccount = "C-001" }
				};
			if (_movementsDb.Products.Count() == 0)
			{
				_movementsDb.Products.AddRange(products);
			}

			//Since the ProductCustomers is nullable I did not enter the List of ProductCustomers 
			var customers = new List<Customer>
			{
					new Customer { CustomerFirstName = "John", CustomerLastName = "Doe", CustomerEmail = "john.doe@example.com" },
					new Customer { CustomerFirstName = "Jane", CustomerLastName = "Smith", CustomerEmail = "jane.smith@example.com" },
					new Customer { CustomerFirstName = "Bob", CustomerLastName = "Johnson", CustomerEmail = "bob.johnson@example.com" }
			 };

			if (_movementsDb.Customers.Count() == 0)
			{
				_movementsDb.Customers.AddRange(customers);
			}
			

			var productCustomers = new List<ProductCustomer>
				{
					new ProductCustomer { ProductId = products[0].ProductId, CustomerId = customers[0].CustomerId },
					new ProductCustomer { ProductId = products[0].ProductId, CustomerId = customers[0].CustomerId },
					new ProductCustomer { ProductId = products[1].ProductId, CustomerId = customers[1].CustomerId },
					new ProductCustomer { ProductId = products[2].ProductId, CustomerId = customers[2].CustomerId }
				};

			if (_movementsDb.ProductsCustomers.Count() == 0)
			{
				_movementsDb.ProductsCustomers.AddRange(productCustomers);
			}
			
			_movementsDb.SaveChanges();

			int count = _movementsDb.Products.Count();

		}
	}
}
