using System;
using System.Collections.Generic;

namespace PersonalFinance.DataModel
{
    public partial class MoneySource
    {
        public int SourceId { get; set; }
        public int? UserId { get; set; }
        public string SourceName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
