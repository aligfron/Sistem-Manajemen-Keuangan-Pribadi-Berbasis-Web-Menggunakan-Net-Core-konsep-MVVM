using Newtonsoft.Json;
using PersonalFinance.ViewModel;
using System.Net;
using System.Text;

namespace PersonalFinance.Web.Models
{
    public class UsersModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiUrl;

        HttpContent content;
        private string jsonData;

        public UsersModel(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }
        public async Task<VMUser> GetByEmail(string email)
        {
            VMUser? data = null;
            try
            {

                HttpResponseMessage apiResponseMsg =
                    await httpClient.GetAsync($"{apiUrl}Users/GetByEmail/{email}");
                if (apiResponseMsg != null)
                {
                    if (apiResponseMsg.StatusCode == HttpStatusCode.OK)
                    {
                        VMResponse<VMUser>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMUser>>
                             (apiResponseMsg.Content.ReadAsStringAsync().Result);
                        data = apiResponse!.Data;
                    }
                    else
                    {
                        throw new Exception($"{apiResponseMsg.StatusCode} - {apiResponseMsg.Content.ReadAsStringAsync().Result}");
                    }
                }
                else
                {
                    throw new Exception($"{apiResponseMsg.StatusCode} - {apiResponseMsg.RequestMessage}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<VMUser> Login(string email, string password)
        {
            VMUser? data = null;
            try
            {

                HttpResponseMessage apiResponseMsg =
                    await httpClient.GetAsync($"{apiUrl}Users/Login/{email}/{password}");
                if (apiResponseMsg != null)
                {
                    if (apiResponseMsg.StatusCode == HttpStatusCode.OK)
                    {
                        VMResponse<VMUser>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMUser>>
                             (apiResponseMsg.Content.ReadAsStringAsync().Result);
                        data = apiResponse!.Data;
                    }
                    else
                    {
                        throw new Exception($"{apiResponseMsg.StatusCode} - {apiResponseMsg.Content.ReadAsStringAsync().Result}");
                    }
                }
                else
                {
                    throw new Exception($"{apiResponseMsg.StatusCode} - {apiResponseMsg.RequestMessage}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<VMResponse<VMUser>?> CreateAsync(VMUser data)
        {
            VMResponse<VMUser>? apiResponse = new VMResponse<VMUser>();
            try
            {
                // Serialize data ke JSON
                string jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Kirim request POST ke API
                var response = await httpClient.PostAsync($"{apiUrl}Users", content);

                // Deserialize respons dari API
                string responseBody = await response.Content.ReadAsStringAsync();
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMUser>>(responseBody);

                // Tangani kode status
                if (response.IsSuccessStatusCode)
                {
                    if (apiResponse?.StatusCode == HttpStatusCode.Created)
                    {
                        return apiResponse;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Conflict) // Tangani 409 Conflict
                {
                    throw new Exception(apiResponse?.Message ?? "Email already exists.");
                }
                else
                {
                    throw new Exception($"Error: {apiResponse?.Message ?? "Unknown error"}");
                }
            }
            catch (Exception ex)
            {
                // Log error di konsol
                Console.WriteLine($"UserModel.CreateAsync: {ex.Message}");
            }

            return apiResponse;
        }

    }
}
