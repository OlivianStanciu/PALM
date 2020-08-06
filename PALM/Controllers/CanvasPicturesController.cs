using System;
using System.Collections.Generic;
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
    public class CanvasPicturesController : Controller
    {
        private MediaStoreService _mediaStoreService;
        public CanvasPicturesController(MediaStoreService mediaStoreService)
        {
            _mediaStoreService = mediaStoreService;
        }
        // GET: CanvasPictures
        public ActionResult Index()
        {
            return View(_mediaStoreService.CanvasPictures.Get().OrderByDescending(p => p.Priority));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SortCanvasPictures([FromBody]List<string> payload)
        {

            if (payload == null)
            {
                return BadRequest();
            }
            this.OrderCanvasPicturesByPriority(payload);
            return RedirectToAction(nameof(Index));
        }

        private void OrderCanvasPicturesByPriority(List<string> payload)
        {
            int priority = payload.Count;
            foreach (var canvasId in payload)
            {
                CanvasPicture canvas = _mediaStoreService.CanvasPictures.Get(canvasId);
                canvas.Priority = priority;
                _mediaStoreService.CanvasPictures.Update(canvasId, canvas);
                priority--;
            }
        }


        // GET: CanvasPictures/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            CanvasPicture canvas = _mediaStoreService.CanvasPictures.Get(id);

            if (canvas == null)
            {
                return NotFound();
            }

            return View(canvas);
        }

        // GET: CanvasPictures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CanvasPictures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CanvasPicture canvas,
            [FromForm(Name = "cover")]IFormFile cover,
            [FromForm(Name = "canvasPicture")]IFormFile canvasPicture)
        {
            //var x = Request.Form.Files;
            if (ModelState.IsValid && cover != null && canvasPicture != null)
            {
                try
                {

                    using var coverReadStream = cover.OpenReadStream();
                    MediaStreamContainer coverStreamContainer = new MediaStreamContainer(coverReadStream)
                    {
                        ContentType = cover.ContentType,
                        FileName = cover.FileName
                    };

                    using var canvasPictureReadStream = canvasPicture.OpenReadStream();
                    MediaStreamContainer canvasPictureStreamContainer = new MediaStreamContainer(canvasPictureReadStream)
                    {
                        ContentType = canvasPicture.ContentType,
                        FileName = canvasPicture.FileName
                    };


                    _mediaStoreService.CanvasPictures.Create(canvas, coverStreamContainer, canvasPictureStreamContainer);

                    coverStreamContainer.Dispose();
                    canvasPictureStreamContainer.Dispose();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("canvasPicture", "Please select one image");
                ModelState.AddModelError("cover", "Please select one image");
                return View(canvas);
            }

        }


        // GET: CanvasPictures/Delete/5
        public PartialViewResult Delete(string id)
        {
            CanvasPicture canvas = _mediaStoreService.CanvasPictures.Get(id);
            return PartialView(canvas);
        }

        // POST: CanvasPictures/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                _mediaStoreService.CanvasPictures.Remove(id);

                //Reorder the remaining photobooks by priority
                List<string> payload = _mediaStoreService.CanvasPictures.Get().OrderByDescending(c => c.Priority).Select(c => c.Id).ToList<string>();
                this.OrderCanvasPicturesByPriority(payload);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}