using Newtonsoft.Json;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;

namespace UI.Code.Services
{
    public static class DataUploadService
    {
        public static async Task<Response> UploadFile(string filePath, string parentName, string endpointUrl)
        {
            Response result = new Response();
            var parameters = new  Dictionary<string, string>();
            var extension = Path.GetExtension(filePath);
            parameters.Add("extension", extension);
            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(App.AppUrl);

                    using (var formData = new MultipartFormDataContent())
                    {
                        string ServerFileName = parentName + Guid.NewGuid().ToString();

                        formData.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "MainImage" + DateTime.Now.Ticks, ServerFileName);

                        formData.Add(DictionaryItems, "Model");

                        var response = client.PostAsync(endpointUrl, formData).Result;

                        var responseBody = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Response>(responseBody);
                        
                    }

                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ex.Message;
                result.Status = ApiResult.Fail;
            }
            return await Task.FromResult(result);
        }

        public static async Task<Response> UploadFinalReportTemplate(ProjectDocument document, string endpointUrl)
        {
            Response result = new Response();
            var parameters = new Dictionary<string, string>();
            var extension = Path.GetExtension(document.DocURL);
            parameters.Add("extension", extension);
            parameters.Add("DocumentName", document.DocumentName);
            parameters.Add("ProjectId", document.ProjectId);
            parameters.Add("DocUrl", document.DocURL);
            parameters.Add("UserName", document.UserName);
            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = new HttpClient())                {

                    client.BaseAddress = new Uri(App.AppUrl);

                    using (var formData = new MultipartFormDataContent())
                    {
                        string ServerFileName = Path.GetFileNameWithoutExtension(document.DocURL) + Guid.NewGuid().ToString();

                        formData.Add(new ByteArrayContent(File.ReadAllBytes(document.DocURL)), Path.GetFileNameWithoutExtension(document.DocURL) + DateTime.Now.Ticks, ServerFileName);

                        formData.Add(DictionaryItems, "Model");

                        var response = client.PostAsync(endpointUrl, formData).Result;

                        var responseBody = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Response>(responseBody);
                    }

                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ex.Message;
                result.Status = ApiResult.Fail;
            }
            return await Task.FromResult(result);
        }
        public static async Task<Response> UploadProjectDocument(ProjectDocument document, string endpointUrl)
        {
            Response result = new Response();

            var parameters = new Dictionary<string, string>();
            if (document.ProjectId == "")
            {
                parameters.Add("extension", ".html");
            }
            else
            {
                var extension = Path.GetExtension(document.DocURL);
                parameters.Add("extension", extension);
            }
            parameters.Add("DocumentName", document.DocumentName);
            parameters.Add("ProjectId", document.ProjectId);
            parameters.Add("DocUrl", document.DocURL);
            parameters.Add("UserName", document.UserName);
            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(App.AppUrl);

                    using (var formData = new MultipartFormDataContent())
                    {
                        string ServerFileName = Path.GetFileNameWithoutExtension(document.DocURL) + Guid.NewGuid().ToString();

                        if (document.ProjectId=="")    
                        {                            
                            WordDocument reportDoc = new WordDocument(document.DocURL, FormatType.Docx);
                            if (File.Exists("Finel.html"))
                            {
                                File.Delete("Finel.html");
                            }
                            reportDoc.Save("Finel.html", FormatType.Html);                            
                            reportDoc.Close();

                            formData.Add(new ByteArrayContent(File.ReadAllBytes("Finel.html")), "Finel"
                                + DateTime.Now.Ticks, ServerFileName);
                        }
                        else
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(document.DocURL)), Path.GetFileNameWithoutExtension(document.DocURL)
                                + DateTime.Now.Ticks, ServerFileName);

                        formData.Add(DictionaryItems, "Model");

                        var response =  client.PostAsync(endpointUrl, formData).Result;

                        var responseBody = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Response>(responseBody);
                    }

                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ex.Message;
                result.Status = ApiResult.Fail;
            }
            return await Task.FromResult(result);
        }

    }
}
