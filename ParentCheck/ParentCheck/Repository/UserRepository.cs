using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class UserRepository: IUserRepository
    {
        private ParentcheckContext _parentcheckContext;

        public UserRepository(ParentcheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
