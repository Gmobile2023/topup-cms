﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HLS.Topup.Common
{
    public interface IRedisCache
    {
        Task<string> GetCache(string key);
        Task<bool> SetCache(string key, string value, TimeSpan expire);
        Task<bool> ClearCache(string key);
        Task<bool> ClearAllCache(IEnumerable<string> keys);
        Task<bool> SetCacheObject(string key, object obj, TimeSpan expire);
        Task<T> GetCacheObject<T>(string key);
        Task<string> GetAndSetValue(string key, string value);
        Task<List<string>> GetKeys(string partem);
        Task SetFile(string token, byte[] content);

        Task<byte[]> GetFile(string token);
        Task<bool> AddEntity<T>(string key, T dto,TimeSpan? expire = null);
        Task<T> GetEntity<T>(string key);
        Task<bool> UpdateEntity<T>(string key, T dto);
        Task<bool> DeleteEntity(string key);
        Task RemoveByPatternAsync(string pattern);
    }
}