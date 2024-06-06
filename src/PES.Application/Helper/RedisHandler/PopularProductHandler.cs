using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Application.Helper.RedisHandler
{
    public class PopularProductHandler : IDisposable
    {
        private readonly IDatabase _database;
        private const int Month = 30 * 86400;

        public PopularProductHandler(IDatabase database)
        {
            _database = database;
        }



        //?Prevent User Voting in Month Duplicate
        //todo +1 view

        public async ValueTask<int> AddUserViewProduct(string userId, Guid productId)
        {
            if (await _database.SetAddAsync(productId + "_View", userId))
            {
                return 0;
            }
            else
            {
                return 1;

            }
        }

        //todo +2 addTocart
        public async ValueTask<int> AddUserAddToCart(string userId, Guid productId)
        {
            if (await _database.SetAddAsync(productId + "_AddCart", userId))
            {
                return 0;
            }
            else
            {
                return 2;

            }
        }

        //todo +3 addToOrder
        public async ValueTask<int> AddUserOrderProduct(string userId, Guid productId)
        {
            if (await _database.SetAddAsync(productId + "_AddOrder", userId))
            {
                return 0;
            }
            else
            {
                return 3;

            }
        }


        public async Task ProductVoting(string userId, Guid productId, int productType)
        {
            int point = 0;
            switch (productType)
            {
                case 1: point = await AddUserViewProduct(userId, productId); break;
                case 2: point = await AddUserAddToCart(userId, productId); break;
                case 3: point = await AddUserOrderProduct(userId, productId); break;
                default:
                    break;
            }
            string CurrentMonth = string.Format("{0:MMMM}", DateTime.Now.AddHours(7));
            await _database.SortedSetIncrementAsync($"Voting_${CurrentMonth}", productId.ToString(), point);
        }


        public async Task<SortedSetEntry[]> ProductRatingList(string month)
        {
            return await _database.SortedSetRangeByRankWithScoresAsync($"Voting_${month}");


        }





        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
