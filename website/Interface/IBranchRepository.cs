using System.Collections.Generic;
using website.Dto;
using website.Models;

namespace website.Interface
{
    public interface IBranchRepository
    {
        List<CompanyBranchDetail> GetBranchList();

        List<BranchAdminRegistrationDTO> getBranchAdminList();

        CompanyBranchDetail GetBranchDetail(int id);

        BranchAdminRegistrationDTO GetBranchAdmin(int id);

        bool SaveBranchAdmin(BranchAdminRegistrationDTO applicationUserDTO);


        bool SaveBranchDetail(CompanyBranchDetail branchDetail);

        bool DeleteBranchDetail(int id);

        CompanyBranchDetail GetBranchByName(string branchName);
    }
}