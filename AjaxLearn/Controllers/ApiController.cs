using AjaxLearn.Models;
using AjaxLearn.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace AjaxLearn.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ApiController(MyDBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            Thread.Sleep(50000);
            return Content("Hello Rick");
        }



        public IActionResult Category()
        {
            var Categories = _context.Categories;
            return Json(Categories);
        }

        public IActionResult Spot(int id = 1)
        {
            var Spots = _context.Spots.Where(s => s.CategoryId == id)
                .Select(s => new Spot { SpotTitle = s.SpotTitle, SpotId = s.SpotId });
            return Json(Spots);
        }

        public IActionResult Avatar(int id)
        {
            Member? member = _context.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                if (img != null)
                    return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult RegisterHW(UserDTO user)
        {
            //int a = 1;
            //int b = 0;
            //int c = a / b;
            //Thread.Sleep(2000);

            return Content((_context.Members.Any(m => m.Name == user.Name)).ToString());
        }

        public IActionResult RegisterFile(Member member, IFormFile? File)
        {
            string fileName = "empty.jpg";
            if (File != null)
            {
                fileName = File.FileName;
            }

            //string uploadPath = @"C:\Project\AjaxLearn\AjaxLearn\wwwroot\uploads\a.jpg";
            //上面這樣寫絕對路徑，實際部署時會出現問題。
            //_hostingEnvironment.WebRootPath  ==>"C:\Project\AjaxLearn\AjaxLearn\wwwroot"
            string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);

            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                File?.CopyTo(fileStream);
            }
            return Content($"上傳成功：{File?.FileName}-{File?.Length}-{File?.ContentType}", "text/plain", Encoding.UTF8);
        }

        public IActionResult querybyKeyword(string keyword)
        {
            var spots = _context.Spots.Where(s => s.SpotTitle.Contains(keyword)).Select(s => s.SpotTitle);
            return Json(spots);
        }

        [HttpPost]
        public IActionResult Register(Member _user, IFormFile File)
        {
            if (!_context.Members.Any(m => m.Name == _user.Name))
            {                           
                string errorMessage = "";
                if (File == null)
                {
                    errorMessage += "尚未附加檔案；";
                }
                else
                {
                    if (!IsImage(File.ContentType))          //1. 只允許上傳圖檔
                    {
                        errorMessage += $"檔案非圖片格式：{File.ContentType}；";
                    }
                    if(File.Length >= 1 * 1024 * 1024)        //2. 圖檔最大1M
                    {
                        errorMessage += $"超過1MB：{File.Length}；";
                    }
                    if (IsExist(File.FileName))               //3. 檔案名稱重複處理
                    {
                        errorMessage += $"檔案名稱已存在：{File.FileName}；";
                    }
                }

                //處理檔案部分
                if (errorMessage=="")
                {
                    //轉成二進位
                    byte[]? imgByte = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        File?.CopyTo(memoryStream);
                        imgByte = memoryStream.ToArray();
                    }
                    _user.FileData = imgByte;
                    _user.FileName = File?.FileName;

                    _context.Members.Add(_user);
                    _context.SaveChanges();
                }
                else
                {
                    return Content(errorMessage, "text/plain", Encoding.UTF8);
                }
                return Content($"會員新增成功！", "text/plain", Encoding.UTF8);

            }
            return Content($"{_user.Name}已存在", "text/plain", Encoding.UTF8);
        }

        private bool IsImage(string contentType)
        {
            string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
            return allowedImageTypes.Contains(contentType);
        }

        private bool IsExist(string fileName)
        {
            return _context.Members.Any(m => m.FileName == fileName);
        }


        //景點資料
        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO _search)
        {
            //根據分類編號搜尋
            var spots = _search.CategoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == _search.CategoryId);

            //根據關鍵字搜尋
            if (!string.IsNullOrEmpty(_search.Keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.Keyword) || s.SpotDescription.Contains(_search.Keyword));
            }

            //排序
            switch (_search.SortBy)
            {
                case "spotTitle":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
                    break;
                default: //spotId
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //總共有幾筆
            int totalCount = spots.Count();
            //一頁幾筆資料
            int pageSize = _search.PageSize ?? 9;
            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            //目前第幾頁
            int page = _search.Page ?? 1;


            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);


            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();

            return Json(spotsPaging);
        }

    }
}
