
namespace VirtoCommerce.Storefront.Domain.CustomerReviews
{
    public static class CustomerReviewConverter
    {
        public static Model.CustomerReviews.CustomerReview ToCustomerReview(this AutoRestClients.CustomerReviewsModuleApi.CustomerReview itemDto)
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
                Rating = itemDto.Rating.GetValueOrDefault(),
                ReviewPhotoPath = itemDto.ReviewPhotoPath
            };
        }

        public static AutoRestClients.CustomerReviewsModuleApi.CustomerReviewSearchCriteria ToSearchCriteriaDto(this Model.CustomerReviews.CustomerReviewSearchCriteria searchCriteria)
        {
            return new AutoRestClients.CustomerReviewsModuleApi.CustomerReviewSearchCriteria
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
