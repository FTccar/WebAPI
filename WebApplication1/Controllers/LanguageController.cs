using Programming.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Attributes;
using WebApplication1.Security;

namespace WebApplication1.Controllers
{
    [ApiExceptionAttribute]
    public class LanguageController : ApiController
    {
        LanguageDAL languageDAL = new LanguageDAL();

        [HttpGet]
        public IHttpActionResult SerachByName(string name)
        {
            return Ok("Name :" + name);
        }

        [HttpGet]
        public IHttpActionResult SerachBySurName(string surName)
        {
            return Ok("Surname :" + surName);
        }

        [ResponseType(typeof(IEnumerable<Languages>))]
        [HttpGet] // Bu methodun GET işleminde çalışacağını bildiriyor. 
        //[NonAction] bu methodu kullanarak bu methodun dışarıya açılmasını engelleyebilirim
        //[Authorize] Bu method yerine artık APIAutorize isimli kendi Authorize methodumuzu yazdık
        [APIAutorize(Roles ="M,U,A")] // Sadece Rolü M,U,A olan kullanıcılar çalıştırabilsin
        public IHttpActionResult GetAllLanguages()
        {
            var languages = languageDAL.GetAllLanguage();
            //Request.CreateResponse(HttpStatusCode.OK, languages);
            return Ok(languages);
        }

        [ResponseType(typeof(Languages))]
        public HttpResponseMessage Get(int id)
        {
            var language = languageDAL.GetLanguageById(id);
            if (language == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Böyle bir kayıt bulunamadı!");
            return Request.CreateResponse(HttpStatusCode.OK, language);
        }

        [ResponseType(typeof(Languages))]
        public IHttpActionResult Post(Languages language)
        {
            if (ModelState.IsValid)
            {
                var _language = languageDAL.CreateLanguage(language);
                //Request.CreateResponse(HttpStatusCode.OK, _language);
                return CreatedAtRoute("DefaultApi", new { id = _language.Id }, _language);
            }
            else
            {
                //return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                return BadRequest(ModelState);
            }

        }

        [ResponseType(typeof(Languages))]
        public HttpResponseMessage Put(int id, Languages language)
        {
            if (languageDAL.IsThereAnyLanguage(id) == true)
            {
                if (ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, languageDAL.UpdateLanguage(id, language));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Kayıt bulunamadı.");
            }
        }

        public IHttpActionResult Delete(int id)
        {
            languageDAL.DeleteLanguage(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
