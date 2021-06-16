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
    public interface IVisualProjectLocationPhotoDataStore
    {
        Task<Response> AddItemAsync(VisualProjectLocationPhoto item);
        Task<Response> UpdateItemAsync(VisualProjectLocationPhoto item);
        Task<Response> DeleteItemAsync(VisualProjectLocationPhoto item);


        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByVisualProjectLocationId(string VisualProjectLocationId);

    }
    
    public class VisualProjectLocationPhotoDataStore : IVisualProjectLocationPhotoDataStore
    {
         List<VisualProjectLocationPhoto> items;

        public VisualProjectLocationPhotoDataStore()
        {
            items = new List<VisualProjectLocationPhoto>();

        }

        public Task<Response> AddItemAsync(VisualProjectLocationPhoto item)
        {
            throw new NotImplementedException();
        }

       
        public async Task<Response> DeleteItemAsync(VisualProjectLocationPhoto item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocationImage/DeleteVisualProjectLocationImage", item))
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
        public Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByVisualProjectLocationId(string VisualProjectLocationId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualProjectLocationImage/GetVisualProjectLocationImageByVisualLocationId?VisualLocationId=" + VisualProjectLocationId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                  
                  //  items = items.Where(c => c.ImageDescription == "TRUE").ToList();
                    items = JsonConvert.DeserializeObject<List<VisualProjectLocationPhoto>>(result.Data.ToString());
                    //if (App.IsInvasive == true)
                    //{
                    //    items = items.Where(c => c.ImageDescription == "TRUE").ToList();
                    //}
                    //else
                    //{
                    //    items = items.Where(c => c.ImageDescription != "TRUE").ToList();
                    //}
                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }

        public Task<Response> UpdateItemAsync(VisualProjectLocationPhoto item)
        {
            throw new NotImplementedException();
        }
        public async Task<Response> Reorder(List<ReorderList> list)
        {


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualProjectLocationImage/Reorder", list))
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