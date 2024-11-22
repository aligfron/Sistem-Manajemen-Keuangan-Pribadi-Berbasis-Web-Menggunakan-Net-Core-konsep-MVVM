using Newtonsoft.Json;
using PersonalFinance.ViewModel;
using System.Net;
using System.Text;

namespace PersonalFinance.Web.Models
{
    public class PemasukanModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiUrl;
        private VMResponse<List<VMMoneySource>>? apiResponse;

        HttpContent content;
        private string jsonData;

        public PemasukanModel(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }
        public async Task<VMResponse<VMMoneySource>?> CreateAsync(VMMoneySource data)
        {
            VMResponse<VMMoneySource>? apiResponse = new VMResponse<VMMoneySource>();
            try
            {
                // Serialize data ke JSON
                string jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Kirim request POST ke API
                var response = await httpClient.PostAsync($"{apiUrl}Pemasukan", content);

                // Deserialize respons dari API
                string responseBody = await response.Content.ReadAsStringAsync();
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMMoneySource>>(responseBody);

                // Tangani kode status
                if (response.IsSuccessStatusCode)
                {
                    if (apiResponse?.StatusCode == HttpStatusCode.Created)
                    {
                        return apiResponse;
                    }
                }
                else
                {
                    throw new Exception($"Error: {apiResponse?.Message ?? "Unknown error"}");
                }
            }
            catch (Exception ex)
            {
                // Log error di konsol
                Console.WriteLine($"DataModel.CreateAsync: {ex.Message}");
            }

            return apiResponse;
        }
        public async Task<List<VMMoneySource>>? getByFilter(string? filter)
        {
            List<VMMoneySource>? dataCoba = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMMoneySource>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiUrl + "Pemasukan")
                    : await httpClient.GetStringAsync(apiUrl + "Pemasukan/GetBy/" + filter));

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse.Data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMMoneySource>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMMoneySource>? apiResponse = new VMResponse<VMMoneySource>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMMoneySource>?>(
                    await httpClient.DeleteAsync($"{apiUrl}Pemasukan/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMMoneySource>?>(
                     await httpClient.DeleteAsync($"{apiurl}Category?id={id}&userId={userId}").Result.Content.ReadAsStringAsync()
                     );*/

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(apiResponse.Message);
                    }

                }
                else
                {
                    throw new Exception("Data api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMMoneySource?> getById(int id)
        {
            VMMoneySource? dataCoba = null;
            try
            {
                VMResponse<VMMoneySource>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMMoneySource>>
                    (await httpClient.GetStringAsync(apiUrl + "Pemasukan/" + id));

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse.Data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }

        public async Task<VMResponse<VMMoneySource>?> UpdateAsync(VMMoneySource data)
        {
            VMResponse<VMMoneySource>? apiResponse = new VMResponse<VMMoneySource>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMMoneySource>?>
                    (await httpClient.PutAsync($"{apiUrl}Pemasukan", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.Message);
                    }
                }
                else
                {
                    throw new Exception("Data api could not be reached");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"DataModel.GetbyId: {e.Message}");

            }
            return apiResponse;
        }
    }
}
