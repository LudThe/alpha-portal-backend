using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, IMemberInformationRepository memberInformationRepository, IMemberAddressRepository memberAddressRepository) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IMemberInformationRepository _memberInformationRepository = memberInformationRepository;
    private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;


    public async Task<IEnumerable<Member>> GetAll()
    {
        var entities = await _memberRepository.GetAllAsync(
             orderByDescending: true,
                sortBy: x => x.FirstName,
                filterBy: null,
                i => i.ContactInformation,
                i => i.Address,
                i => i.MemberRole
            );
        var members = entities.Select(MemberFactory.Map);

        return members!;
    }


    public async Task<Member?> GetById(int id)
    {
        var memberEntity = await _memberRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.ContactInformation,
                i => i.Address,
                i => i.MemberRole
            );

        if (memberEntity == null) return null;

        var member = MemberFactory.Map(memberEntity);
        return member;
    }


    public async Task<ServiceResult> CreateAsync(MemberRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _memberRepository.ExistsAsync(x => x.ContactInformation.Email == form.Email))
            return ServiceResult.Conflict();

        try
        {
            var memberEntity = MemberFactory.Create(form);
            var result = await _memberRepository.AddAsync(memberEntity!);
            if (!result)
                return ServiceResult.Failed();

            await _memberInformationRepository.AddAsync(memberEntity!.ContactInformation);

            await _memberAddressRepository.AddAsync(memberEntity.Address);

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> UpdateAsync(int id, MemberRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        var memberEntity = await _memberRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.ContactInformation,
                i => i.Address,
                i => i.MemberRole
            );

        if (memberEntity == null) return ServiceResult.NotFound();

        try
        {
            var updatedMemberEntity = MemberFactory.Update(memberEntity, form);
            var result = await _memberRepository.UpdateAsync(updatedMemberEntity!);
            if (!result)
                return ServiceResult.Failed();

            await _memberInformationRepository.UpdateAsync(updatedMemberEntity!.ContactInformation);

            await _memberAddressRepository.UpdateAsync(updatedMemberEntity.Address);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> RemoveAsync(int id)
    {
        var memberEntity = await _memberRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Projects
            );

        if (memberEntity == null) return ServiceResult.NotFound();

        try
        {
            // can't remove if connected to project
            var hasProjects = memberEntity.Projects.Count != 0;
            if (hasProjects)
                return ServiceResult.Conflict();

            var result = await _memberRepository.RemoveAsync(memberEntity);
            if (!result)
                return ServiceResult.Failed();

            await _memberInformationRepository.RemoveAsync(memberEntity.ContactInformation);

            await _memberAddressRepository.RemoveAsync(memberEntity.Address);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}
