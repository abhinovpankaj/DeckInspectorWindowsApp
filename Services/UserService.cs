using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;

namespace UI.Code.Services
{
    public interface IUserService
    {
        Task<Response> AddItemAsync(User item);
        Task<Response> UpdateItemAsync(User item);
        Task<bool> DeleteItemAsync(string id);
        Task<User> GetItemAsync(string id);
        Task<IEnumerable<User>> GetItemsAsync(bool ? IsActive, string search, bool forceRefresh = false);
        //Task<IEnumerable<BuildingApartment>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class UserService : IUserService
    {
         List<User> items;

        public UserService()
        {
            items = new List<User>();
            //{
            //    new User { Id = Guid.NewGuid().ToString(),Mobile="9898989899" ,UserName="admin",FirstName="testing1",LastName="surname1" , Pwd="123",Role="Admin",IsActive=true},
            //     new User { Id = Guid.NewGuid().ToString(), Mobile="9898989891", UserName="admin2",FirstName="testing2",LastName="surname2" ,Pwd="xyz",Role="Admin",IsActive=true},
            //       new User { Id = Guid.NewGuid().ToString(), Mobile="9898982899" , UserName="user",FirstName="testing3",LastName="surname3" ,Pwd="xyz",Role="User",IsActive=true},
            //       new User { Id = Guid.NewGuid().ToString(),Mobile="9498989899" , UserName="user2",FirstName="testing4",LastName="surname4" ,Pwd="xyz4",Role="User",IsActive=false},
                   
            //};
            

        }
        public async Task<Response> AddItemAsync(User item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/User/InsertUser", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                    //if (response.IsSuccessStatusCode == false)
                    //{
                    //    throw new ApiException
                    //    {
                    //        StatusCode = (int)response.StatusCode,
                    //        Content = result.Message
                    //    };
                    //}
                    return await Task.FromResult(result);


                }
            }
        }
        public async Task<Response> Assign(AssignLocationAndBuilding item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/User/Assign", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                    //if (response.IsSuccessStatusCode == false)
                    //{
                    //    throw new ApiException
                    //    {
                    //        StatusCode = (int)response.StatusCode,
                    //        Content = result.Message
                    //    };
                    //}
                    return await Task.FromResult(result);


                }
            }
        }
        public async Task<Response> UpdateItemAsync(User item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/User/UpdateUser", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                    //if (response.IsSuccessStatusCode == false)
                    //{
                    //    throw new ApiException
                    //    {
                    //        StatusCode = (int)response.StatusCode,
                    //        Content = result.Message
                    //    };
                    //}
                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((User arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<User> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
        public async Task<Response> ValidateLogin(User user)
        {
           
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/User/ValidateLogin",user))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                    //if (response.IsSuccessStatusCode == false)
                    //{
                    //    throw new ApiException
                    //    {
                    //        StatusCode = (int)response.StatusCode,
                    //        Content = result.Message
                    //    };
                    //}
                    return await Task.FromResult(result);


                }
            }
        }
        public async Task<IEnumerable<User>> GetItemsAsync(bool ? IsActive,string search, bool forceRefresh = false)
        {

            string Query = string.Format("api/User/GetUserList?isActive={0}&searchText={1}", IsActive, search);
            //if(!string.IsNullOrEmpty(CreatedOn))
            //{
            //    Query += "?" + CreatedOn;
            //}
            //if (!string.IsNullOrEmpty(search))
            //{
            //    Query += "?" + search;
            //}

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(Query))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }

        }


        public async Task<IEnumerable<User>> GetUserListForAssign(string UserID, string ProjectID, string ProjectLocationID, string BuildingID, string Type,string searchText)
        {

            string Query = string.Format("api/User/GetUserListForAssign?UserID={0}&ProjectID={1}&ProjectLocationID={2}&BuildingID={3}&Type={4}&searchText={5}", UserID, ProjectID, ProjectLocationID, BuildingID, Type, searchText);
            //if(!string.IsNullOrEmpty(CreatedOn))
            //{
            //    Query += "?" + CreatedOn;
            //}
            //if (!string.IsNullOrEmpty(search))
            //{
            //    Query += "?" + search;
            //}

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(Query))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }

        }






    }
}
