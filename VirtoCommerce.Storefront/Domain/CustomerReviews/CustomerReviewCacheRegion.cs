using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Storefront.Model.Common.Caching;

namespace VirtoCommerce.Storefront.Domain.CustomerReviews
{
    public class CustomerReviewCacheRegion : CancellableCacheRegion<CustomerReviewCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _customerReviewRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateCustomerCustomerReviewChangeToken(string productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            var cancellationTokenSource = _customerReviewRegionTokenLookup.GetOrAdd(productId, new CancellationTokenSource());

            return new CompositeChangeToken(new[] { CreateChangeToken(), new CancellationChangeToken(cancellationTokenSource.Token) });
        }

        public static void ExpireCustomerCustomerReview(string productId)
        {
            if (!string.IsNullOrEmpty(productId) && _customerReviewRegionTokenLookup.TryRemove(productId, out var token))
            {
                token.Cancel();
            }
        }
    }
}
