﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vitro.Startup))]
namespace Vitro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
