using System;

namespace USPSystem.ViewModels
{
    public class HoldStatusViewModel
    {
        public bool IsOnHold { get; set; }
        public string HoldReason { get; set; }
        public DateTime? HoldStartDate { get; set; }
        public DateTime? HoldEndDate { get; set; }
        public string HoldPlacedBy { get; set; }
    }
} 