using System.ComponentModel.DataAnnotations;

namespace Jason.WebApi
{
    public class VersionInfo
    {

        [Key]
        public long Version { get; set; }
        public DateTime AppliedOn { get; set; }
        public string Description { get; set; }
    }
}
