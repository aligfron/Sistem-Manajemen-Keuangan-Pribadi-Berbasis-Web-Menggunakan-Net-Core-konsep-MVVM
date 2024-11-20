using System;
using System.Collections.Generic;

namespace PersonalFinance.DataModel
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int? UserId { get; set; }
        public string ReportType { get; set; } = null!;
        public DateTime? GeneratedAt { get; set; }
        public string? ReportData { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
