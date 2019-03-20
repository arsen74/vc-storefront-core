using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerReviewStaticConverter
    {
        public static CustomerReview ToShopifyModel(this storefrontModel.CustomerReview item)
        {
            return new ShopifyModelConverter().ToLiquidCustomerReview(item);
        }
    }

    public partial class ShopifyModelConverter
    {
        public virtual CustomerReview ToLiquidCustomerReview(storefrontModel.CustomerReview item)
        {
            return new CustomerReview
            {
                Id = item.Id,
                AuthorNickname = item.AuthorNickname,
                Content = item.Content,
                CreatedDate = item.CreatedDate,
                Rating = item.Rating,
                LikeCount = item.LikeCount,
                DislikeCount = item.DislikeCount,
                ProductId = item.ProductId
            };
        }
    }
}
