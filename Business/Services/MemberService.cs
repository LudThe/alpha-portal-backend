using Business.Factories;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class MemberService(MemberRepository memberRepository, MemberInformationRepository memberInformationRepository, MemberAddressRepository memberAddressRepository, ProjectRepository projectRepository)
{
    private readonly MemberRepository _memberRepository = memberRepository;
    private readonly MemberInformationRepository _memberInformationRepository = memberInformationRepository;
    private readonly MemberAddressRepository _memberAddressRepository = memberAddressRepository;
    private readonly ProjectRepository _projectRepository = projectRepository;


    public async Task<IEnumerable<Member>> GetAll()
    {
        var list = await _memberRepository.GetAllAsync(
            selector: x => MemberFactory.Map(x)!
        );

        return list.OrderBy(x => x.Id);
    }


    public async Task<Member?> GetById(int id)
    {
        var memberEntity = await _memberRepository.GetAsync(
                predicate: x => x.Id == id
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

        var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);

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
        var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);

        if (memberEntity == null) return ServiceResult.NotFound();

        try
        {
            // can't remove if connected to project
            var hasProjects = await _projectRepository.ExistsAsync(x => x.MemberId == id);
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
