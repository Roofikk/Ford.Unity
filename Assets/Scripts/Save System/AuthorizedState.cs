using Ford.SaveSystem.Ver2;
using Ford.WebApi;
using Ford.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace Ford.SaveSystem
{
    public class AuthorizedState : SaveSystemState
    {
        public FordApiClient ApiClient { get; set; }
        private readonly SecureString _accessToken;
        public StorageHistory History { get; private set; }

        public AuthorizedState()
        {
            ApiClient = new();
            _accessToken = new SecureString();

            string accessToken = new Storage().GetAccessToken();
            foreach (var c in accessToken)
            {
                _accessToken.AppendChar(c);
            }
        }

        public override async Task<ICollection<HorseBase>> GetHorses(StorageSystem storage, int below = 0, int amount = 20,
            string orderByDate = "desc", string orderByName = "false")
        {
            var result = await ApiClient.GetHorsesAsync(_accessToken.ToString(), below, amount, orderByDate, orderByName);
            var newResult = await RefreshTokenAndRetrieveResult(result, _accessToken.ToString(), ApiClient.GetHorsesAsync, below, amount, orderByDate, orderByName);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.GetHorses();
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<bool> Initiate(StorageSystem storage, SaveSystemState fromState)
        {
            if (fromState is UnauthorizedState unauthorizedState)
            {
                History = unauthorizedState.History;
            }

            return true;
        }

        public override async Task<HorseBase> CreateHorse(StorageSystem storage, CreationHorse horse)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.CreateHorseAsync(accessToken, horse);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.CreateHorseAsync, horse);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.CreateHorse(horse);
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<HorseBase> UpdateHorse(StorageSystem storage, UpdatingHorse horse)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.UpdateHorseAsync(accessToken, horse);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.UpdateHorseAsync, horse);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.CreateHorse(new CreationHorse()
                {
                    Name = horse.Name,
                    Description = horse.Description,
                    Sex = horse.Sex,
                    BirthDate = horse.BirthDate,
                    City = horse.City,
                    Region = horse.Region,
                    Country = horse.Country,
                    OwnerName = "���������",
                    OwnerPhoneNumber = "����������",
                });
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<bool> DeleteHorse(StorageSystem storage, long horseId)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.DeleteHorseAsync(accessToken, horseId);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.DeleteHorseAsync, horseId);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return false;
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }

            return true;
        }

        public override async Task<ICollection<SaveInfo>> GetSaves(StorageSystem storage, long horseId, int below = 0, int amount = 20)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.GetSavesAsync(accessToken, horseId, below, amount);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.GetSavesAsync, horseId, below, amount);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.GetSaves(horseId, below, amount);
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<FullSaveInfo> GetFullSave(StorageSystem storage, long horseId, long saveId)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.GetSaveAsync(accessToken, horseId, saveId);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.GetSaveAsync, horseId, saveId);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.GetSave(horseId, saveId);
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<SaveInfo> CreateSave(StorageSystem storage, FullSaveInfo save)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.CreateSaveAsync(accessToken, save);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.CreateSaveAsync, save);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.CreateSave(save);
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<SaveInfo> UpdateSave(StorageSystem storage, SaveInfo save)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.UpdateSaveInfoAsync(accessToken, save);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.UpdateSaveInfoAsync, save);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return await storage.CreateSave((FullSaveInfo)save);
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }

            return newResult.Content;
        }

        public override async Task<bool> DeleteSave(StorageSystem storage, long saveId)
        {
            FordApiClient client = new();
            var accessToken = GetAccessToken();

            var result = await client.DeleteSaveAsync(accessToken, saveId);
            var newResult = await RefreshTokenAndRetrieveResult(result, accessToken, client.DeleteSaveAsync, saveId);

            if (newResult.StatusCode == HttpStatusCode.Unauthorized || newResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                storage.ChangeState(SaveSystemStateEnum.Unauthorized);
                return false;
            }

            if (newResult.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }

            return true;
        }

        public override async Task<bool> TryChangeState(StorageSystem storage)
        {
            storage.ChangeState(SaveSystemStateEnum.Unauthorized);
            return await Task.FromResult(true);
        }

        private string GetAccessToken()
        {
            Storage store = new();
            return store.GetAccessToken();
        }

        private async Task<ResponseResult> RefreshTokenAndRetrieveResult(ResponseResult result,
            string accessToken, Func<string, Task<ResponseResult>> repeat)
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat);
        }

        private async Task<ResponseResult<T>> RefreshTokenAndRetrieveResult<T>(ResponseResult<T> result, 
            string accessToken, Func<string, Task<ResponseResult<T>>> repeat) where T : class
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat);
        }

        private async Task<ResponseResult> RefreshTokenAndRetrieveResult<TParam>(ResponseResult result,
            string accessToken, Func<string, TParam, Task<ResponseResult>> repeat, TParam param)
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat, param);
        }

        private async Task<ResponseResult<TResult>> RefreshTokenAndRetrieveResult<TParam, TResult>(ResponseResult<TResult> result,
            string accessToken, Func<string, TParam, Task<ResponseResult<TResult>>> repeat, TParam param)
            where TResult : class
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat, param);
        }

        private async Task<ResponseResult<TResult>> RefreshTokenAndRetrieveResult<TParam1, TParam2, TResult>(ResponseResult<TResult> result,
            string accessToken, Func<string, TParam1, TParam2, Task<ResponseResult<TResult>>> repeat, TParam1 param1, TParam2 param2)
            where TResult : class
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat, param1, param2);
        }

        private async Task<ResponseResult<TResult>> RefreshTokenAndRetrieveResult<TParam1, TParam2, TParam3, TResult>(ResponseResult<TResult> result,
            string accessToken, Func<string, TParam1, TParam2, TParam3, Task<ResponseResult<TResult>>> repeat, TParam1 param1, TParam2 param2, TParam3 param3)
            where TResult : class
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat, param1, param2, param3);
        }
        private async Task<ResponseResult<TResult>> RefreshTokenAndRetrieveResult<TParam1, TParam2, TParam3, TParam4, TResult>(
            ResponseResult<TResult> result, string accessToken, Func<string, TParam1, TParam2, TParam3, TParam4, Task<ResponseResult<TResult>>> repeat, 
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
            where TResult : class
        {
            if (result.StatusCode != HttpStatusCode.Unauthorized)
            {
                return await Task.FromResult(result);
            }

            FordApiClient client = new();
            return await client.RefreshTokenAndReply(accessToken, repeat, param1, param2, param3, param4);
        }
    }
}
