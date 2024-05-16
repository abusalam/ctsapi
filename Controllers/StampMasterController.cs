using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.Common;
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

        [HttpGet("GetALLStampLabels")]
        public async Task<APIResponse<IEnumerable<StampLabelMasterDTO>>> GetALLStampLabels()
        {
            APIResponse<IEnumerable<StampLabelMasterDTO>> response = new();
            try
            {
                IEnumerable<StampLabelMasterDTO> allStampLabels = await _stampMasterService.GetAllStampLabels();
                if (allStampLabels.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampLabels;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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
                IEnumerable<StampLabelMasterDTO> allStampLabels = (IEnumerable<StampLabelMasterDTO>)await _stampMasterService.GetStampLabelById(id);
                if (allStampLabels.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampLabels;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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

        [HttpGet("GetALLStampCategories")]
        public async Task<APIResponse<IEnumerable<StampCategoryDTO>>> GetALLStampCategorys()
        {
            APIResponse<IEnumerable<StampCategoryDTO>> response = new();
            try
            {
                IEnumerable<StampCategoryDTO> allStampCategorys = (IEnumerable<StampCategoryDTO>)await _stampMasterService.GetAllStampCategories();
                if (allStampCategorys.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCategorys;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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


        [HttpGet("GetStampCategorysById")]
        public async Task<APIResponse<IEnumerable<StampCategoryDTO>>> GetStampCategorysById(long id)
        {
            APIResponse<IEnumerable<StampCategoryDTO>> response = new();
            try
            {
                IEnumerable<StampCategoryDTO> allStampCategorys = (IEnumerable<StampCategoryDTO>)await _stampMasterService.GetStampCategoryById(id);
                if (allStampCategorys.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampCategorys;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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

        [HttpDelete("DeleteStampCategorysById")]
        public async Task<APIResponse<bool>> DeleteStampCategorysById(long id)
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

        [HttpGet("GetALLStampVendors")]
        public async Task<APIResponse<IEnumerable<StampVendorDTO>>> GetALLStampVendors()
        {
            APIResponse<IEnumerable<StampVendorDTO>> response = new();
            try
            {
                IEnumerable<StampVendorDTO> allStampVendors = (IEnumerable<StampVendorDTO>)await _stampMasterService.GetAllStampVendors();
                if (allStampVendors.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampVendors;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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
                IEnumerable<StampVendorDTO> allStampVendors = (IEnumerable<StampVendorDTO>)await _stampMasterService.GetStampVendorById(id);
                if (allStampVendors.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampVendors;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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
        public async Task<APIResponse<bool>> CreateStampVendor(StampVendorInsertDTO stampVendor)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampVendor != null)
                {
                    if(await _stampMasterService.CreateNewStampVendor(stampVendor))
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

        [HttpGet("GetALLStampTypes")]
        public async Task<APIResponse<IEnumerable<StampTypeDTO>>> GetALLStampTypes()
        {
            APIResponse<IEnumerable<StampTypeDTO>> response = new();
            try
            {
                IEnumerable<StampTypeDTO> allStampTypes = (IEnumerable<StampTypeDTO>)await _stampMasterService.GetAllStampTypes();
                if (allStampTypes.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampTypes;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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
                IEnumerable<StampTypeDTO> allStampTypes = (IEnumerable<StampTypeDTO>)await _stampMasterService.GetStampTypeById(id);
                if (allStampTypes.Count() > 0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allStampTypes;
                    response.Message = AppConstants.DataFound;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
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

    }
}
