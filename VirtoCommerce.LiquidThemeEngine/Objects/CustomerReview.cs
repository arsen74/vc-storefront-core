using System;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class CustomerReview : Drop
    {
        public string AuthorNickname { get; set; }

        public string Content { get; set; }

        public string ProductId { get; set; }

        public int Rating { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public long? CreatedDateTick => CreatedDate?.Ticks;

        public string LocalizedCreatedDate => CreatedDate?.ToShortDateString();

        public DateTime? CreatedDate { get; set; }
    }
}
