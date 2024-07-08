﻿using AutoMapper;
using CTS_BE.BAL.Interfaces.stampRequisition;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using CTS_BE.Enum;
using CTS_BE.Helper.Authentication;
using Renci.SshNet;
using System.Reflection.Emit;

namespace CTS_BE.BAL.Services.stampRequisition
{
    public class StampRequisitionService : IStampRequisitionService
    {
        private readonly IStampRequisitionRepository _stampRequisitionRepo;
        //private readonly IStampIndentRepository _stampIndentRepo;
        //private readonly IStampInvoiceRepository _stampInvoiceRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampRequisitionService(
            IStampRequisitionRepository stampRequisitionRepo,
            //IStampIndentRepository stampIndentRepo,
            //IStampInvoiceRepository stampInvoiceRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampRequisitionRepo = stampRequisitionRepo;
            //_stampIndentRepo = stampIndentRepo;
            //_stampInvoiceRepo = stampInvoiceRepo;
            _mapper = mapper;
            _auth = claim;
        }
        
        // ======================Stamp Requisitions===========================
        public async Task<IEnumerable<StampRequisitionDTO>> ListAllStampRequisitions(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null)
        {
            IEnumerable<StampRequisitionDTO> stampRequisitionList = await _stampRequisitionRepo.GetSelectedColumnByConditionAsync(
                entity => true,
            entity => new StampRequisitionDTO
            {
                VendorStampRequisitionId = entity.VendorStampRequisitionId,
                VendorId = entity.VendorId,
                VendorType = entity.Vendor.VendorTypeNavigation.VendorType,
                LicenseNo = entity.Vendor.LicenseNo,
                Amount = (entity.Sheet * entity.Combination.StampLabel.NoLabelPerSheet + entity.Label) * entity.Combination.StampType.Denomination,
                Status = entity.Status.Name,
                RequisitionDate = entity.RequisitionDate
            }, pageIndex, pageSize, filters, (sortParameters != null) ? sortParameters.Field : null, (sortParameters != null) ? sortParameters.Order : null);
            return stampRequisitionList;
        }

    }
}