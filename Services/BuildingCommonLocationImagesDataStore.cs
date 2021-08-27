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
    public interface IBuildingCommonLocationImages
    {
        Task<bool> AddItemAsync(BuildingCommonLocationImages item);
        Task<bool> UpdateItemAsync(BuildingCommonLocationImages item);
        Task<Response> DeleteItemAsync(BuildingCommonLocationImages item);
        Task<BuildingCommonLocationImages> GetItemAsync(string id);
        Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class BuildingCommonLocationImagesDataStore : IBuildingCommonLocationImages
    {
         List<BuildingCommonLocationImages> items;

        public BuildingCommonLocationImagesDataStore()
        {
            items = new List<BuildingCommonLocationImages>();
        }
        public async Task<bool> AddItemAsync(BuildingCommonLocationImages item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BuildingCommonLocationImages item)
        {
            var oldItem = items.Where((BuildingCommonLocationImages arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingCommonLocationImages item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingLocationImage/DeleteBuildingLocationImage", item))
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
        public async Task<BuildingCommonLocationImages> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        
        public async Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingLocationImage/GetBuildingLocationImageByBuildingLocationId?BuildingLocationId=" + BuildingId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingCommonLocationImages>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }

        public async Task<Response> Reorder(List<ReorderList> list)
        {


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingLocationImage/Reorder", list))
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