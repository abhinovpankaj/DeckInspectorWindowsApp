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
    public interface IProjectBuilding
    {
        Task<Response> AddItemAsync(ProjectBuilding item);
        Task<Response> UpdateItemAsync(ProjectBuilding item);
        Task<Response> DeleteItemAsync(ProjectBuilding item);
        Task<ProjectBuilding> GetItemAsync(string id);
        Task<IEnumerable<ProjectBuilding>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectID(string projectId);

    }
    public class ProjectBuildingDataStore : IProjectBuilding
    {
        List<ProjectBuilding> items;

        public ProjectBuildingDataStore()
        {
            items = new List<ProjectBuilding>();
           

        }
        public async Task<Response> AddItemAsync(ProjectBuilding item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectBuilding/InsertProjectBuilding", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<Response> UpdateItemAsync(ProjectBuilding item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectBuilding/UpdateProjectBuilding", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<Response> DeleteItemAsync(ProjectBuilding item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectBuilding/DeleteProjectBuilding", item))
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

        public async Task<ProjectBuilding> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
      

        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectID(string projectId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectBuilding/GetBuildingByProjectID?ProjectID=" + projectId + "&UserID=" + App.LogUser.Id.ToString()))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    if (result.Status == ApiResult.Success)
                    {
                        items = JsonConvert.DeserializeObject<List<ProjectBuilding>>(result.Data.ToString());

                        response.EnsureSuccessStatusCode();
                    }
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectBuilding/Reorder", list))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }
        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectIDAndUser(string ProjectID, string UserId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectBuilding/GetAssignBuildingByProjectIDAndUserID?ProjectID=" + ProjectID + "&UserId=" + UserId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<ProjectBuilding>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }

    }
}