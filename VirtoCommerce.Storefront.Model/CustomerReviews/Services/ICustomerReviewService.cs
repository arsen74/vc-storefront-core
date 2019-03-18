using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.CustomerReviews.Services
{
    public interface ICustomerReviewService
    {
        double? GetProductRating(string productId);

        Task<double?> GetProductRatingAsync(string productId, CancellationToken token = default(CancellationToken));

        void CreateReview(CustomerReviewCreateModel newCustomerReview);

        Task CreateReviewAsync(CustomerReviewCreateModel newCustomerReview, CancellationToken token = default(CancellationToken));

        void AddLikeToReview(string productId, string customerReviewId, string userId);

        Task AddLikeToReviewAsync(string productId, string customerReviewId, string userId, CancellationToken token = default(CancellationToken));

        void AddDislikeToReview(string productId, string customerReviewId, string userId);

        Task AddDislikeToReviewAsync(string productId, string customerReviewId, string userId, CancellationToken token = default(CancellationToken));
    }
}
