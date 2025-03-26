using Data.Repositories;

namespace Business.Services;

public class MemberService(MemberRepository memberRepository, MemberInformationRepository memberInformationRepository, MemberAddressRepository memberAddressRepository, MemberRoleRepository memberRoleRepository)
{
    private readonly MemberRepository _memberRepository = memberRepository;
    private readonly MemberInformationRepository _memberInformationRepository = memberInformationRepository;
    private readonly MemberAddressRepository _memberAddressRepository = memberAddressRepository;
    private readonly MemberRoleRepository _memberRoleRepository = memberRoleRepository;
}
