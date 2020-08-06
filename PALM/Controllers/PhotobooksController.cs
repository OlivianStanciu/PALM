using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PALM.Data;
using PALM.Models;
using PALM.Services.MediaStore;

namespace PALM.Controllers
{
    [Authorize]
    public class PhotobooksController : Controller
    {
        private MediaStoreService _mediaStoreService;

        public PhotobooksController(MediaStoreService mediaStoreService)
        {
            _mediaStoreService = mediaStoreService;
        }

        // GET: Photobooks
        public ActionResult Index()
        {
            return View(_mediaStoreService.Photobooks.Get().OrderByDescending(p => p.Priority));
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            return Json(_mediaStoreService.Photobooks.Get(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SortPhotobooks([FromBody]List<string> payload)
        {


            if (payload == null)
            {
                return BadRequest();
            }

            this.OrderPhotobooksByPriority(payload);
            return RedirectToAction(nameof(Index));
        }

        private void OrderPhotobooksByPriority(List<string> payload)
        {
            int priority = payload.Count;
            foreach (var photobookId in payload)
            {
                Photobook photobook = _mediaStoreService.Photobooks.Get(photobookId);
                photobook.Priority = priority;
                _mediaStoreService.Photobooks.Update(photobookId, photobook);
                priority--;
            }
        }


        // GET: Photobooks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }

        //    Photobook photoBook = _mediaStoreService.Photobooks.Get(id);

        //    if (photoBook == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(photoBook);
        //}

        // GET: Photobooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photobooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Photobook photobook, 
            [FromForm(Name="cover")]IFormFile cover, 
            [FromForm(Name = "files")]IEnumerable<IFormFile> files)
        {
            //var x = Request.Form.Files;
            if (ModelState.IsValid && cover != null && files?.Count() > 0)
            {
                try
                {

                    using var coverReadStream = cover.OpenReadStream();
                    MediaStreamContainer coverStreamContainer = new MediaStreamContainer(coverReadStream)
                    {
                        ContentType = cover.ContentType,
                        FileName = cover.FileName
                    };


                    List<MediaStreamContainer> fileStreamContainers = new List<MediaStreamContainer>();
                    foreach (var file in files)
                    {
                        MediaStreamContainer fileStream = new MediaStreamContainer(file.OpenReadStream())
                        {
                            ContentType = file.ContentType,
                            FileName = file.FileName
                        };
                        fileStreamContainers.Add(fileStream);
                    }


                    _mediaStoreService.Photobooks.Create(photobook, coverStreamContainer, fileStreamContainers);


                    //Disposable
                    coverStreamContainer.Dispose();
                    foreach (var f in fileStreamContainers)
                    {
                        f.Dispose();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("files", "Please upload at least one image");
                ModelState.AddModelError("cover", "Please select one image");
                return View(photobook);
            }
           
        }

        // GET: Photobooks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Photobook photoBook = _mediaStoreService.Photobooks.Get(id);

            if (photoBook == null)
            {
                return NotFound();
            }

            return View(photoBook);
        }

        // POST: Photobooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Photobook photobook)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Photobook p = _mediaStoreService.Photobooks.Get(photobook.Id);
                    p.Title = photobook.Title;
                    p.SubTitle = photobook.SubTitle;
                    p.Description = photobook.Description;
                    p.CoverDescription = photobook.CoverDescription;
                    p.Attributes = photobook.Attributes;

                    _mediaStoreService.Photobooks.Update(p.Id, p);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(photobook);
                }
            }
            return View(photobook);

        }

        // GET: Photobooks/Delete/5
        public PartialViewResult Delete(string id)
        {
            Photobook p = _mediaStoreService.Photobooks.Get(id);
            return PartialView(p);
        }

        // POST: Photobooks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                _mediaStoreService.Photobooks.Remove(id);

                //Reorder the remaining photobooks by priority
                List<string> payload = _mediaStoreService.Photobooks.Get().OrderByDescending(p => p.Priority).Select(p => p.Id).ToList<string>();
                this.OrderPhotobooksByPriority(payload);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}