﻿using ParentCheck.BusinessObject;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class ReferenceDomain : IReferenceDomain
    {
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceDomain(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public async Task<List<ReferenceDTO>> GetReferenceByTypeAsync(int referenceTypeId, long userId)
        {
            return await _referenceRepository.GetReferenceByTypeAsync(referenceTypeId, userId);
        }
    }
}