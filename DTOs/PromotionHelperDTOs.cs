namespace BE_MEGA_PROJECT.DTOs
{
    public class PromotionHelperDTOs
    {
        public class DropdownOptionDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class NeighborhoodDropdownDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int CityId { get; set; }
            public string CityName { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
        }

        public class BranchDropdownDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
        }

        public class PackageDropdownDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<string> Services { get; set; } = new();
            public string ServicesDisplay { get; set; } = string.Empty;
        }

        public class ServiceDropdownDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal MonthlyPrice { get; set; }
            public decimal SetupPrice { get; set; }
            public string DisplayName { get; set; } = string.Empty;
        }

        public class TargetTypeOptionDTO
        {
            public string Value { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class DiscountTypeOptionDTO
        {
            public string Value { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class AppliesToOptionDTO
        {
            public string Value { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class CreatePromotionDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string DiscountType { get; set; } = string.Empty;
            public decimal DiscountAmount { get; set; }
            public string TargetType { get; set; } = string.Empty;
            public int TargetId { get; set; }
            public string AppliesTo { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int? ServiceId { get; set; }
        }

        public class PromotionValidationDTO
        {
            public bool IsValid { get; set; }
            public List<string> Messages { get; set; } = new();
        }
    }
}
