namespace Yelp_GUI.YelpEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class BusinessInformationEngine
    {
        private Business currentBusiness = null;

        public void SetBusiness(Business business)
        {
            this.currentBusiness = business;
        }

        public Business GetBusiness()
        {
            return this.currentBusiness;
        }

        public void ClearBusiness()
        {
            this.currentBusiness = null;
        }

    }
}
