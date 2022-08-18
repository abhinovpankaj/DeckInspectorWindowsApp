using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;

namespace UI.Code.Services
{
    public interface IProjectService
    {
        Task<Response> AddItemAsync(Project item);
        Task<Response> UpdateItemAsync(Project item);
        Task<Response> DeleteItemAsync(Project item);
        Task<Response> GetItemAsync(string id);
        Task<Response> FinelReportUpdateItemAsync(Project item);
        Task<Response> CreateInvasiveReport(Project item);
        Task<Response> MoveItemAsync(OrganizationDetail item);
        Task<IEnumerable<Project>> GetItemsAsync(string search, string Type, string CreatedOn, bool forceRefresh = false,bool IsDeleted=false);
        Task<IEnumerable<Project>> TreeGetItemsAsync(string TreeID, string search, string Type, string CreatedOn, bool forceRefresh = false);
        //Task<IEnumerable<BuildingApartment>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class ProjectService : IProjectService
    {
        List<Project> items;
        public async Task<Response> CreateInvasiveReport(Project item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/CreateInvasiveReport", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }
        public ProjectService()
        {
            

        }
        public async Task<Response> AddItemAsync(Project item)
        {
            
            //item.ImageUrl = null;
            
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/InsertProject", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);
                }
            }

            // return await Task.FromResult(new Response());

        }

        public async Task<Response> UpdateItemAsync(Project item)
        {

            item.UserId = new Guid(App.LogUser.Id);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/UpdateProject", item))
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

        public async Task<Response> FinelReportUpdateItemAsync(Project item)
        {

            item.UserId = new Guid(App.LogUser.Id);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/FinelReportCompleteUpdateProject", item))
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

        public async Task<Response> DeleteItemAsync(Project item)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/DeleteProject", item))
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

        public async Task<Response> DeletePermanentlyItemAsync(string id)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.DeleteAsync($"api/Project/Delete?id=" + id))
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
        public async Task<Response> RestoreItemAsync(Project item)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/RestoreProject", item))
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
        public async Task<Response> GetItemAsync(string id)
        {
            // string Query = string.Format("api/Project/GetProjectByIDMobile?Id={0}&UserId={1}", id, App.LogUser.Id.ToString());

            string Query = string.Format("api/Project/GetProjectByID?Id={0}", id);


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


                    // Project project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }


        public async Task<Response> MoveItemAsync(OrganizationDetail item)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/MoveProject", item))
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

        public async Task<IEnumerable<Project>> GetItemsAsync(string search, string Type, string CreatedOn, bool isProjectDeleted= false,bool forceRefresh = false)
        {

            string Query = string.Empty;
            if (App.LogUser.RoleName == "Admin")
            {
                Query = string.Format("api/Project/GetProjectList?CreatedOn={0}&isActive={1}&searchText={2}&ProjectType={3}", CreatedOn, isProjectDeleted, search, Type);
            }
            else
            {
                Query = string.Format("api/Project/GetProjectForMobile?UserID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", App.LogUser.Id.ToString(), CreatedOn, isProjectDeleted, search, Type);
            }
            // string Query =string.Format("api/Project/GetProjectList?CreatedOn={0}&isActive={1}&searchText={2}&ProjectType={3}", CreatedOn,false,search,Type);
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


                    items = JsonConvert.DeserializeObject<List<Project>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }

        }

        public async Task<IEnumerable<ProjectDocument>> GetDocuments(string id)
        {

            string endPoint = string.Format("api/Project/GetDocuments?Id={0}", id);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(endPoint))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                    if (result.Status == ApiResult.Success)
                    {
                        var documents = JsonConvert.DeserializeObject<List<ProjectDocument>>(result.Data.ToString());

                        response.EnsureSuccessStatusCode();
                        return await Task.FromResult(documents);
                    }
                    else
                        return new List<ProjectDocument>();
                    
                }
            }
        }
        public async Task<Response> DeleteProjectDocument(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/DeleteDocument", id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                   
                    return await Task.FromResult(result);
                }
            }
        }

        public async Task<Response> AddProjectDocumentAsync(ProjectDocument document)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/AddDocuments", document))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);
                }
            }
        }
        public async Task<IEnumerable<Project>> TreeGetItemsAsync(string TreeID, string search, string Type, string CreatedOn, bool forceRefresh = false)
        {
            string Query = string.Empty;
            if (string.IsNullOrEmpty(TreeID))
            {
                return await Task.FromResult(items);
               
            }
            else
            {
                if (App.LogUser.RoleName == "Admin")
                {
                    Query = string.Format("api/Project/TreeGetProject?TreeID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", TreeID, CreatedOn, false, search, Type);
                }
                else
                {
                    Query = string.Format("api/Project/TreeGetProjectForMobile?TreeID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}&UserID={5}", TreeID, CreatedOn, false, search, Type, App.LogUser.Id.ToString());
                    //Query = string.Format("api/Project/TreeGetProjectForMobile?UserID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", App.LogUser.Id.ToString(), CreatedOn, false, search, Type);
                }
            }
           

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


                    items = JsonConvert.DeserializeObject<List<Project>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }

        }
    }
}
