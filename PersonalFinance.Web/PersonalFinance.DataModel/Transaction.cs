﻿using System;
using System.Collections.Generic;

namespace PersonalFinance.DataModel
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime Waktu { get; set; }
        public string SumberGaji { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}