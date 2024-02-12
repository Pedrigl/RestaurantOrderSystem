using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Web;
namespace TestUtilities.Data.Configuration
{
    public class FakeAutoMapper
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            return new Mapper(config);
        }
    }
}
