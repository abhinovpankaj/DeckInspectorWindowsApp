﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Model
{
    public class VisualBuildingApartment
    {
        public Guid Id { get; set; }
        public string BuildingApartmentId { get; set; }
        public string Name { get; set; }
        public string ImageDescription { get; set; }
        public string AdditionalConsideration { get; set; }

        public string ExteriorElements { get; set; }


        public string WaterProofingElements { get; set; }
        public string ConditionAssessment { get; set; }
        public string VisualReview { get; set; }

        public string AnyVisualSign { get; set; }
        public string FurtherInasive { get; set; }
        public string LifeExpectancyEEE { get; set; }
        public string LifeExpectancyLBC { get; set; }
        public string LifeExpectancyAWE { get; set; }


        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public bool IsDelete { get; set; }
        public int SeqNo { get; set; }
        public string Username { get; set; }

        //New Fields

        public bool IsPostInvasiveRepairsRequired { get; 
            set; }
        public bool IsInvasiveRepairApproved { get; set; }
        public bool IsInvasiveRepairComplete { get; set; }
        
        public string ConclusiveComments { get; set; }
        public string ConclusiveLifeExpEEE { get; set; }
        public string ConclusiveLifeExpLBC { get; set; }
        public string ConclusiveLifeExpAWE { get; set; }
        public string ConclusiveAdditionalConcerns { get; set; }


        //Ends


    }
}
