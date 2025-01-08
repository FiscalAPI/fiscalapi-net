using System;

namespace Fiscalapi.Common
{
    public abstract class BaseDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CatalogDto : BaseDto, ICatalog
    {
        public string Description { get; set; }
    }

    public interface ICatalog
    {
        string Id { get; set; }
        string Description { get; set; }
    }
}