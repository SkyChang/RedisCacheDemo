using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedisCacheDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //設定連線字串
            ConnectionMultiplexer connection = 
                ConnectionMultiplexer.Connect(
                "skyrediscache.redis.cache.windows.net,ssl=true,password=...");
            
            IDatabase cache = connection.GetDatabase();

            //存放簡單的形態到Cache
            cache.StringSet("key1", "value");
            cache.StringSet("key2", 25);

            //取得資料
            ViewData["key1"] = cache.StringGet("key1");
            ViewData["key2"] = (int)cache.StringGet("key2");


            //Cache-Aside Pattern
            string value = cache.StringGet("key1");
            if (value == null)
            {
                //如果找不到資料，就從資料庫取得
                //因沒有實作GetValueFromDataSource()，所以下面先註解掉
                //value = GetValueFromDataSource();
                cache.StringSet("key1", value);
            }

            //指定時間
            cache.StringSet("key1", "value1", TimeSpan.FromMinutes(90));

            return View();
        }

        public ActionResult SessionTest()
        {
            Session["Test"] = "Hello Redis";

            return View();
        }
    }
}