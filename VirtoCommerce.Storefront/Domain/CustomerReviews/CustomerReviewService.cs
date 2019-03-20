using System;
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
    public class CustomerReviewService : ICustomerReviewSearchService, ICustomerReviewService
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

        public void AddDislikeToReview(string productId, string customerReviewId, string userId)
        {
            AddDislikeToReviewAsync(productId, customerReviewId, userId)
                .GetAwaiter()
                .GetResult();
        }

        public async Task AddDislikeToReviewAsync(string productId, string customerReviewId, string userId, CancellationToken token = default(CancellationToken))
        {
            await _customerReviewApi.DislikeWithHttpMessagesAsync(
                new API.Models.CustomerReviewAppraisalModel
                {
                    Appraisal = 1,
                    ReviewId = customerReviewId,
                    UserId = userId
                },
                cancellationToken: token);

            CustomerReviewCacheRegion.ExpireCustomerCustomerReview(productId);
        }

        public void AddLikeToReview(string productId, string customerReviewId, string userId)
        {
            AddLikeToReviewAsync(productId, customerReviewId, userId)
                .GetAwaiter()
                .GetResult();
        }

        public async Task AddLikeToReviewAsync(string productId, string customerReviewId, string userId, CancellationToken token = default(CancellationToken))
        {
            await _customerReviewApi.LikeWithHttpMessagesAsync(
                new API.Models.CustomerReviewAppraisalModel
                {
                    Appraisal = 1,
                    ReviewId = customerReviewId,
                    UserId = userId
                },
                cancellationToken: token);

            CustomerReviewCacheRegion.ExpireCustomerCustomerReview(productId);
        }

        public void CreateReview(CustomerReviewCreateModel newCustomerReview)
        {
            CreateReviewAsync(newCustomerReview)
                .GetAwaiter()
                .GetResult();
        }

        public async Task CreateReviewAsync(CustomerReviewCreateModel newCustomerReview, CancellationToken token = default(CancellationToken))
        {
            newCustomerReview.CreatedDate = DateTimeOffset.UtcNow.DateTime;

            await _customerReviewApi.UpdateWithHttpMessagesAsync(
                new[] { newCustomerReview.ToCustomerReview() },
                cancellationToken: token);

            CustomerReviewCacheRegion.ExpireCustomerCustomerReview(newCustomerReview.ProductId);
        }

        public double? GetProductRating(string productId)
        {
            return GetProductRatingAsync(productId)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<double?> GetProductRatingAsync(string productId, CancellationToken token = default(CancellationToken))
        {
            var cacheKey = CacheKey.With(typeof(CustomerReviewService), nameof(GetProductRatingAsync), productId);

            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateCustomerCustomerReviewChangeToken(productId));
                cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviewApi.GetProductRatingWithHttpMessagesAsync(productId, cancellationToken: token);

                return result.Body.Rating;
            });
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
                for (int i = 0; i < criteria.ProductIds.Length; i++)
                {
                    cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateCustomerCustomerReviewChangeToken(criteria.ProductIds[i]));
                }
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
