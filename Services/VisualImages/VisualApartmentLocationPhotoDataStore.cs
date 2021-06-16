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
    

    public interface IVisualApartmentLocationPhotoDataStore
    {
        Task<Response> AddItemAsync(VisualApartmentLocationPhoto item);
        Task<Response> UpdateItemAsync(VisualApartmentLocationPhoto item);
        Task<Response> DeleteItemAsync(VisualApartmentLocationPhoto item);


        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetVisualBuildingApartmentImageByVisualApartmentId(string VisualApartmentId);

    }

    public class VisualApartmentLocationPhotoDataStore : IVisualApartmentLocationPhotoDataStore
    {
        List<VisualApartmentLocationPhoto> items;

        public VisualApartmentLocationPhotoDataStore()
        {
            items = new List<VisualApartmentLocationPhoto>();

        }

        public Task<Response> AddItemAsync(VisualApartmentLocationPhoto item)
        {
            throw new NotImplementedException();
        }


        public async Task<Response> DeleteItemAsync(VisualApartmentLocationPhoto item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingApartmentImage/DeleteVisualBuildingApartmentImage", item))
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
        public Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VisualApartmentLocationPhoto>> GetVisualBuildingApartmentImageByVisualApartmentId(string VisualApartmentId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingApartmentImage/GetVisualBuildingApartmentImageByVisualApartmentId?VisualApartmentId=" + VisualApartmentId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<VisualApartmentLocationPhoto>>(result.Data.ToString());
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

        public Task<Response> UpdateItemAsync(VisualApartmentLocationPhoto item)
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingApartmentImage/Reorder", list))
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