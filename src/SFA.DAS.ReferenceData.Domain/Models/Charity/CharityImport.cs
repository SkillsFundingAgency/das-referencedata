using Newtonsoft.Json;
using System;

namespace SFA.DAS.ReferenceData.Domain.Models.Charity
{
    public class CharityImport
    {
        [JsonProperty("date_of_extract")]
        public DateTime? DateOfExtract { get; set; }
        
        [JsonProperty("organisation_number")]
        public int OrganisationNumber { get; set; }
        
        [JsonProperty("registered_charity_number")]
        public int RegisteredCharityNumber { get; set; }
        
        [JsonProperty("linked_charity_number")]
        public int LinkedCharityNumber { get; set; }
        
        [JsonProperty("charity_name")]
        public string CharityName { get; set; }
        
        [JsonProperty("charity_type")]
        public string CharityType { get; set; }
        
        [JsonProperty("charity_registration_status")]
        public string CharityRegistrationStatus { get; set; }
        
        [JsonProperty("date_of_registration")]
        public DateTime? DateOfRegistration { get; set; }
        
        [JsonProperty("date_of_removal")]
        public DateTime? DateOfRemoval { get; set; }
        
        [JsonProperty("charity_reporting_status")]
        public string CharityReportingStatus { get; set; }
        
        [JsonProperty("latest_acc_fin_period_start_date")]
        public DateTime? LatestAccFinPeriodStartDate { get; set; }
        
        [JsonProperty("latest_acc_fin_period_end_date")]
        public DateTime? LatestAccFinPeriodEndDate { get; set; }        
        [JsonProperty("latest_income")]
        public long? LatestIncome { get; set; }
        
        [JsonProperty("latest_expenditure")]
        public long? LatestExpenditure { get; set; }
        
        [JsonProperty("charity_contact_address1")]
        public string CharityContactAddress1 { get; set; }
        
        [JsonProperty("charity_contact_address2")]
        public string CharityContactAddress2 { get; set; }
        
        [JsonProperty("charity_contact_address3")]
        public string CharityContactAddress3 { get; set; }
        
        [JsonProperty("charity_contact_address4")]
        public string CharityContactAddress4 { get; set; }
        
        [JsonProperty("charity_contact_address5")]
        public string CharityContactAddress5 { get; set; }
        
        [JsonProperty("charity_contact_postcode")]
        public string CharityContactPostcode { get; set; }
        
        [JsonProperty("charity_contact_phone")]
        public string CharityContactPhone { get; set; }
        
        [JsonProperty("charity_contact_email")]
        public string CharityContactEmail { get; set; }
        
        [JsonProperty("charity_contact_web")]
        public string CharityContactWeb { get; set; }
        
        [JsonProperty("charity_company_registration_number")]
        public string CharityCompanyRegistrationNumber { get; set; }
        
        [JsonProperty("charity_insolvent")]
        public bool CharityInsolvent { get; set; }
        
        [JsonProperty("charity_in_administration")]
        public bool CharityInAdministration { get; set; }
        
        [JsonProperty("charity_previously_excepted")]        
        public bool? CharityPreviouslyExcepted { get; set; }
        
        [JsonProperty("charity_is_cdf_or_cif")]        
        public string CharityIsCdfOrCif { get; set; }
        
        [JsonProperty("charity_is_cio")]
        public bool? CharityIsCio { get; set; }
        
        [JsonProperty("cio_is_dissolved")]
        public bool? CioIsDissolved { get; set; }
        
        [JsonProperty("date_cio_dissolution_notice")]        
        public DateTime? DateCioDissolutionNotice { get; set; }
        
        [JsonProperty("charity_activities")]        
        public string CharityActivities { get; set; }
        
        [JsonProperty("charity_gift_aid")]        
        public bool? CharityGiftAid { get; set; } 
        
        [JsonProperty("charity_has_land")]
        public bool? CharityHasLand { get; set; }
    }
}
