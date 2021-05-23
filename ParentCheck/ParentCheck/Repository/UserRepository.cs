using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class UserRepository: IUserRepository
    {
        private ParentCheckContext _parentcheckContext;

        public UserRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
