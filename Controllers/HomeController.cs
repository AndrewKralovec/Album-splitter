using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO; 
using System.IO.Compression;
using System.Net.Http.Headers; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http; 
using TrackSplitter.Models; 

namespace TrackSplitter.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment hostingEnv;
        public HomeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadTrack(Track track)
        {
            string path = hostingEnv.WebRootPath + $@"/Temp/temp/{track.file.FileName}" ; 
            string temp = string.Concat(hostingEnv.WebRootPath, "/Temp/temp"); 
            // empty the temp folder
            Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));
            
            using (FileStream fs = System.IO.File.Create(path)){
                track.file.CopyTo(fs);
                fs.Flush();
            }
            Splitter splitter = new Splitter(temp); 
            if(splitter.split(track)){
                return Json("Worked"); 
            }else{
                return Json("Did not work");
            }
        }
        [HttpPost]
        public FileResult Download()
        {
            string temp = string.Concat(hostingEnv.WebRootPath, "/Temp/temp"); 
            var archive = hostingEnv.WebRootPath + $@"/Temp/archive.zip"; 
            // clear any existing archive
            if (System.IO.File.Exists(archive)){
                System.IO.File.Delete(archive);
            }
            ZipFile.CreateFromDirectory(temp, archive);
            return File(archive, "application/zip", "archive.zip");
        }
    }
}
