using System;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public class CustomerReviewCreateModel
    {
        public string AuthorNickname { get; set; }

        public string Content { get; set; }

        public string ProductId { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}
