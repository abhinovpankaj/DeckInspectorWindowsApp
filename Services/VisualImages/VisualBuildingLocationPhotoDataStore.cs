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
   

    public interface IVisualBuildingLocationPhotoDataStore
    {
        Task<Response> AddItemAsync(VisualBuildingLocationPhoto item);
        Task<Response> UpdateItemAsync(VisualBuildingLocationPhoto item);
        Task<Response> DeleteItemAsync(VisualBuildingLocationPhoto item);


        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualBuildingLocationPhoto>> GetVisualBuildingLocationImageByVisualBuildingId(string VisualBuildingId);

    }

    public class VisualBuildingLocationPhotoDataStore : IVisualBuildingLocationPhotoDataStore
    {
        List<VisualBuildingLocationPhoto> items;

        public VisualBuildingLocationPhotoDataStore()
        {
            items = new List<VisualBuildingLocationPhoto>();

        }

        public Task<Response> AddItemAsync(VisualBuildingLocationPhoto item)
        {
            throw new NotImplementedException();
        }


        public async Task<Response> DeleteItemAsync(VisualBuildingLocationPhoto item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocationImage/DeleteVisualBuildingLocationImage", item))
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
        public Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VisualBuildingLocationPhoto>> GetVisualBuildingLocationImageByVisualBuildingId(string VisualBuildingId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingLocationImage/GetVisualBuildingLocationImageByVisualBuildingId?VisualBuildingId=" + VisualBuildingId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<VisualBuildingLocationPhoto>>(result.Data.ToString());
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

        public Task<Response> UpdateItemAsync(VisualBuildingLocationPhoto item)
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocationImage/Reorder", list))
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