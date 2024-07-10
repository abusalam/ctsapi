using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Services.stamp;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StampMasterController : Controller
    {
        private readonly IStampMasterService _stampMasterService;

        public StampMasterController(IStampMasterService stampMasterService)
        {
            _stampMasterService = stampMasterService;
        }

        // Stamp Label
        [HttpPatch("StampLabelList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampLabelMasterDTO>>>> StampLabelList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp labels with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampLabelMasterDTO>>> response = new();
            try
            {
                IEnumerable<StampLabelMasterDTO> labelList = await _stampMasterService.ListAllStampLabels(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampLabelMasterDTO>> result = new DynamicListResult<IEnumerable<StampLabelMasterDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Label ID",
                            DataType = "numeric",
                            FieldName = "labelId",
                            FilterField = "Label Id",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="No: of Label/Sheet",
                            DataType = "numeric",
                            FieldName = "noLabelPerSheet",
                            FilterField = "no: of Label/Sheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Is Active",
                            DataType = "boolean",
                            FieldName = "isActive",
                            FilterField = "IsActive",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "Created At",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },
                    Data = labelList,
                    DataCount = 1
                };
                if (labelList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampLabels")]
        public async Task<APIResponse<IEnumerable<StampLabelDTO>>> GetALLStampLabels()
        {
            APIResponse<IEnumerable<StampLabelDTO>> response = new();
            try
            {
                IEnumerable<StampLabelDTO> allStampLabels = await _stampMasterService.GetAllStampLabels();
                if (allStampLabels.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampLabels;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet("GetStampLabelsById")]
        public async Task<APIResponse<IEnumerable<StampLabelMasterDTO>>> GetStampLabelsById(long id)
        {
            APIResponse<IEnumerable<StampLabelMasterDTO>> response = new();
            try
            {
                IEnumerable<StampLabelMasterDTO> allStampLabels = await _stampMasterService.GetStampLabelById(id);
                if (allStampLabels.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampLabels;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPost("CreateStampLabel")]
        public async Task<APIResponse<bool>> CreateStampLabel(StampLabelMasterInsertDTO stampLabel)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampLabel != null)
                {
                    if(await _stampMasterService.CreateNewStampLabel(stampLabel))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampLabelsById")]
        public async Task<APIResponse<bool>> DeleteStampLabelsById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                ;
                if (await _stampMasterService.DeleteStampLabelsById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // Stamp Category
        [HttpPatch("StampCategoryList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampCategoryDTO>>>> StampCategoryList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp categories with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampCategoryDTO>>> response = new();
            try
            {
                IEnumerable<StampCategoryDTO> categoryList = await _stampMasterService.ListAllStampCategories(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampCategoryDTO>> result = new DynamicListResult<IEnumerable<StampCategoryDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Category Id",
                            DataType = "numeric",
                            FieldName = "stampCategoryId",
                            FilterField = "StampCategoryId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stampCategory1",
                            FilterField = "StampCategory1",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "text",
                            FieldName = "description",
                            FilterField = "Description",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Is Active",
                            DataType = "boolean",
                            FieldName = "isActive",
                            FilterField = "IsActive",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                    },
                Data = categoryList,
                    DataCount = 1
                };
                if (categoryList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                }
                response.result = result;
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampCategories")]
        public async Task<APIResponse<IEnumerable<StampCategoryDTO>>> GetALLStampCategories()
        {
            APIResponse<IEnumerable<StampCategoryDTO>> response = new();
            try
            {
                IEnumerable<StampCategoryDTO> allStampCategorys = await _stampMasterService.GetAllStampCategories();
                if (allStampCategorys.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCategorys;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampCategoryTypes")]
        public async Task<APIResponse<IEnumerable<CategoryTypeDTO>>> GetALLStampCategoryTypes()
        {
            APIResponse<IEnumerable<CategoryTypeDTO>> response = new();
            try
            {
                IEnumerable<CategoryTypeDTO> allStampCategorys = await _stampMasterService.GetAllCategoryType();
                if (allStampCategorys.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCategorys;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet("GetStampCategoryById")]
        public async Task<APIResponse<IEnumerable<StampCategoryDTO>>> GetStampCategoryById(long id)
        {
            APIResponse<IEnumerable<StampCategoryDTO>> response = new();
            try
            {
                IEnumerable<StampCategoryDTO> allStampCategorys = await _stampMasterService.GetStampCategoryById(id);
                if (allStampCategorys.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCategorys;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        
        [HttpPost("CreateStampCategory")]
        public async Task<APIResponse<bool>> CreateStampCategory(StampCategoryInsertDTO stampCategory)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampCategory != null)
                {
                    if(await _stampMasterService.CreateNewStampCategory(stampCategory))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampCategoryById")]
        public async Task<APIResponse<bool>> DeleteStampCategoryById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                ;
                if (await _stampMasterService.DeleteStampCategorysById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        
        // Stamp Vendor
        [HttpPatch("StampVendorList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampVendorDTO>>>> StampVendorList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp vendors with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampVendorDTO>>> response = new();
            try
            {
                IEnumerable<StampVendorDTO> vendorList = await _stampMasterService.ListAllStampVendors(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampVendorDTO>> result = new DynamicListResult<IEnumerable<StampVendorDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Vendor Id",
                            DataType = "numeric",
                            FieldName = "stampVendorId",
                            FilterField = "StampVendorId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Vendor Name",
                            DataType = "text",
                            FieldName = "vendorName",
                            FilterField = "VendorName",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Vendor Type",
                            DataType = "numeric",
                            FieldName = "vendorType",
                            FilterField = "VendorType",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Treasury",
                            DataType = "text",
                            FieldName = "vendorTreasury",
                            FilterField = "VendorTreasury",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="License No",
                            DataType = "text",
                            FieldName = "licenseNo",
                            FilterField = "LicenseNo",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Address",
                            DataType = "text",
                            FieldName = "address",
                            FilterField = "Address",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Phone Number",
                            DataType = "numeric",
                            FieldName = "phoneNumber",
                            FilterField = "PhoneNumber",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Effective From",
                            DataType = "date",
                            FieldName = "effectiveFrom",
                            FilterField = "EffectiveFrom",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Valid Upto",
                            DataType = "data",
                            FieldName = "validUpto",
                            FilterField = "ValidUpto",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Pan Number",
                            DataType = "text",
                            FieldName = "panNumber",
                            FilterField = "PanNumber",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Is Active",
                            DataType = "boolean",
                            FieldName = "isActive",
                            FilterField = "IsActive",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Active At Grips",
                            DataType = "boolean",
                            FieldName = "activeAtGrips",
                            FilterField = "ActiveAtGrips",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Vendor Photo",
                            DataType = "text",
                            FieldName = "vendorPhoto",
                            FilterField = "VendorPhoto",
                            IsFilterable = true,
                            IsSortable = true,
                        },new ListHeader
                        {
                            Name ="Vendor Pan Photo",
                            DataType = "text",
                            FieldName = "vendorPanPhoto",
                            FilterField = "VendorPanPhoto",
                            IsFilterable = true,
                            IsSortable = true,
                        },new ListHeader
                        {
                            Name ="vendor Licence Photo",
                            DataType = "text",
                            FieldName = "vendorLicencePhoto",
                            FilterField = "VendorLicencePhoto",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },
                Data = vendorList,
                    DataCount = 1
                };
                if (vendorList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampVendors")]
        public async Task<APIResponse<IEnumerable<StampVendorDetailsDropdownDTO>>> GetALLStampVendors()
        {
            APIResponse<IEnumerable<StampVendorDetailsDropdownDTO>> response = new();
            try
            {
                IEnumerable<StampVendorDetailsDropdownDTO> allStampVendors = await _stampMasterService.GetAllStampVendors();
                if (allStampVendors.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampVendors;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampVendorTypes")]
        public async Task<APIResponse<IEnumerable<VendorTypeDTO>>> GetALLStampVendorTypes()
        {
            APIResponse<IEnumerable<VendorTypeDTO>> response = new();
            try
            {
                IEnumerable<VendorTypeDTO> allStampVendors = await _stampMasterService.GetAllStampVendorTypes();
                if (allStampVendors.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampVendors;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet("GetStampVendorsById")]
        public async Task<APIResponse<IEnumerable<StampVendorDTO>>> GetStampVendorsById(long id)
        {
            APIResponse<IEnumerable<StampVendorDTO>> response = new();
            try
            {
                IEnumerable<StampVendorDTO> allStampVendors = await _stampMasterService.GetStampVendorById(id);
                if (allStampVendors.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampVendors;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPost("CreateStampVendor")]
        public async Task<APIResponse<bool>> CreateStampVendor([FromForm] StampVendorInsertDTO stampVendor, IFormFile vendorPhoto, IFormFile vendorPanPhoto, IFormFile vendorLicencePhoto)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampVendor != null)
                {
                    // Ensure the uploads directory exists
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    string photoPath = "";
                    string panPhotoPath = "";
                    string licencePhotoPath = "";

                    if (vendorPhoto != null)
                    {
                        photoPath = Path.Combine(uploadsDir, Path.GetFileName(vendorPhoto.FileName));
                        using (var stream = new FileStream(photoPath, FileMode.Create))
                        {
                            await vendorPhoto.CopyToAsync(stream);
                        }
                    }

                    if (vendorPanPhoto != null)
                    {
                        panPhotoPath = Path.Combine(uploadsDir, Path.GetFileName(vendorPanPhoto.FileName));
                        using (var stream = new FileStream(panPhotoPath, FileMode.Create))
                        {
                            await vendorPanPhoto.CopyToAsync(stream);
                        }
                    }

                    if (vendorLicencePhoto != null)
                    {
                        licencePhotoPath = Path.Combine(uploadsDir, Path.GetFileName(vendorLicencePhoto.FileName));
                        using (var stream = new FileStream(licencePhotoPath, FileMode.Create))
                        {
                            await vendorLicencePhoto.CopyToAsync(stream);
                        }
                    }

                    if (await _stampMasterService.CreateNewStampVendor(stampVendor, photoPath, panPhotoPath, licencePhotoPath))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }

                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampVendorsById")]
        public async Task<APIResponse<bool>> DeleteStampVendorsById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                ;
                if (await _stampMasterService.DeleteStampVendorsById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // Stamp Type
        [HttpPatch("StampTypeList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampTypeDTO>>>> StampTypeList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp types with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampTypeDTO>>> response = new();
            try
            {
                IEnumerable<StampTypeDTO> typeList = await _stampMasterService.ListAllStampTypes(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampTypeDTO>> result = new DynamicListResult<IEnumerable<StampTypeDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Denomination ID",
                            DataType = "numeric",
                            FieldName = "denominationId",
                            FilterField = "DenominationId",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Denomination",
                            DataType = "numeric",
                            FieldName = "denomination",
                            FilterField = "Denomination",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Is Active",
                            DataType = "boolean",
                            FieldName = "isActive",
                            FilterField = "IsActive",
                            IsFilterable = true,
                            IsSortable = true,
                        }, 
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "Created At",
                            IsFilterable = true,
                            IsSortable = true,
                        }, 
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },
                    Data = typeList,
                    DataCount = 1
                };
                if (typeList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampTypes")]
        public async Task<APIResponse<IEnumerable<StampTypeDataDTO>>> GetALLStampTypes()
        {
            APIResponse<IEnumerable<StampTypeDataDTO>> response = new();
            try
            {
                IEnumerable<StampTypeDataDTO> allStampTypes = await _stampMasterService.GetAllStampTypes();
                if (allStampTypes.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampTypes;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }


        [HttpGet("GetStampTypesById")]
        public async Task<APIResponse<IEnumerable<StampTypeDTO>>> GetStampTypesById(long id)
        {
            APIResponse<IEnumerable<StampTypeDTO>> response = new();
            try
            {
                IEnumerable<StampTypeDTO> allStampTypes = await _stampMasterService.GetStampTypeById(id);
                if (allStampTypes.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampTypes;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPost("CreateStampType")]
        public async Task<APIResponse<bool>> CreateStampType(StampTypeInsertDTO stampType)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampType != null)
                {
                    if (await _stampMasterService.CreateNewStampType(stampType))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampTypesById")]
        public async Task<APIResponse<bool>> DeleteStampTypesById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                ;
                if (await _stampMasterService.DeleteStampTypesById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        // Stamp discount details
        [HttpPatch("StampDiscountDetailsList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<DiscountDetailsDTO>>>> StampDiscountDetailsList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp discount details with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<DiscountDetailsDTO>>> response = new();
            try
            {
                IEnumerable<DiscountDetailsDTO> discountDetailsList = await _stampMasterService.ListAllDiscountDetails(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<DiscountDetailsDTO>> result = new DynamicListResult<IEnumerable<DiscountDetailsDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Discount ID",
                            DataType = "numeric",
                            FieldName = "discountId",
                            FilterField = "Discount ID",
                            IsFilterable = false,
                            IsSortable = false,
                        },
                        new ListHeader
                        {
                            Name ="Denomination From",
                            DataType = "text",
                            FieldName = "denominationFrom",
                            FilterField = "Denomination From",
                            IsFilterable = true,
                            IsSortable = true,
                        }, 
                        new ListHeader
                        {
                            Name ="Denomination To",
                            DataType = "text",
                            FieldName = "denominationTo",
                            FilterField = "Denomination To",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Discount (%)",
                            DataType = "numeric",
                            FieldName = "discount",
                            FilterField = "Discount",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Vendor Type",
                            DataType = "text",
                            FieldName = "vendorType",
                            FilterField = "Vendor Type",
                            IsFilterable = true,
                            IsSortable = true,
                        }, 
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stampCategory",
                            FilterField = "StampCategory",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Is Active",
                            DataType = "boolean",
                            FieldName = "isActive",
                            FilterField = "IsActive",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },
                    Data = discountDetailsList,
                    DataCount = 1
                };
                if (discountDetailsList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpPost("CreateStampDiscountDetails")]
        public async Task<APIResponse<bool>> CreateStampDiscountDetails(DiscountDetailsInsertDTO stampDiscountDetails)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampDiscountDetails != null)
                {
                    if (await _stampMasterService.CreateNewStampDiscountDetails(stampDiscountDetails))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampDiscountDetailsById")]
        public async Task<APIResponse<bool>> DeleteStampDiscountDetailsById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                
                if (await _stampMasterService.DeleteStampDiscountDetailsById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        
        [HttpGet("GetDiscount")]
        public async Task<APIResponse<decimal>> GetDiscount(long vendorTypeId, long stampCategoryId, decimal amount)
        {
            APIResponse<decimal> response = new();
            try
            {
                var discount = await _stampMasterService.GetDiscount(vendorTypeId, stampCategoryId, amount);
                if (discount != null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = discount;
                    response.Message = AppConstants.DataFound;
                }
                else
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.result = 0;
                    response.Message = AppConstants.DataNotFound;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
       
        // Stamp combination details
        [HttpPatch("StampCombinationList")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<StampCombinationDTO>>>> StampCombinationList(DynamicListQueryParameters dynamicListQueryParameters)
        {
            //Get all stamp discount details with sort & filter parameters
            APIResponse<DynamicListResult<IEnumerable<StampCombinationDTO>>> response = new();
            try
            {
                IEnumerable<StampCombinationDTO> discountDetailsList = await _stampMasterService.StampCombinationList(dynamicListQueryParameters.filterParameters, dynamicListQueryParameters.PageIndex, dynamicListQueryParameters.PageSize, dynamicListQueryParameters.sortParameters);
                DynamicListResult<IEnumerable<StampCombinationDTO>> result = new DynamicListResult<IEnumerable<StampCombinationDTO>>
                {
                    Headers = new List<ListHeader>
                    {
                        new ListHeader
                        {
                            Name ="Stamp Combination Id",
                            DataType = "numeric",
                            FieldName = "stampCombinationId",
                            FilterField = "StampCombinationId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category Id",
                            DataType = "numeric",
                            FieldName = "stampCategoryId",
                            FilterField = "stampCategoryId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Category",
                            DataType = "text",
                            FieldName = "stampCategory1",
                            FilterField = "StampCategory1",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Description",
                            DataType = "text",
                            FieldName = "description",
                            FilterField = "Description",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Denomination",
                            DataType = "decimal",
                            FieldName = "denomination",
                            FilterField = "Denomination",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Denomination Id",
                            DataType = "numeric",
                            FieldName = "stampDenominationId",
                            FilterField = "StampDenominationId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="No Label Per Sheet",
                            DataType = "numeric",
                            FieldName = "noLabelPerSheet",
                            FilterField = "NoLabelPerSheet",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Stamp Label Id",
                            DataType = "numeric",
                            FieldName = "stampLabelId",
                            FilterField = "StampLabelId",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created At",
                            DataType = "text",
                            FieldName = "createdAt",
                            FilterField = "CreatedAt",
                            IsFilterable = true,
                            IsSortable = true,
                        },
                        new ListHeader
                        {
                            Name ="Created By",
                            DataType = "numeric",
                            FieldName = "createdBy",
                            FilterField = "CreatedBy",
                            IsFilterable = true,
                            IsSortable = true,
                        }
                    },
                Data = discountDetailsList,
                    DataCount = 1
                };
                if (discountDetailsList.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                else
                {
                    response.Message = AppConstants.DataNotFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                }
                response.result = result;
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }

        [HttpGet("GetALLStampCombinations")]        
        public async Task<APIResponse<IEnumerable<GetAllStampCombinationDTO>>> GetALLStampCombinations()
        {
            APIResponse<IEnumerable<GetAllStampCombinationDTO>> response = new();
            try
            {
                IEnumerable<GetAllStampCombinationDTO> allStampCombinations = await _stampMasterService.GetAllStampCombinations();
                if (allStampCombinations.Count() > 0)
                {
                    response.Message = AppConstants.DataFound;
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCombinations;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPost("CreateStampCombination")]
        public async Task<APIResponse<bool>> CreateStampCombination(StampCombinationInsertDTO newStampCombination)
        {
            APIResponse<bool> response = new();
            try
            {
                if (newStampCombination != null)
                {
                    if (await _stampMasterService.CreateNewStampCombination(newStampCombination))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataAdded;
                        response.result = true;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpDelete("DeleteStampCombinationById")]
        public async Task<APIResponse<bool>> DeleteStampCombinationById(long id)
        {
            APIResponse<bool> response = new();
            try
            {
                ;
                if (await _stampMasterService.DeleteStampCombinationById(id))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = true;
                    response.Message = AppConstants.Deleted;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.DataNotFound;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
    }

}

