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
    public interface IProjectLocation
    {
        Task<Response> AddItemAsync(ProjectLocation item);
        Task<Response> UpdateItemAsync(ProjectLocation item);
        Task<Response> DeleteItemAsync(ProjectLocation item);

        Task<ProjectLocation> GetItemAsync(string id);

     //   Task<bool> AddProjectCommonLocationAsync(ProjectLocation item);
        Task<IEnumerable<ProjectLocation>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectID(string projectId);

    }
    public class ProjectLocationService : IProjectLocation
    {
        List<ProjectLocation> items;

        public ProjectLocationService()
        {
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<ProjectLocation>();
            //{
            //    new ProjectLocation
            //    {
            //        Id = "1",
            //        ProjectId="1",
            //        Name  = "Project Location 1 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",
            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectLocation
            //    {
            //        Id = "2",
            //        ProjectId="1",
            //        Name  = "Project Location 2 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",

            //    },
            //    new ProjectLocation
            //    {
            //        Id = "3",
            //        ProjectId="1",
            //        Name  = "Project Location 3 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectLocation
            //    {
            //        Id = "4",
            //        ProjectId="2",
            //        Name  = "Project Location 4 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectLocation
            //    {
            //        Id = "5",
            //        ProjectId="2",
            //        Name  = "Project Location 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new ProjectLocation
            //    {
            //        Id = "6",
            //        ProjectId="2",
            //        Name  = "Project Location 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //      new ProjectLocation
            //    {
            //        Id = "7",
            //        ProjectId="1",
            //        Name  = "Project Location 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new ProjectLocation
            //    {
            //        Id = "8",
            //        ProjectId="1",
            //        Name  = "Project Location 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //      new ProjectLocation
            //    {
            //        Id = "9",
            //        ProjectId="1",
            //        Name  = "Project Location 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new ProjectLocation
            //    {
            //        Id = "10",
            //        ProjectId="1",
            //        Name  = "Project Location 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //      new ProjectLocation
            //    {
            //        Id = "11",
            //        ProjectId="1",
            //        Name  = "Project Location 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new ProjectLocation
            //    {
            //        Id = "12",
            //        ProjectId="1",
            //        Name  = "Project Location 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //};

        }
        public async Task<Response> AddItemAsync(ProjectLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocation/InsertProjectLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }

        public async Task<Response> UpdateItemAsync(ProjectLocation item)
        {
            item.UserId = new Guid(App.LogUser.Id);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocation/UpdateProjectLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<Response> DeleteItemAsync(ProjectLocation item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocation/DeleteProjectLocation", item))
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

        public async Task<ProjectLocation> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ProjectLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectID(string projectId)
        {
            //item.UserId = new Guid();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectLocation/GetLocationByProjectID?ProjectID=" + projectId+"&UserID="+ App.LogUser.Id.ToString()))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<ProjectLocation>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }

        public async Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectIDAndUser(string ProjectID, string UserId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectLocation/GetAssignLocationByProjectIDAndUserID?ProjectID=" + ProjectID+ "&UserId="+UserId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<ProjectLocation>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
            //return await Task.FromResult(items.Where(c=>c.ProjectId== projectId));
        }

        public async Task<Response> Reorder( List<ReorderList> list)
        {
            

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AppUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocation/Reorder", list))
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