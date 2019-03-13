using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PagedList.Core;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.CustomerReviews;
using VirtoCommerce.Storefront.Model.CustomerReviews.Services;
using API = VirtoCommerce.Storefront.AutoRestClients.CustomerReviewsModuleApi;

namespace VirtoCommerce.Storefront.Domain.CustomerReviews
{
    public class CustomerReviewService : ICustomerReviewSearchService
    {
        private readonly API.ICustomerReviews _customerReviewApi;
        private readonly IStorefrontMemoryCache _memoryCache;
        private readonly IApiChangesWatcher _apiChangesWatcher;

        public CustomerReviewService(API.ICustomerReviews customerReviewApi, IStorefrontMemoryCache memoryCache, IApiChangesWatcher apiChangesWatcher)
        {
            _customerReviewApi = customerReviewApi;

            _memoryCache = memoryCache;

            _apiChangesWatcher = apiChangesWatcher;
        }

        public IPagedList<CustomerReview> SearchCustomerReviews(CustomerReviewSearchCriteria criteria)
        {
            return SearchCustomerReviewsAsync(criteria)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IPagedList<CustomerReview>> SearchCustomerReviewsAsync(CustomerReviewSearchCriteria criteria, CancellationToken token = default(CancellationToken))
        {
            var cacheKey = CacheKey.With(typeof(CustomerReviewService), nameof(SearchCustomerReviewsAsync), criteria.GetCacheKey());

            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());
                cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviewApi.SearchCustomerReviewsWithHttpMessagesAsync(criteria.ToSearchCriteriaDto(), cancellationToken: token);

                return new StaticPagedList<CustomerReview>(
                    subset: result.Body.Results.Select(p => p.ToCustomerReview()),
                    pageNumber: criteria.PageNumber,
                    pageSize: criteria.PageSize,
                    totalItemCount: result.Body.TotalCount.GetValueOrDefault());
            });
        }
    }
}
