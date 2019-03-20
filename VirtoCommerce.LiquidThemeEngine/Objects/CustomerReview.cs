using System;
using System.Globalization;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class CustomerReview : Drop
    {
        public string Id { get; set; }

        public string AuthorNickname { get; set; }

        public string Content { get; set; }

        public string ProductId { get; set; }

        public int Rating { get; set; }

        public string RatingPercentage => ((Rating / 5d) * 100).ToString(CultureInfo.InvariantCulture);

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public long? CreatedDateTick => CreatedDate?.Ticks;

        public string LocalizedCreatedDate => CreatedDate?.ToShortDateString();

        public DateTime? CreatedDate { get; set; }
    }
}
