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
        Task<IEnumerable<Project>> GetItemsAsync(string search, string Type, string CreatedOn, bool forceRefresh = false);
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
            //items = new List<Project>()
            //{
            //    new Project
            //    {
            //        Id = "1",
            //        Name  = "Sample Project 1 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",


            //        CreatedOn=" May 3 ,2020",
            //        ProjectType="Visual Report",


            //    },
            //    new Project
            //    {
            //        //Id = Guid.NewGuid().ToString(),
            //        Id="2",
            //        Name  = "Sample Project 2",
            //        Description="This is sample project description. a little big description for test",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Pankaj",

            //        CreatedOn=" April 3 ,2019",
            //        ProjectType="Invasive Report",


            //    },
            //    new Project
            //    {
            //        Id = "3",
            //        Name  = "Sample Project 3 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",


            //        CreatedOn=" May 3 ,2020",
            //        ProjectType="Visual Report",


            //    },
            //    new Project
            //    {
            //        Id = "4",
            //        Name  = "Sample Project 4 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",


            //        CreatedOn=" May 3 ,2020",
            //        ProjectType="Final Report",


            //    }

            //};


        }
        public async Task<Response> AddItemAsync(Project item)
        {
            //item.UserId =new Guid( App.LogUser.Id);
            //using (HttpClient client = new HttpClient())
            //{
            //    Dictionary<string, string> parameters = new Dictionary<string, string>();

            //    parameters.Add("Name", item.Name);
            //    parameters.Add("Address", item.Address);
            //    parameters.Add("Description", item.Description);
            //    parameters.Add("ProjectType", item.ProjectType);
            //    parameters.Add("UserID", App.LogUser.Id.ToString());

            //    MultipartFormDataContent form = new MultipartFormDataContent();
            //    HttpContent content = new StringContent("fileToUpload");
            //    HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            //    form.Add(content, "fileToUpload");
            //    form.Add(DictionaryItems, "Model");
            //    /// System.IO.File.OpenRead(filePath)
            //    var stream = System.IO.File.OpenRead(item.ImageUrl);
            //    content = new StreamContent(stream);
            //    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            //    {
            //        Name = "fileToUpload",
            //        //  FileName = (item.Name + .Replace(" ","_") + ".png"
            //        FileName = item.Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".png"
            //    };

            //    form.Add(content);

            //    HttpResponseMessage response = null;

            //    try
            //    {
            //        response = (client.PostAsync(App.AppUrl + "api/Project/NewProject", form)).Result;
            //        var responseBody = await response.Content.ReadAsStringAsync();
            //        Response result = JsonConvert.DeserializeObject<Response>(responseBody);
            //        item.Id = result.ID;
            //        return await Task.FromResult(result);

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    if (response.IsSuccessStatusCode == true)
            //    {

            //    }
            //}
            // item.ImageUrl = @"E:\UmeshWork\icon.png";
            item.ImageUrl = null;
            //item.ImageName = "tstBase2";
            //using (var stream = System.IO.File.OpenRead(item.ImageUrl))
            //{
            //    byte[] filebytearray = new byte[stream.Length];
            //    stream.Read(filebytearray, 0, (int)stream.Length);
            //    item.ImageUrl = Convert.ToBase64String(filebytearray);
            //}
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

        public async Task<IEnumerable<Project>> GetItemsAsync(string search, string Type, string CreatedOn, bool forceRefresh = false)
        {

            string Query = string.Empty;
            if (App.LogUser.RoleName == "Admin")
            {
                Query = string.Format("api/Project/GetProjectList?CreatedOn={0}&isActive={1}&searchText={2}&ProjectType={3}", CreatedOn, false, search, Type);
            }
            else
            {
                Query = string.Format("api/Project/GetProjectForMobile?UserID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", App.LogUser.Id.ToString(), CreatedOn, false, search, Type);
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

        public async Task<IEnumerable<Project>> TreeGetItemsAsync(string TreeID, string search, string Type, string CreatedOn, bool forceRefresh = false)
        {
            string Query = string.Empty;
            if (string.IsNullOrEmpty(TreeID))
            {
                return await Task.FromResult(items);
                //if (App.LogUser.RoleName == "Admin")
                //{
                //    Query = string.Format("api/Project/GetProjectList?CreatedOn={0}&isActive={1}&searchText={2}&ProjectType={3}", CreatedOn, false, search, Type);
                //}

                //else
                //{
                //    Query = string.Format("api/Project/GetProjectForMobile?UserID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", App.LogUser.Id.ToString(), CreatedOn, false, search, Type);
                //}
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
    }
}
