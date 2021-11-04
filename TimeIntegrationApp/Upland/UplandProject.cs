using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;

namespace TimeIntegrationApp.Upland
{
    class UplandProject
    {
        public string ProjectCode { get; set; }
        public int UniqueID { get; set; }
        public string State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AccessType { get; set; }
        public int IsPlaceholder { get; set; }
        public int Suspend { get; set; }
        public int DefaultPhaseId { get; set; }
        public int OverrideFunded { get; set; }
        public int OverrideRandD { get; set; }
        public int OverrideCapitalized { get; set; }
        public int OverrideCosted { get; set; }
        public int OverrideBillable { get; set; }
        public int IsFunded { get; set; }
        public int IsRandD { get; set; }
        public int IsCapitalized { get; set; }
        public int IsPayable { get; set; }
        public int IsBillable { get; set; }
        public string Priority { get; set; }
        public string HierarchyCode { get; set; }
        public int CompanyId { get; set; }
        public int OverridePlan { get; set; }
        public int AllowUserToEditETC { get; set; }
        public int WIPRule { get; set; }
        public int UnearnedRevenueAccount { get; set; }
        public int ProjectWorkflowMapId { get; set; }
        public int WIPAccount { get; set; }
        public int RevenueAccount { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int ParentId { get; set; }
        public int AlternateManagerId { get; set; }
        public int ActualManagerId { get; set; }
        public int PortfolioId { get; set; }
        public int CanBeInvoiced { get; set; }
        public int ManagerAutoApproved { get; set; }
        public string Phealth { get; set; }
        public string TrackingNo { get; set; }
        public string TimeEntryNotesOption { get; set; }
        public int RevenueRecognitionAccountId { get; set; }
        public int ContactId { get; set; }

    }
}
