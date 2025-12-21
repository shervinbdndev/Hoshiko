using Hoshiko.Infrastructure.Identity;

namespace Hoshiko.Web.Models
{
    public class StageProgressViewModel
    {
        public string StageName { get; set; } = "";
        public bool IsLearnCompleted { get; set; }
        public bool IsQuizCompleted { get; set; }
    }

    public class UserProgressViewModel
    {
        public AppUser User { get; set; } = default!;
        public List<StageProgressViewModel> StagesProgress { get; set; } = new();
        public string? CertificateCode { get; set; }
    }
}
