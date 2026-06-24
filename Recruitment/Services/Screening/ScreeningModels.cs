using System.Text.Json.Serialization;

namespace Recruitment.API.Services.Screening
{
    public class ScreeningReport
    {
        [JsonPropertyName("match_score")]
        public int MatchScore { get; set; }

        [JsonPropertyName("verdict")]
        public string Verdict { get; set; } = string.Empty;

        [JsonPropertyName("seniority")]
        public string Seniority { get; set; } = string.Empty;

        [JsonPropertyName("years_experience")]
        public float YearsExperience { get; set; }

        [JsonPropertyName("must_have_met")]
        public List<string> MustHaveMet { get; set; } = new();

        [JsonPropertyName("must_have_missing")]
        public List<string> MustHaveMissing { get; set; } = new();

        [JsonPropertyName("nice_to_have_met")]
        public List<string> NiceToHaveMet { get; set; } = new();

        [JsonPropertyName("strengths")]
        public List<string> Strengths { get; set; } = new();

        [JsonPropertyName("concerns")]
        public List<string> Concerns { get; set; } = new();

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonPropertyName("reasoning")]
        public string Reasoning { get; set; } = string.Empty;
    }

    public class RankingItem
    {
        [JsonPropertyName("cv_path")]
        public string CvPath { get; set; } = string.Empty;

        [JsonPropertyName("ime_kandidata")]
        public string ImeKandidata { get; set; } = string.Empty;

        [JsonPropertyName("ocena")]
        public ScreeningReport Ocena { get; set; } = new();
    }

    public class RankingResponse
    {
        [JsonPropertyName("ranglista")]
        public List<RankingItem> Ranglista { get; set; } = new();

        [JsonPropertyName("hr_preporuka")]
        public string HrPreporuka { get; set; } = string.Empty;
    }

    public class RankingRequest
    {
        [JsonPropertyName("cv_paths")]
        public List<string> CvPaths { get; set; } = new();

        [JsonPropertyName("requirements")]
        public string Requirements { get; set; } = string.Empty;

        [JsonPropertyName("imena")]
        public Dictionary<string, string> Imena { get; set; } = new();
    }
}
