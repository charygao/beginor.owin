﻿using System;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using TestWithNowin.Logic;

namespace TestWithNowin.Controllers {

    [RoutePrefix("api/sample")]
    public class SampleController : ApiController {

        private IAuthenticationManager authenticationManager;
        private SampleManager manager;

        public SampleController(
            IAuthenticationManager authenticationManager,
            SampleManager manager
        ) {
            this.authenticationManager = authenticationManager;
            this.manager = manager;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                authenticationManager = null;
                manager = null;
            }
            base.Dispose(disposing);
        }

        [HttpGet, Route("anonymous")]
        public IHttpActionResult Anonymous() {
            return Ok("result from anonymous action");
        }

        [HttpGet, Route("security"), Authorize]
        public IHttpActionResult Security() {
            return Ok("result from security action, your name is: " + User.Identity.Name);
        }


        [HttpGet, Route("login/{user}")]
        public IHttpActionResult Login(string user) {
            var authMgr = Request.GetOwinContext().Authentication;

            var claim = new Claim(ClaimTypes.Name, user);
            var identity = new ClaimsIdentity(new[] { claim }, CookieAuthenticationDefaults.AuthenticationType);
            authMgr.SignIn(identity);
            return Ok(string.Format("user {0} logged in!", user));
        }

        [HttpGet, Route("error")]
        public IHttpActionResult Error() {
            throw new NotImplementedException();
        }
    }
}
