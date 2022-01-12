using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UI.Code.Model;

namespace UI.Code.Services
{
    

    public interface IVisualFormVisualProjectLocationDataStore
    {
        Task<Response> AddItemAsync(VisualProjectLocation item);
        Task<Response> UpdateItemAsync(VisualProjectLocation item);
        Task<Response> DeleteItemAsync(VisualProjectLocation item);

   
        Task<IEnumerable<VisualProjectLocation>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocation>> GetItemsAsyncByVisualProjectLocationId(string VisualProjectLocationId);

    }
    public class VisualProjectLocationService : IVisualFormVisualProjectLocationDataStore
    {
        List<VisualProjectLocation> items;

        public VisualProjectLocationService()
        {
            
            items = new List<VisualProjectLocation>();
           

        }
        public async Task<Response> AddItemAsync(VisualProjectLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);
            //item.AdditionalConsideration = item.AdditionalConsideration == null ? "" : item.AdditionalConsideration;
            item.ImageDescription = item.ImageDescription == null ? "" : item.ImageDescription;
            //item.Username = App.LogUser.UserName;
            //item.CreatedOn = DateTime.Today.ToString();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocation/InsertVisualProjectLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }

        public async Task<Response> UpdateItemAsync(VisualProjectLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocation/UpdateVisualProjectLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<Response> DeleteItemAsync(VisualProjectLocation item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocation/DeleteVisualProjectLocation", item))
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

        //public async Task<VisualProjectLocation> GetItemAsync(string id)
        //{
        //    return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        //}

        public async Task<IEnumerable<VisualProjectLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<VisualProjectLocation>> GetItemsAsyncByVisualProjectLocationId(string VisualProjectLocationId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualProjectLocation/GetVisualProjectLocationByProjectLocationId?ProjectLocationId=" + VisualProjectLocationId + "&isInvasive=" + App.IsInvasive))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<VisualProjectLocation>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }


        public async Task<Response> Reorder(List<ReorderList> list)
        {


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocation/Reorder", list))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }
      
    }
}