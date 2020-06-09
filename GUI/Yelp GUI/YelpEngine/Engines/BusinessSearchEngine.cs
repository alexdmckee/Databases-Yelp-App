namespace Yelp_GUI.YelpEngine
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Yelp_GUI.YelpEngine.Objects;

    public class BusinessSearchEngine
    {
        public BusinessSearch businessSearch = new BusinessSearch();

        public BusinessSearchEngine()
        {

        }

        public BusinessSearch GetBusinessSearch()
        {
            return this.businessSearch;
        }

        public void SetCurrentUser(User user)
        {
            this.businessSearch.CurrentUser = user;
        }

        public void AddSelectedBusinessCategory(string category)
        {
            if (!this.businessSearch.BusinessCategories.Contains(category))
            {
                this.businessSearch.BusinessCategories.Add(category);
            }
        }

        public void RemoveSelectedBusinessCategory(string category)
        {
            this.businessSearch.BusinessCategories.Remove(category);
        }

        public bool ContainsBusinessCategory(string category)
        {
            return this.businessSearch.BusinessCategories.Contains(category);
        }

        public List<string> GetSelectedBusinessCategories()
        {
            return this.businessSearch.BusinessCategories;
        }

        public void SetBusinessLocation(string state, string city, string zipcode)
        {
            this.businessSearch.State = state;
            this.businessSearch.City = city;
            this.businessSearch.Zipcode = zipcode;
        }

        public void ClearBusinessLocation()
        {
            this.businessSearch.State = string.Empty;
            this.businessSearch.City = string.Empty;
            this.businessSearch.Zipcode = string.Empty;
        }

        public void AddSelectedBusinss(Business business)
        {
            this.businessSearch.SelectedBusiness = business;
        }

        public Business GetSelectedBusiness()
        {
            return this.businessSearch.SelectedBusiness;
        }

        public void SetSortingMethod(string sortingMethod)
        {
            this.businessSearch.SortingMethod = sortingMethod;
        }

        public string GetSortingMethod()
        {
            return this.businessSearch.SortingMethod;
        }

        public void AddAttributeToSortBy(string attributeName, string attributeValue)
        {
            this.businessSearch.BusinessAttributes.Add(attributeName, attributeValue);
        }

        public void RemoveAttributeToSortBy(string attributeName)
        {
            this.businessSearch.BusinessAttributes.Remove(attributeName);
        }

        public void AddSearchResult(Business business)
        {
            this.businessSearch.SearchResults.Add(business);
        }

        public void ClearSearchResults()
        {
            this.businessSearch.SearchResults.Clear();
        }

        public List<Business> GetSearchResults()
        {
            return this.businessSearch.SearchResults;
        }

        public User GetCurrentUser()
        {
            return this.businessSearch.CurrentUser;
        }
    }
}
