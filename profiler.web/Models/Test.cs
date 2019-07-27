using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace profiler.web.Models
{
    public class Test
    {
    }


    public interface IUser
    {
        string Name { get; set; }
        string Id { get; set; }
    }

    public interface IBizUser : IUser
    {
        string GetAnalysis();
    }


    public class User : IUser
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class BizUser : User, IBizUser
    {
        public string GetAnalysis()
        {
            throw new NotImplementedException();
        }
    }
}
