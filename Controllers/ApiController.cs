using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Models.DTO;

namespace WebApplication3.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;

        private readonly IWebHostEnvironment _host;
        public ApiController(MyDBContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
        }
       
        [HttpGet]
        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(10000);
            //return Content("Hello Content");
            //return Content("<h2>Hello Content</h2>", "text/html");
            return Content("<h2>Hello Content，歡迎光臨</h2>","text/html",System.Text.Encoding.UTF8); //使用Content傳送字串(內容,HTML格式,UTF8編碼)
		}
        //public IActionResult Cities()
        //{
        //    //var districts = _dbContext.Addresses.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
        //    //return Json(districts);
        //}


        //public IActionResult Districts(string city)
        //{
        //    var districts = _dbContext.Addresses.Where(a =>a.City == city).Select(a => a.District).Distinct();)
        //    return Json(districts);
        //}

        public IActionResult Avatar(int id=1)
        {
           Member? member = _dbContext.Members.Find(id);
			if (member != null)
            {
                byte[] img = member.FileData;
				return File(img, "image/jpeg");
			}
            return NotFound();
		}

        // public IActionResult Register(string name, int age = 26)
        public IActionResult Register(UserDTO _user)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "Guest";
            }
            // todo 檔案存在的處理
            // todo 限制上傳的檔案類型
            // todo 限制上傳的檔案大小

            string fileName = "empty.jpg";
            if (_user.Avatar != null)
            {
                fileName = _user.Avatar.FileName;
            }
            //取得檔案上傳的實際路徑
            string uploadPath = Path.Combine(_host.WebRootPath, "uploads", fileName);
            //檔案上傳
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                _user.Avatar?.CopyTo(fileStream);
            } 

            //return Content($"Hello {_user.Name}, {_user.Age}歲了,電子郵件是{_user.Email}");
            return Content ($"{ _user.Avatar?.FileName}-{_user.Avatar?.Length}-{ _user.Avatar?.ContentType}");
        }

        public IActionResult Homework2(string name)
        {
            Member? member = _dbContext.Members.Where(x => x.Name == name).FirstOrDefault();

            if (member != null)
            {
                return Content($"{name}已經註冊過了");
            }
            return Content($"{name}您可以註冊");

        }

        public IActionResult Homework(string name)
        {
            Member? member = _dbContext.Members.Where(x => x.Name == name).FirstOrDefault();

            if (member != null)
            {
                return Content($"{name}已經註冊過了");
            }
            return Content($"{name}您可以註冊");

        }

        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO _search)
        {
            //根據分類編號讀取景點資料
            var spots = _search.categoryId == 0 ? _dbContext.SpotImagesSpots : _dbContext.SpotImagesSpots.Where(s => s.CategoryId == _search.categoryId);

            //根據關鍵字搜尋
            if (!string.IsNullOrEmpty(_search.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.keyword) || s.SpotDescription.Contains(_search.keyword));
            }

            //排序
            switch (_search.sortBy)
            {
                case "spotTitle":
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending
                        (s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending
                       (s => s.CategoryId);
                    break;
                default:
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending
                       (s => s.SpotId);
                    break;
            }

            //分頁
            int TotalCount = spots.Count(); //搜尋出來的資料總共有幾筆
            int pageSize = _search.pageSize ?? 9; //每頁多少筆資料
            int TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize); //計算出總共有幾頁
            int page = _search.Page ?? 1; //第幾頁

            //取出分頁資料
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

            //設計要回傳的資料格式
            SpotPagingDTO spotsPaging = new SpotPagingDTO();
            spotsPaging.TotalPages = TotalPages;
            spotsPaging.SpotsResult = spots.ToList();
            
            return Json(spotsPaging);
        }

        public IActionResult Categories()
        {
            return Json(_dbContext.Categories);
        }

        public IActionResult SpotsTitle(string keyword)
        {
            var spots = _dbContext.Spots.Where(s => s.SpotTitle.Contains(keyword))
                               .Select(s => s.SpotTitle).Take(8);
            return Json(spots);

        }
    }
}
