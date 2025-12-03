using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using BejanIonelaLab7.Models;

namespace BejanIonelaLab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }

      
        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
                return _database.UpdateAsync(product);
            else
                return _database.InsertAsync(product);
        }

        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product);
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<int> SaveListProductAsync(ListProduct listp)
        {
            if (listp.ID != 0)
                return _database.UpdateAsync(listp);
            else
                return _database.InsertAsync(listp);
        }

        public Task<List<Product>> GetListProductsAsync(int shoplistid)
        {
            return _database.QueryAsync<Product>(
                "select P.ID, P.Description from Product P " +
                "inner join ListProduct LP on P.ID = LP.ProductID " +
                "where LP.ShopListID = ?", shoplistid);
        }

        public Task<int> DeleteListProductByIdsAsync(int shopListId, int productId)
        {
            return _database.ExecuteAsync(
                "DELETE FROM ListProduct WHERE ShopListID = ? AND ProductID = ?",
                shopListId, productId);
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<int> SaveShoppingListAsync(ShopList shopList)
        {
            if (shopList.ID != 0)
                return _database.UpdateAsync(shopList);
            else
                return _database.InsertAsync(shopList);
        }

        public Task<int> DeleteShoppingListAsync(ShopList shopList)
        {
            return _database.DeleteAsync(shopList);
        }

        public Task<List<Shop>> GetShopsAsync()
        {
            return _database.Table<Shop>().ToListAsync();
        }

        public Task<int> SaveShopAsync(Shop shop)
        {
            if (shop.ID != 0)
            {
                return _database.UpdateAsync(shop);
            }
            else
            {
                return _database.InsertAsync(shop);
            }
        }

        public Task<int> DeleteShopAsync(Shop shop)
        {
            return _database.DeleteAsync(shop);
        }
    }
}
