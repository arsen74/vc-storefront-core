using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.CustomerReviews;
using VirtoCommerce.Storefront.Model.CustomerReviews.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [StorefrontApiRoute]
    public class ApiProductReviewController : StorefrontControllerBase
    {
        private readonly ICustomerReviewService _customerReviewService;

        public ApiProductReviewController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder, ICustomerReviewService customerReviewService)
            : base(workContextAccessor, urlBuilder)
        {
            _customerReviewService = customerReviewService;
        }

        [HttpPost("product/{productId}/review")]
        public ActionResult CreateCustomerReview([FromRoute] string productId, [FromBody] CustomerReviewCreateModel customerReviewCreateModel)
        {
            customerReviewCreateModel.ProductId = productId;

            _customerReviewService.CreateReview(customerReviewCreateModel);

            return NoContent();
        }

        [HttpPost("product/{productId}/review/{reviewId}/appraisal")]
        public ActionResult AppraisalCustomerReview(string productId, string reviewId, [FromBody] CustomerReviewAppraisalModel model)
        {
            if (model?.Appraisal > 0)
            {
                _customerReviewService.AddLikeToReview(productId, reviewId, model?.UserId);
            }
            else
            {
                _customerReviewService.AddDislikeToReview(productId, reviewId, model?.UserId);
            }

            return NoContent();
        }
    }
}
