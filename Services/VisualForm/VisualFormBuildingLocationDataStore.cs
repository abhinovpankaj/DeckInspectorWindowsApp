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
    //public interface IVisualFormBuildingLocationDataStore
    //{
    //    Task<bool> AddItemAsync(BuildingLocation_Visual item);
    //    Task<bool> UpdateItemAsync(BuildingLocation_Visual item);
    //    Task<bool> DeleteItemAsync(string id);
    //    Task<BuildingLocation_Visual> GetItemAsync(string id);
    //    Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false);
    //    Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId);

    //}
    //public class VisualFormBuildingLocationDataStore : IVisualFormBuildingLocationDataStore
    //{
    //    readonly List<BuildingLocation_Visual> items;

    //    public VisualFormBuildingLocationDataStore()
    //    {
    //        items = new List<BuildingLocation_Visual>();

    //    }
    //    public async Task<bool> AddItemAsync(BuildingLocation_Visual item)
    //    {
    //        items.Add(item);

    //        return await Task.FromResult(true);
    //    }

    //    public async Task<bool> UpdateItemAsync(BuildingLocation_Visual item)
    //    {
    //        var oldItem = items.Where((BuildingLocation_Visual arg) => arg.Id == item.Id).FirstOrDefault();
    //        items.Remove(oldItem);
    //        items.Add(item);

    //        return await Task.FromResult(true);
    //    }

    //    public async Task<bool> DeleteItemAsync(string id)
    //    {
    //        var oldItem = items.Where((BuildingLocation_Visual arg) => arg.Id == id).FirstOrDefault();
    //        items.Remove(oldItem);

    //        return await Task.FromResult(true);
    //    }

    //    public async Task<BuildingLocation_Visual> GetItemAsync(string id)
    //    {
    //        return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
    //    }

    //    public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
    //    {
    //        return await Task.FromResult(items);
    //    }


    //    public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId)
    //    {
    //        return await Task.FromResult(items.Where(c=>c.BuildingLocationId == buildingLocationId));
    //    }




    //}

    public interface IVisualFormBuildingLocationDataStore
    {
        Task<Response> AddItemAsync(VisualBuildingLocation item);
        Task<Response> UpdateItemAsync(VisualBuildingLocation item);
        Task<Response> DeleteItemAsync(VisualBuildingLocation item);


        Task<IEnumerable<VisualBuildingLocation>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualBuildingLocation>> GetVisualBuildingLocationByBuildingLocationId(string BuildingLocationId);

    }
    public class VisualFormBuildingLocationDataStore : IVisualFormBuildingLocationDataStore
    {
        List<VisualBuildingLocation> items;

        public VisualFormBuildingLocationDataStore()
        {

            items = new List<VisualBuildingLocation>();


        }
        public async Task<Response> AddItemAsync(VisualBuildingLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocation/InsertVisualBuildingLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }

        public async Task<Response> UpdateItemAsync(VisualBuildingLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocation/UpdateVisualBuildingLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<Response> DeleteItemAsync(VisualBuildingLocation item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocation/DeleteVisualBuildingLocation", item))
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

        //public async Task<VisualBuildingLocation> GetItemAsync(string id)
        //{
        //    return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        //}

        public async Task<IEnumerable<VisualBuildingLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<VisualBuildingLocation>> GetVisualBuildingLocationByBuildingLocationId(string BuildingLocationId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingLocation/GetVisualBuildingLocationByBuildingLocationId?BuildingLocationId=" + BuildingLocationId + "&isInvasive=" + App.IsInvasive))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<VisualBuildingLocation>>(result.Data.ToString());

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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocation/Reorder", list))
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