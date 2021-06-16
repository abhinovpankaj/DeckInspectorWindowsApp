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
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<BuildingCommonLocationImages>();
            //{
            //    new BuildingCommonLocationImages
            //    {
            //        Id = "1",
            //        BuildingId="1",
            //        Name  = "Building Location Image 1 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",
            //        CreatedOn=" May 3 ,2020",
                  

            //    },
            //    new BuildingCommonLocationImages
            //    {
            //        Id = "2",
            //        BuildingId="1",
            //        Name  = "Building Location Image 2 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",

            //    },
            //    new BuildingCommonLocationImages
            //    {
            //        Id = "3",
            //        BuildingId="1",
            //        Name  = "Building Location Image 3 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingCommonLocationImages
            //    {
            //        Id = "4",
            //        BuildingId="2",
            //        Name  = "Building Location Image 4 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingCommonLocationImages
            //    {
            //        Id = "5",
            //        BuildingId="2",
            //        Name  = "Building Location Image 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new BuildingCommonLocationImages
            //    {
            //        Id = "6",
            //        BuildingId="2",
            //        Name  = "Building Location Image 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },

            //};

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