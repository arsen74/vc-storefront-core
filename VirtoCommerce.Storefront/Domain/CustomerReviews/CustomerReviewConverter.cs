
namespace VirtoCommerce.Storefront.Domain.CustomerReviews
{
    public static class CustomerReviewConverter
    {
        public static Model.CustomerReviews.CustomerReview ToCustomerReview(this AutoRestClients.CustomerReviewsModuleApi.Models.CustomerReview itemDto)
        {
            return new Model.CustomerReviews.CustomerReview
            {
                AuthorNickname = itemDto.AuthorNickname,
                Content = itemDto.Content,
                CreatedBy = itemDto.CreatedBy,
                CreatedDate = itemDto.CreatedDate.GetValueOrDefault(),
                DislikeCount = itemDto.DislikeCount.GetValueOrDefault(),
                Id = itemDto.Id,
                IsActive = itemDto.IsActive.GetValueOrDefault(),
                LikeCount = itemDto.LikeCount.GetValueOrDefault(),
                ModifiedBy = itemDto.ModifiedBy,
                ModifiedDate = itemDto.ModifiedDate,
                ProductId = itemDto.ProductId,
                Rating = itemDto.Rating.GetValueOrDefault()
            };
        }

        public static AutoRestClients.CustomerReviewsModuleApi.Models.CustomerReview ToCustomerReview(this Model.CustomerReviews.CustomerReviewCreateModel model)
        {
            return new AutoRestClients.CustomerReviewsModuleApi.Models.CustomerReview
            {
                AuthorNickname = model.AuthorNickname,
                Content = model.Content,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                ProductId = model.ProductId,
                Rating = model.Rating
            };
        }

        public static AutoRestClients.CustomerReviewsModuleApi.Models.CustomerReviewSearchCriteria ToSearchCriteriaDto(this Model.CustomerReviews.CustomerReviewSearchCriteria searchCriteria)
        {
            return new AutoRestClients.CustomerReviewsModuleApi.Models.CustomerReviewSearchCriteria
            {
                IsActive = searchCriteria.IsActive,
                ProductIds = searchCriteria.ProductIds,
                Skip = searchCriteria.Start,
                Take = searchCriteria.PageSize,
                Sort = searchCriteria.Sort
            };
        }
    }
}
