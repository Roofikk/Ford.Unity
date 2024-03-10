using Ford.SaveSystem;
using Ford.WebApi.Data;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ford.WebApi
{
    public class FordApiClient
    {
        private static Uri _hostUri;

        private readonly string _usersUri = "api/users";

        private readonly string _signUpUri = "api/identity/sign-up";
        private readonly string _signInUri = "api/identity/login";
        private readonly string _refreshTokenUri = "api/identity/refresh-token";
        private readonly string _accountUri = "api/identity/account";
        private readonly string _passwordChangeUri = "api/identity/account/password";

        private readonly string _horsesUri = "api/horses";
        private readonly string _updateHorseOwnersUri = "api/horseOwners";
        private readonly string _addHorseOwnersUri = "api/horseOwners/add-owner";
        private readonly string _changeOwnerRoleUri = "api/horseOwners/change-owner-role";

        private readonly string _savesUri = "api/saves";

        public static void SetHost(string host)
        {
            _hostUri = new Uri(host);
        }

        #region User
        public async Task<ResponseResult<User>> FindUser(string login)
        {
            Uri uri = new(_hostUri, $"{_usersUri}/search?userName={login}");
            var result = await GetRequest<User>(uri);
            return result;
        }
        #endregion

        #region Account
        /// <summary>
        /// Sign up user<br/>
        /// POST request<br/>
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDto>> SignUpAsync(RegisterUserDto registerUser)
        {
            Uri uri = new Uri(_hostUri, _signUpUri);
            var result = await PostRequest<AccountDto>(uri, registerUser);
            return result;
        }

        /// <summary>
        /// Get access token<br/>
        /// POST request<br/>
        /// </summary>
        /// <param name="loginRequestData"></param>
        /// <returns></returns>
        public async Task<ResponseResult<TokenDto>> SignInAsync(LoginRequestDto loginRequestData)
        {
            Uri uri = new Uri(_hostUri, _signInUri);
            var result = await PostRequest<TokenDto>(uri, loginRequestData);
            return result;
        }

        public async Task<ResponseResult<TokenDto>> RefreshTokenAsync(TokenDto requestToken)
        {
            Uri uri = new(_hostUri, _refreshTokenUri);
            var result = await PostRequest<TokenDto>(uri, requestToken);
            return result;
        }

        /// <summary>
        /// Get logining user info<br/>
        /// GET request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDto>> GetUserInfoAsync(string accessToken)
        {
            Uri uri = new(_hostUri, _accountUri);
            var account = await GetRequest<AccountDto>(uri, accessToken);
            return account;
        }

        /// <summary>
        /// Update user info<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="requestAccount"></param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountDto>> UpdateUserInfoAsync(string accessToken, UpdatingAccountDto requestAccount)
        {
            Uri uri = new(_hostUri, _accountUri);
            var result = await PostRequest<AccountDto>(uri, requestAccount, accessToken);
            return result;
        }

        /// <summary>
        /// Change password<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="passwordRequest"></param>
        /// <returns></returns>
        public async Task<ResponseResult<TokenDto>> ChangePasswordAsync(string accessToken, UpdatingPasswordDto passwordRequest)
        {
            Uri uri = new(_hostUri, _passwordChangeUri);
            var result = await PostRequest<TokenDto>(uri, passwordRequest, accessToken);
            return result;
        }
        #endregion

        #region Horse
        /// <summary>
        /// Get horse<br/>
        /// GET request
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="horseId">Horse id</param>
        /// <returns>Horse object</returns>
        public async Task<ResponseResult<HorseBase>> GetHorseAsync(string accessToken, long horseId)
        {
            Uri uri = new(_hostUri, $"{_horsesUri}?horseId={horseId}");
            var horse = await GetRequest<HorseBase>(uri, accessToken);
            return horse;
        }

        /// <summary>
        /// Get horses<br/>
        /// GET request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ICollection<HorseBase>>> GetHorsesAsync(string accessToken, int below = 0, int above = 20)
        {
            Uri uri = new(_hostUri, $"{_horsesUri}?below={below}&above={above}");
            var result = await GetRequest<RetrieveArray<HorseBase>>(uri, accessToken);
            return new ResponseResult<ICollection<HorseBase>>(result.Content.Items, result.StatusCode, result.Errors);
        }

        /// <summary>
        /// Create horse<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="horse">Horse object for creation</param>
        /// <returns></returns>
        public async Task<ResponseResult<HorseBase>> CreateHorseAsync(string accessToken, CreationHorse horse)
        {
            Uri uri = new(_hostUri, _horsesUri);
            var result = await PostRequest<HorseBase>(uri, horse, accessToken);
            return result;
        }

        /// <summary>
        /// Update horse object data<br/>
        /// PUT request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horse"></param>
        /// <returns></returns>
        public async Task<ResponseResult<HorseBase>> UpdateHorseAsync(string accessToken, UpdatingHorse horse)
        {
            Uri uri = new(_hostUri, _horsesUri);
            var result = await PutRequest<HorseBase>(uri, horse, accessToken);
            return result;
        }
        
        // �� ��������
        /// <summary>
        /// Delete horse<br/>
        /// DELETE request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horseId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteHorseAsync(string accessToken, long horseId)
        {
            Uri uri = new(_hostUri, $"{_horsesUri}?horseId={horseId}");
            var result = await DeleteRequest(uri, accessToken);
            return result;
        }
        #endregion

        #region HorseOwner
        /// <summary>
        /// Update horse owners<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horseOwners"></param>
        /// <returns></returns>
        public async Task<ResponseResult<HorseBase>> UpdateHorseOwnersAsync(string accessToken, UpdatingHorseOwnersDto horseOwners)
        {
            Uri uri = new(_hostUri, _updateHorseOwnersUri);
            var horse = await PostRequest<HorseBase>(uri, horseOwners, accessToken);
            return horse;
        }

        /// <summary>
        /// Delete horse owner<br/>
        /// DELETE request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> DeleteHorseOwnerAsync(string accessToken, long horseId, long userId)
        {
            Uri uri = new(_hostUri, $"{_horsesUri}?userId={userId}&horseId={horseId}");
            var result = await DeleteRequest(uri, accessToken);
            return result;
        }

        /// <summary>
        /// Add owner to horse<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horseOwner"></param>
        /// <returns></returns>
        public async Task<ResponseResult<HorseUserDto>> AddOwnerAsync(string accessToken, AddingHorseOwner horseOwner)
        {
            Uri uri = new(_hostUri, _updateHorseOwnersUri);
            var ownerDto = new AddingHorseOwnerDto(horseOwner);
            var result = await PostRequest<HorseUserDto>(uri, ownerDto, accessToken);
            return result;
        }

        /// <summary>
        /// Change owner role of horse<br/>
        /// POST request
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="horseOwner"></param>
        /// <returns></returns>
        public async Task<ResponseResult<HorseUserDto>> ChangeOwnerRoleAsync(string accessToken, AddingHorseOwner horseOwner)
        {
            Uri uri = new(_hostUri, _changeOwnerRoleUri);
            var ownerDto = new AddingHorseOwnerDto(horseOwner);
            var result = await PostRequest<HorseUserDto>(uri, ownerDto, accessToken);
            return result;
        }
        #endregion

        #region Saves
        public async Task<ResponseResult<IEnumerable<SaveInfo>>> GetSavesAsync(string accessToken, long horseId)
        {
            Uri uri = new Uri(_hostUri, $"{_savesUri}?horseId={horseId}");
            var result = await GetRequest<IEnumerable<SaveInfo>>(uri, accessToken);
            return result;
        }

        public async Task<ResponseResult<FullSaveInfo>> GetSaveAsync(string accessToken, long horseId, long saveId)
        {
            Uri uri = new Uri(_hostUri, $"{_savesUri}?horseId={horseId}&saveId={saveId}");
            var result = await GetRequest<FullSaveInfo>(uri, accessToken);
            return result;
        }

        public async Task<ResponseResult<SaveInfo>> CreateSaveAsync(string accessToken, long horseId, FullSaveInfo fullSave)
        {
            Uri uri = new Uri(_hostUri, $"{_savesUri}&horseId={horseId}");
            var result = await PostRequest<SaveInfo>(uri, fullSave, accessToken);
            return result;
        }

        public async Task<ResponseResult<SaveInfo>> UpdateSaveInfoAsync(string accessToken, SaveInfo save)
        {
            Uri uri = new Uri(_hostUri, _savesUri);
            var result = await PutRequest<SaveInfo>(uri, save, accessToken);
            return result;
        }

        public async Task<ResponseResult> DeleteSaveAsync(string accessToken, long saveId)
        {
            Uri uri = new Uri(_hostUri, $"{_savesUri}&saveId={saveId}");
            var result = await DeleteRequest(uri, accessToken);
            return result;
        }
        #endregion

        private async Task<ResponseResult<T>> GetRequest<T>(Uri uri, string accessToken = "") where T : class
        {
            HttpClient client = new();
            HttpRequestMessage request = new(HttpMethod.Get, uri);

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var response = await client.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<T>(responseText);
                    return new ResponseResult<T>(result);
                }

                return ReturnBadResponse<T>(responseText, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return new ResponseResult<T>(null, HttpStatusCode.InternalServerError, new List<ResponseError>()
                {
                    new()
                    {
                        Title = "ConnectionFailure",
                        Message = "Check your connection or repeat the request later"
                    }
                });
            }
        }

        private async Task<ResponseResult<T>> PostRequest<T>(Uri uri, object transferObject, string accessToken = "") where T : class
        {
            string json = JsonConvert.SerializeObject(transferObject);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpClient client = new();

            HttpRequestMessage request = new(HttpMethod.Post, uri)
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var response = await client.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var retrieveHorse = JsonConvert.DeserializeObject<T>(responseText);
                    return new ResponseResult<T>(retrieveHorse);
                }

                return ReturnBadResponse<T>(responseText, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return new ResponseResult<T>(null, HttpStatusCode.InternalServerError, new List<ResponseError>()
                {
                    new()
                    {
                        Title = "ConnectionFailure",
                        Message = "Check your connection or repeat the request later"
                    }
                });
            }
        }

        private async Task<ResponseResult<T>> PutRequest<T>(Uri uri, object transferObject, string accessToken = "") where T : class
        {
            string json = JsonConvert.SerializeObject(transferObject);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpClient client = new();

            HttpRequestMessage request = new(HttpMethod.Put, uri)
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var response = await client.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var retrieveHorse = JsonConvert.DeserializeObject<T>(responseText);
                    return new ResponseResult<T>(retrieveHorse);
                }

                return ReturnBadResponse<T>(responseText, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return new ResponseResult<T>(null, HttpStatusCode.InternalServerError, new List<ResponseError>()
                {
                    new()
                    {
                        Title = "ConnectionFailure",
                        Message = "Check your connection or repeat the request later"
                    }
                });
            }
        }

        private async Task<ResponseResult> DeleteRequest(Uri uri, string accessToken = "")
        {
            HttpClient client = new();
            HttpRequestMessage request = new(HttpMethod.Delete, uri);

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ResponseResult(response.StatusCode);
                }

                string responseText = await response.Content.ReadAsStringAsync();
                return ReturnBadResponse(responseText, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, new List<ResponseError>()
                {
                    new()
                    {
                        Title = "ConnectionFailure",
                        Message = "Check your connection or repeat the request later"
                    }
                });
            }
        }

        private ResponseResult<T> ReturnBadResponse<T>(string responseText, HttpStatusCode code) where T : class
        {
            try
            {
                BadResponseDto badResponse = JsonConvert.DeserializeObject<BadResponseDto>(responseText);
                return new ResponseResult<T>(null, code, badResponse?.Errors);
            }
            catch (JsonSerializationException ex)
            {
                return new ResponseResult<T>(null, code, new List<ResponseError>()
                {
                    new()
                    {
                        Title = "JsonSerializer",
                        Message = $"Text could not be serialized\nText: {responseText}"
                    }
                });
            }
        }

        private ResponseResult ReturnBadResponse(string responseText, HttpStatusCode code)
        {
            var badResponse = JsonConvert.DeserializeObject<BadResponseDto>(responseText);
            if (badResponse != null)
            {
                return new ResponseResult(code, badResponse?.Errors);
            }

            return new ResponseResult(code, new List<ResponseError>()
            {
                new()
                {
                    Title = "Unknown error",
                    Message = "Unknown error"
                }
            });
        }
    }
}