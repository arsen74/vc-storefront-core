using System.Threading;
using System.Threading.Tasks;
using PagedList.Core;

namespace VirtoCommerce.Storefront.Model.CustomerReviews.Services
{
    public interface ICustomerReviewSearchService
    {
        IPagedList<CustomerReview> SearchCustomerReviews(CustomerReviewSearchCriteria criteria);

        Task<IPagedList<CustomerReview>> SearchCustomerReviewsAsync(CustomerReviewSearchCriteria criteria, CancellationToken token = default(CancellationToken));
    }
}
