using Castle.Core.Internal;
using ClassifiedAds.Domain.Entities;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using Spl.Crm.SaleOrder.Modules.Project.Model;
using System.Reflection.Emit;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public class ProjectRepository : BaseRepository<SysMasterProjects>, IProjectRepository
{

    public ProjectRepository(SaleOrderDBContext db) : base(db)
    {

    }

    public List<SysMasterProjects> FindProjectListRawSql(string? keySearch)
    {

        string stm = string.Format(@"select DISTINCT SMP.ProjectId, SMP.ProjectName, SMP.ProjectType
                                     from Sys_Master_Projects SMP
                                     left join Sys_Master_ProjectContents SMPC on SMPC.ProjectID = SMP.ProjectID
                                     left join Sys_Master_ProjectAddresses SMPA on SMPA.ProjectID = SMP.ProjectID
                                     left join Sys_Master_ProjectLicenses SMPL on SMPL.ProjectID = SMP.ProjectID
                                     left join Sys_Admin_Bu SAB on SAB.BUCode = SMP.BUID
                                     where SMP.isDelete = 0 {0}",
                                     !keySearch.IsNullOrEmpty() ? string.Format($"and SMP.ProjectID = '{keySearch}'") : "");
        return db.SysMasterProjects.FromSqlRaw(stm).ToList();
    }

    public List<ProjectSummary> FindProjectSummaryByIdRawSql(int projectId)
    {
        string stm = string.Format(@"select SMP.ProjectID , SMU.UnitStatus , count(*) as total_unit
                                     from Sys_Master_Projects SMP
                                     left join Sys_Master_Units SMU on SMP.ProjectID = SMU.ProjectID
                                     where SMP.isDelete = 0
                                     and SMU.isDelete = 0
                                     and SMP.ProjectID = '{0}'
                                     group by SMP.ProjectID , SMU.UnitStatus",
                                     projectId);
        return db.ProjectSummary.FromSqlRaw(stm).ToList();
    }

    public List<ProjectUnits> FindProjectUnitListByIdRawSql(ProjectUnitsRequest projectUnitsRequest)
    {
        string stm = string.Format(@"SELECT
                                    MP.ProjectName,
                                    MP.ProjectType,
                                    MP.ProjectID,
                                    UN.UnitID,
                                    UN.UnitNumber
		                                + '|' + ISNULL(CTO.CustomerName,'')
                                        + '|' + ISNULL(CTO.CustomerSureName,'')
		                                + '|' + ISNULL(CO.ContractNumber,'')
		                                + '|' + ISNULL(BO.BookingNumber,'')
		                                + '|' + ISNULL(UN.HouseNumber,'') AS Critiria,
                                    UN.HouseNumber,
                                    UN.HousePlanSide,
                                    PM.ModelName,
                                    MM.ModelTypeName,
                                    UN.FurnitureFlag,
                                    UN.Block,
                                    UN.TowerID,
                                    UN.FloorID,
                                    UN.Objective,
                                    UN.AllocatedCode,
                                    UN.HouseArea,
                                    UN.SellingArea,
                                    UN.TitledeedArea,
                                    UN.BuildingArea,
                                    UN.AssetType,
                                    UN.UnitStatus,
                                    CASE
                                        WHEN CO.SaleOrderStatus = 'C' AND ISNULL(CO.isTmp,0) = 0 AND UN.UnitStatus IN (0,1) THEN 'AVAIL'
                                        WHEN CO.SaleOrderStatus = 'C' AND ISNULL(CO.isTmp,0) = 0 THEN 'CANCEL'
                                        WHEN UN.AssetType = 5 THEN 'SHOP'
                                        WHEN CC.CancelID IS NOT NULL AND CD.DiscountID IS NOT NULL THEN 'CDN'
                                        WHEN CC.CancelID IS NOT NULL THEN CASE CC.CancelType WHEN 0 THEN 'CC' WHEN 1 THEN 'CC' WHEN 2 THEN 'CU' WHEN 3 THEN 'CNU' END
                                        WHEN CC.ReferenceID IS NOT NULL THEN CASE CC.CancelType WHEN 3 THEN 'CNU' END
                                        WHEN CH.ChangeID IS NOT NULL THEN 'CN'
                                        WHEN CD.DiscountID IS NOT NULL THEN 'DC'
                                        WHEN TF.Approve6Date IS NOT NULL THEN 'COMPLETED'
                                        WHEN TF.Approve5Date IS NOT NULL THEN 'APP5'
                                        WHEN TF.Approve4Date IS NOT NULL THEN 'APP4'
                                        WHEN TF.Approve3Date IS NOT NULL THEN 'APP3'
                                        WHEN TF.Approve2Date IS NOT NULL THEN 'APP2'
                                        WHEN TF.Approve1Date IS NOT NULL THEN 'APP1'
                                        WHEN TF.ContractID IS NOT NULL THEN 'TRAN'
                                        WHEN CO.SignDate IS NOT NULL THEN 'SIGN'
                                        WHEN CO.ContractNumber IS NOT NULL THEN 'CONT'
                                        WHEN BO.approvedate IS NOT NULL THEN 'BOOKED'
                                        WHEN BO.BookingID IS NOT NULL THEN 'BOOK'
                                        ELSE 'AVAIL'
                                    END as ShowStatus,
                                    BO.BookingID,
                                    BO.QuotationNumber,
                                    BO.BookingType,
                                    BO.SellingPrice,
                                    BO.TotalSellingPrice,
                                    BO.AllowLowPrice,
                                    BO.DiscountAmount,
                                    CO.ContractNumber,
                                    CO.SignID,
                                    CO.SaleOrderStatus,
                                    ISNULL(CO.isTmp,0) as IsTmp,
                                    ISNULL(Co.isQuotation,0) as IsQuotation,
                                    ISNULL(CO.SellingPrice,0.00) as CR_SellingPrice,
                                    ISNULL(CO.TotalSellingPrice,0.00)  as CR_TotalSellingPrice,
                                    ISNULL(BP.BasePrice,0.00) as BasePrice,
                                    ISNULL(PL.StandardPrice,0.00) as PL_StandardPrice,
                                    ISNULL(PL.MarkUpPrice,0.00) as PL_MarkUpPrice,
                                    ISNULL(PL.SellingPrice,0.00) as PL_SellingPrice,
                                    ISNULL(PL.Contract2Price,0.00) as PL_Contract2Price,
                                    ISNULL(PL.LocationPrice,0.00) as PL_LocationPrice,
                                    ISNULL(PL.IncAreaPrice,0.00) as PL_IncAreaPrice,
                                    ISNULL(PL.IsHotdeal,0) as PL_IsHotdeal,
                                    ISNULL(PL.DiscountAmount,0.00) as PL_DiscountAmount,
                                    ISNULL(PL.DecorateDiscount,0.00) as PL_DecoracteDiscount,
                                    ISNULL(PL.DecorateAmount,0.00) as PL_DecoracteAmount
                                FROM Sys_Master_Units UN
                                LEFT JOIN Sys_Master_Projects MP
                                    ON MP.ProjectID = UN.ProjectID
                                    AND ISNULL(MP.isDelete,0) = 0
                                LEFT JOIN (
                                    SELECT ROW_NUMBER() OVER (PARTITION BY CO.UnitID ORDER BY CO.ModifyDate DESC) Row ,
                                           CO.*
                                    FROM Sys_REM_Contracts CO
                                    WHERE CO.ProjectID = '{0}'
                                    AND CO.SBUID = 'C0001'
                                ) CO
                                    ON CO.UnitID = UN.UnitID
                                    AND CO.SBUID = UN.SBUID
                                    AND CO.Row = 1
                                LEFT JOIN (
                                    SELECT
                                        ROW_NUMBER() OVER(PARTITION BY BP.UnitNumber ORDER BY BP.UpdateDate DESC) Row
                                        , BP.*
                                    FROM Sys_REM_BasePrice BP
                                    WHERE BP.ProjectID = '{0}'
                                    AND BP.UpdateDate <= Convert(NVARCHAR(10),GETDATE(),120) + ' 23:59:59'

                                ) BP
                                    ON BP.UnitNumber = UN.UnitNumber
                                    AND BP.Row = 1
                                LEFT JOIN (
                                    SELECT PD.*
                                        , PL.SBUID
                                        , PL.PriceListName
                                        , PL.StartDate
                                        , PL.ExpireDate
                                    FROM Sys_REM_PricelistDetails PD
                                    LEFT JOIN (
                                        SELECT *
                                        FROM Sys_REM_Pricelist PL
                                        WHERE PL.ProjectID = '{0}'
                                        AND PL.SBUID = 'C0001'
                                        AND ISNULL(PL.isDelete,0) = 0
                                        AND (PL.StartDate <= Convert(NVARCHAR(10),GETDATE(),120) + ' 23:59:59'
                                        AND (PL.ExpireDate IS NULL OR PL.ExpireDate >= Convert(NVARCHAR(10),GETDATE(),120)))
                                    ) PL
                                    ON PL.PriceListID = PD.PriceListID
                                    WHERE ISNULL(PD.isDelete,0) = 0
                                ) PL ON PL.UnitID = UN.UnitID
                                LEFT JOIN Sys_REM_Transfer TF
                                    ON TF.ContractID = CO.ContractID
                                LEFT JOIN (
                                    SELECT CC.CancelID,CC.CancelType,CC.ContractID,CC.ReferenceID
                                    FROM
                                        Sys_REM_CancelContract CC
                                    WHERE CC.ContractID in (SELECT ContractID FROM Sys_REM_Contracts WHERE ProjectID = '{0}')
                                    AND ISNULL(CC.Status,'') = ''
                                    AND ISNULL(CC.isDelete,0) = 0
                                ) CC ON CC.ContractID = CO.ContractID
                                LEFT JOIN Sys_REM_ContractsDiscount	CD
                                    ON CD.ContractID = CO.ContractID
                                    AND ISNULL(CD.ApproveStatus,'') = ''
                                    AND ISNULL(CD.isDelete,0) = 0
                                    AND CD.ContractID in (SELECT ContractID FROM Sys_REM_Contracts WHERE ProjectID = '{0}')
                                LEFT JOIN (
                                    SELECT ChangeID,ContractID
                                    FROM
                                        Sys_REM_ChangeContract CH
                                    WHERE CH.ContractID in (SELECT ContractID FROM Sys_REM_Contracts WHERE ProjectID = '{0}')
                                    AND ISNULL(CH.Status,'') = ''
                                    AND ISNULL(CH.isDelete,0) = 0
                                ) CH ON CH.ContractID = CO.ContractID
                                LEFT JOIN (
                                    SELECT BO.*
                                    FROM Sys_REM_Booking BO
                                    WHERE Bo.BookingID in (SELECT Sys_REM_Contracts.BookingID FROM Sys_REM_Contracts WHERE ProjectID = '{0}')
                                ) BO
                                    ON BO.BookingID = CO.BookingID
                                LEFT JOIN (
                                    SELECT CTO.*
                                    FROM dbo.Sys_REM_ContractOwner CTO
                                    WHERE CTO.ContractID in (SELECT ContractID FROM Sys_REM_Contracts WHERE ProjectID = '{0}')
                                    AND CTO.DeleteDate IS NULL
                                    AND CTO.Status IN ('', 'd')
                                    AND CTO.isDefault = 1
                                ) CTO
                                    ON CTO.ContractID = CO.ContractID
                                LEFT JOIN (
                                        SELECT PM.*
                                        FROM Sys_REM_ProjectModel PM
                                        WHERE PM.ProjectID = '{0}'
                                        AND ISNULL(PM.isDelete,0) = 0
                                    ) PM
                                    ON PM.ProjectID = UN.ProjectID
                                    AND UN.ModelID = PM.ModelID
                                LEFT JOIN Sys_REM_Master_ModelType MM ON MM.ModelTypeID = PM.ModelTypeID
                                WHERE ISNULL(UN.isDelete,0) = 0
                                    {1}
                                    AND UN.SBUID = 'C0001'
                                    AND UN.ProjectId = '{0}'
                                    AND UN.AssetType IN (2, 4, 6)
                                ORDER BY UnitStatus ASC ,UnitID ASC
                                OFFSET {2} ROWS 
                                FETCH NEXT {3} ROWS ONLY",
                                projectUnitsRequest.projectId,
                                !projectUnitsRequest.searchkey.IsNullOrEmpty() ? string.Format($"AND UN.UnitID = '{projectUnitsRequest.searchkey}'") : "",
                                (projectUnitsRequest.num_page - 1) * projectUnitsRequest.num_row,
                                projectUnitsRequest.num_row);

        return db.ProjectUnits.FromSqlRaw(stm).ToList();
    }
}