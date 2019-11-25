using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FabsAFTrigBindSess.Utilities
{
    public partial class ColleagueAccolade
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        [JsonProperty("colleagueName")]
        public string ColleagueName { get; set; }

        [JsonProperty("colleagueUPNEmail")]
        public string ColleagueUPNEmail { get; set; }

        [JsonProperty("accoladeGivenDate")]
        public string AccoladeGivenDate { get; set; } = DateTime.Now.ToString();

        [JsonProperty("accoladeStatement")]
        public string AccoladeStatement { get; set; }

        [JsonProperty("myName")]
        public string MyName { get; set; }

        [JsonProperty("myUPNEmail")]
        public string MyUPNEmail { get; set; }

        [JsonProperty("accoladeLifeValidStatus")]
        public bool AccoladeLifeValidStatus { get; set; } = true;

        [JsonProperty("workBucksWorthy")]
        public bool WorkBucksWorthy { get; set; }

        [JsonProperty("workBucksAmount")]
        public string WorkBucksAmount { get; set; }

        [JsonProperty("workBucksId")]
        public string WorkBucksId { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

    }

    public class AccoladeTable : ColleagueAccolade
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
    public partial class FabsSession
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        [JsonProperty("sessionNumber")]
        public string SessionNumber { get; set; }

        [JsonProperty("sessionName")]
        public string SessionName { get; set; }

        [JsonProperty("sessionDate")]
        public string SessionDate { get; set; }

        [JsonProperty("sessionCity")]
        public string SessionCity { get; set; }

        [JsonProperty("sessionRegionState")]
        public string SessionRegionState { get; set; }

        [JsonProperty("sessionCountry")]
        public string SessionCountry { get; set; }

        [JsonProperty("review")]
        public ReviewOfSession[] Review { get; set; }
    }

    public partial class ReviewOfSession
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zerotofiveRating")]
        public long ZerotofiveRating { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("requestContact")]
        public bool RequestContact { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class CreateSession
    {
        [JsonProperty("sessionNumber")]
        public string SessionNumber { get; set; }

        [JsonProperty("sessionName")]
        public string SessionName { get; set; }

        [JsonProperty("sessionDate")]
        public string SessionDate { get; set; }

        [JsonProperty("sessionCity")]
        public string SessionCity { get; set; }

        [JsonProperty("sessionRegionState")]
        public string SessionRegionState { get; set; }

        [JsonProperty("sessionCountry")]
        public string SessionCountry { get; set; }

        [JsonProperty("review")]
        public ReviewOfSession[] Review { get; set; }
    }

    public class IncomingText
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
    public class Selfie
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("photoBase64")]
        public string PhotoBase64 { get; set; }
    }
}
