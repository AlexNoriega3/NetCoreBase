using Bll.commons;
using Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Entities;

namespace Bll
{
    public class RoleBll : BaseBll, IRoleBll
    {
        public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
        {
            IEnumerable<RoleDto> Roles = null;

            try
            {
                IEnumerable<Role> RolesEntity = await this._UOW.RoleRepository
                    .GetAllAsync();

                Roles = this._mapper.Map<IEnumerable<RoleDto>>(RolesEntity);
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to load Roles");
            }

            return Ok(Roles);
        }

        public async Task<ActionResult<PagedResult<RoleDto>>> Get(QueryParameters paramsQuery)
        {
            PagedResult<RoleDto> Roles = null;

            try
            {
                Roles = await this._UOW.RoleRepository
                    .GetAllAsync<RoleDto>(paramsQuery,
                    !string.IsNullOrWhiteSpace(paramsQuery.Search) ?
                    (w =>
                       w.Name.Contains(paramsQuery.Search)
                    )
                    : null
                    );
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to load Roles");
            }

            return Ok(Roles);
        }

        public async Task<ActionResult<RoleDto>> Get(string RoleId)
        {
            RoleDto Role = null;

            try
            {
                Role RolesEntity = this._UOW.RoleRepository
                    .FindBy(p => p.Id == RoleId)?
                    .FirstOrDefault();

                Role = this._mapper.Map<RoleDto>(RolesEntity);
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to get Role");
            }

            return Ok(Role);
        }

        public async Task<ActionResult<string>> Add(RoleDto Role)
        {
            string RoleId = string.Empty;

            try
            {
                Role RoleEntity = this._mapper.Map<Role>(Role);
                this._UOW.RoleRepository.Add(RoleEntity);
                this._UOW.Save();

                RoleId = RoleEntity.Id;
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to add an Role");
            }

            return Ok(RoleId);
        }

        public async Task<ActionResult<bool>> Edit(string RoleId, RoleDto edit)
        {
            bool success = false;

            try
            {
                Role RoleEntity = await this._UOW.RoleRepository
                    .FindTrackingAsync(p => p.Id == RoleId);

                if (RoleEntity is null)
                {
                    return NotFound();
                }

                RoleEntity = this._mapper.Map(edit, RoleEntity);

                this._UOW.RoleRepository.Update(RoleEntity, RoleId);
                this._UOW.Save();
                success = true;
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to edit an Role");
            }

            return Ok(success);
        }

        public async Task<ActionResult<bool>> Delete(string RoleId)
        {
            bool success = false;

            try
            {
                Role RoleEntity = this._UOW.RoleRepository
                   .Find(p => p.Id == RoleId);

                this._UOW.RoleRepository.Delete(RoleEntity);
                this._UOW.Save();
                success = true;
            }
            catch (Exception e)
            {
                this._logger.Error(e, e.Message);
                return BadRequest("Server error to delete Roles");
            }

            return Ok(success);
        }
    }
}