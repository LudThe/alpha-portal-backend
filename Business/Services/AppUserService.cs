using Business.Factories;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class AppUserService(IAppUserRepository appUserRepository, IAppUserProfileRepository appUserProfileRepository, IAppUserAddressRepository appUserAddressRepository)
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly IAppUserProfileRepository _appUserProfileRepository = appUserProfileRepository;
    private readonly IAppUserAddressRepository _appUserAddressRepository = appUserAddressRepository;


    public async Task<IEnumerable<AppUser>> GetAll()
    {
        var entities = await _appUserRepository.GetAllAsync(
             orderByDescending: true,
                sortBy: x => x.AppUserProfile!.FirstName,
                filterBy: null,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );
        var appUsers = entities.Select(AppUserFactory.Map);

        return appUsers!;
    }


    public async Task<AppUser?> GetById(string id)
    {
        var appUserEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );

        if (appUserEntity == null) return null;

        var appUser = AppUserFactory.Map(appUserEntity);
        return appUser;
    }


    public async Task<ServiceResult> CreateAsync(MemberRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _appUserRepository.ExistsAsync(x => x.Email == form.Email))
            return ServiceResult.Conflict();

        try
        {
            //var memberEntity = MemberFactory.Create(form);
            //var result = await _memberRepository.AddAsync(memberEntity!);
            //if (!result)
            //    return ServiceResult.Failed();

            //await _memberInformationRepository.AddAsync(memberEntity!.ContactInformation);

            //await _memberAddressRepository.AddAsync(memberEntity.Address);

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> UpdateAsync(string id, MemberRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        var memberEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );

        if (memberEntity == null) return ServiceResult.NotFound();

        try
        {
            //var updatedMemberEntity = MemberFactory.Update(memberEntity, form);
            //var result = await _memberRepository.UpdateAsync(updatedMemberEntity!);
            //if (!result)
            //    return ServiceResult.Failed();

            //await _memberInformationRepository.UpdateAsync(updatedMemberEntity!.ContactInformation);

            //await _memberAddressRepository.UpdateAsync(updatedMemberEntity.Address);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> RemoveAsync(string id)
    {
        var appUserEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Projects
            );

        if (appUserEntity == null) return ServiceResult.NotFound();

        try
        {
            // can't remove if connected to project
            var hasProjects = appUserEntity.Projects.Count != 0;
            if (hasProjects)
                return ServiceResult.Conflict();

            var result = await _appUserRepository.RemoveAsync(appUserEntity);
            if (!result)
                return ServiceResult.Failed();

            await _appUserProfileRepository.RemoveAsync(appUserEntity.AppUserProfile!);

            await _appUserAddressRepository.RemoveAsync(appUserEntity.AppUserAddress!);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}
