using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace INPI.PM.Infrastructure.DataAccess.RedisAccess
{
    public class RedisHelper
    {
        private static string[] _hosts = new[] { "127.0.0.1:6379" };
        private static IRedisClientsManager _clientManager = new PooledRedisClientManager(_hosts);

        public bool Set<T>(string key, T t)
        {
            using (IRedisClient client = _clientManager.GetClient())
            {
                try
                {
                    client.Set<T>(key, t);
                    return true;
                }
                catch { return false; }
            }
        }

        public T Get<T>(string key)
        {
            using (IRedisClient client = _clientManager.GetClient())
            {
                try
                {
                    return client.Get<T>(key);
                }
                catch { return default(T); }
            }
        }

        public bool AddModelToSet<T>(string key, T model)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    setClient.AddItemToSet(set, model);
                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }
        public bool AddListToSet<T>(string key, List<T> list)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    list.ForEach(p => set.Add(p));
                    setClient.SaveAsync();
                    return true;
                }
            }
            catch { return false; }
        }
        public bool RemoveModelFromSet<T>(string key, T model)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    setClient.RemoveItemFromSet(set, model);
                    return true;
                }
            }
            catch { return false; }
        }
        public bool RemoveModelFromSet<T>(string key, Func<T, bool> where)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    List<T> list = set.ToList();
                    T model = list.Single<T>(where);
                    setClient.RemoveItemFromSet(set, model);
                    return true;
                }
            }
            catch { return false; }
        }
        public bool RemoveListFromSet<T>(string key, List<T> list)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    list.ForEach(p => set.Remove(p));
                    setClient.SaveAsync();
                    return true;
                }
            }
            catch { return false; }
        }
        public bool UpdataModelInSet<T>(string key, Func<T, bool> where, T model)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    List<T> list = set.ToList();
                    T item = list.Single<T>(where);
                    list.Remove(item);
                    list.Add(model);
                    client.Remove(key);
                    list.ForEach(p => set.Add(p));
                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }
        public List<T> GetAllListFromSet<T>(string key)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    return set.GetAll().ToList();
                }
            }
            catch (Exception ex)
            {
                string CurDir = @"d:\\tttt\\";
                if (!System.IO.Directory.Exists(CurDir)) System.IO.Directory.CreateDirectory(CurDir);   //该路径不存在时，在当前文件目录下创建文件夹"导出.."  

                //不存在该文件时先创建  
                String filePath = CurDir + "lll.txt";
                System.IO.StreamWriter file1 = new System.IO.StreamWriter(filePath, false);     //文件已覆盖方式添加内容  

                file1.Write(ex.Message);                                                              //保存数据到文件  

                file1.Close();                                                                  //关闭文件  
                file1.Dispose();
                return null;
            }
        }
        public T GetModelFromSet<T>(string key, Func<T, bool> where)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    List<T> list = set.ToList();
                    return list.Single<T>(where);
                }
            }
            catch { return default(T); }
        }
        public List<T> GetListFromSet<T>(string key, Func<T, bool> where)
        {
            try
            {
                using (IRedisClient client = _clientManager.GetClient())
                {
                    var setClient = client.As<T>();
                    var set = setClient.Sets[key];
                    List<T> list = set.ToList();
                    return list.Where<T>(where).ToList();
                }
            }
            catch (Exception ex) { return null; }
        }
    }
}
